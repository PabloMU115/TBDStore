using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X500;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelRequest;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class CheckoutController : Controller
    {
        ApplicationDbContext _context;
        UserManager<Usuario> _userManager;
        private string PayPalClientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        private string PayPalUrl { get; set; } = "";

        public CheckoutController(IConfiguration configuration,
                                  ApplicationDbContext context,
                                  UserManager<Usuario> userManager)
        {
            PayPalClientId = configuration["PayPalSettings:ClientId"];
            PayPalSecret = configuration["PayPalSettings:Secret"];
            PayPalUrl = configuration["PayPalSettings:Url"];
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Checkout/CreateOrder")]
        public async Task<JsonResult> CreateOrder([FromBody] List<ContenidoPedidoViewModel> data)
        {
            var total = data.Sum(c => c.precioTotal);
            
            if (data == null)
            {
                return new JsonResult(new { Id = "" });
            }

            // create the request body
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            decimal usdTotal = Math.Round(total / 506m, 2);
            amount.Add("value", usdTotal.ToString("0.00", CultureInfo.InvariantCulture));
            //amount.Add("value", total);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);


            // get access token
            string accessToken = await GetPaypalAccessToken();

            // send request
            string url = PayPalUrl + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {

                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";

                        return new JsonResult(new { Id = paypalOrderId });
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }

        private async Task<string> GetPaypalAccessToken()
        {
            string token = "";

            string url = PayPalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(PayPalClientId + ":" + PayPalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

                requestMessage.Content = new StringContent("grant_type=client_credentials",
                    null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        token = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }
            return token;
        }

        public async Task<HistorialVentas> getHistorialItem(DateTime fechaActual, string id) {
            var productoVendido = await _context.HistorialVentas.FirstOrDefaultAsync(p => p.IdProducto == id);

            if (productoVendido == null)
            {
                productoVendido = new HistorialVentas
                {
                    Id = Guid.NewGuid().ToString(),
                    cantidadVendida = 0,
                    fechaVenta = fechaActual,
                    IdProducto = id
                };
                await _context.HistorialVentas.AddAsync(productoVendido);
                await _context.SaveChangesAsync();
            }

            return productoVendido;
        }

        [HttpPost]
        [Route("Checkout/Complete")]
        public async Task<JsonResult> CompleteOrder([FromBody] ContenidoPedidoRequest data)
        {
            var orderId = data.orderID;
            if (orderId == null)
            {
                return new JsonResult("error");
            }

            // get access token
            string accessToken = await GetPaypalAccessToken();

            string url = PayPalUrl + "/v2/checkout/orders/" + orderId + "/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if (paypalOrderStatus == "COMPLETED")
                        {
                            var idUsuario = _userManager.GetUserId(User);
                            var pedido = new Pedido();

                            var fechaActual = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                            var direccion = await _context.Direcciones.FirstOrDefaultAsync(p => p.IdUsuario == idUsuario && p.esDeterminada == 1);

                            var envío = $"Nombre:{direccion.NombreUsuario}||" +
                                $"Cedula:{direccion.CedulaUsuario}||" +
                                $"Celular:{direccion.NumeroUsuario}||" +
                                $"Provincia:{direccion.Provincia}||" +
                                $"Canton:{direccion.Canton}||" +
                                $"Detalles de envío:{direccion.DetallesDireccion}";

                            var orden = new Orden
                            {
                                idOrden = Guid.NewGuid()+"",
                                paypalID = orderId,
                                fechaPedido = fechaActual,
                                direccion = envío,
                                Estado = EstadoPedido.Pendiente,
                                IdUsuario = idUsuario
                            };

                            _context.Ordenes.Add(orden);

                            foreach (var c in data.contenido)
                            {
                                if (c.cantidad > 0) 
                                {
                                    var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == c.id);
                                    var productoVendido = await getHistorialItem(fechaActual, c.id);

                                    pedido = new Pedido
                                    {
                                        idPedido = Guid.NewGuid().ToString(),
                                        cantidad = c.cantidad,
                                        precioUnitario = c.precioUnitario,
                                        IdOrden = orden.idOrden,
                                        IdProducto = c.id
                                    };

                                    _context.Pedidos.Add(pedido);

                                    //producto.cantidadVendidos += c.cantidad;
                                    //producto.StockDisponible -= c.cantidad;

                                    //productoVendido.cantidadVendida += c.cantidad;

                                    //_context.Productos.Update(producto);
                                    //_context.HistorialVentas.Update(productoVendido);
                                }
                            }
                            var carritoActual = _context.Carrito.Where(p => p.IdUsuario == _userManager.GetUserId(User)).ToList();
                            foreach (var c in carritoActual)
                            {
                                _context.Remove(c);
                            }
                            await _context.SaveChangesAsync();

                            return new JsonResult(new { status = "success", id = orden.idOrden });
                        }
                    }
                }
            }

            return new JsonResult("error");
        }

        [Route("/Checkout/Success/{idOrden}")]
        public ActionResult Success(string idOrden) {
            var pedidoActual = _context.Pedidos.Where(p => p.IdOrden == idOrden).ToList();
            List<Producto> listaProductos = [];
            List<int> listaCantidades= [];
            foreach (var c in pedidoActual)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == c.IdProducto);
                listaProductos.Add(producto);
                listaCantidades.Add(c.cantidad);
            }
            return View(new CheckoutSuccessRequest { productos = listaProductos, cantidades = listaCantidades, ordenId = idOrden });
        }
    }
}
