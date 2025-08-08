using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelRequest;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ApplicationDbContext context, 
            ILogger<HomeController> logger,
            UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("Tienda")]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            else {
                var categorias = (from c in _context.Categorias
                                  select new Categoria
                                  {
                                      IdCategoria = c.IdCategoria,
                                      NombreCategoria = c.NombreCategoria
                                  }).ToList();

                List<string> listaId = new List<string>();
                var listaProductoViews = new List<List<ProductoViewModel>>(listaId.Count);

                foreach (var item in categorias)
                {
                    listaId.Add(item.IdCategoria);
                    listaProductoViews.Add(new List<ProductoViewModel>());
                }

                var productos = (from producto in _context.Productos
                                 join proveedor in _context.Proveedores on producto.IdProveedor equals proveedor.IdProveedor
                                 join categoria in _context.Categorias on producto.IdCategoria equals categoria.IdCategoria
                                 select new ProductoViewModel
                                 {
                                     Id = producto.IdProducto,
                                     Nombre = producto.NombreProducto,
                                     Precio = producto.Precio,
                                     Descripcion = producto.Descripcion,
                                     Stock = producto.StockDisponible,
                                     Imagen = producto.ImagenUrl,
                                     Proveedor = proveedor.NombreProveedor,
                                     Categoria = categoria.NombreCategoria,
                                     IdProveedor = producto.IdProveedor,
                                     IdCategoria = producto.IdCategoria
                                 }).ToList();

                for (int i = 0; i < listaId.Count; i++)
                {
                    var id = listaId[i];
                    foreach (var p in productos)
                    {
                        if (p.IdCategoria == id)
                        {
                            listaProductoViews[i].Add(p);
                        }
                    }
                }

                var viewModel = new TiendaViewModel
                {
                    Categorias = categorias,
                    Productos = listaProductoViews
                };

                return View(viewModel);
            }
        }

        public List<OrdenRequest> listOrdenesAdmin()
        {
            var ordenes = new List<OrdenRequest>();
            ordenes = (from o in _context.Ordenes
                       where o.idOrden.Length > 0
                       select new OrdenRequest
                       {
                           idOrden = o.idOrden,
                           paypalID = o.paypalID,
                           direccion = o.direccion,
                           fechaPedido = o.fechaPedido,
                           fechaEnviado = o.fechaEnviado,
                           fechaRecibido = o.fechaRecibido,
                           numeroDeGuia = o.numeroDeGuia,
                           Estado = o.Estado,
                       }).OrderByDescending(p => p.fechaPedido.Day).ThenBy(p => p.fechaPedido.Month).ToList();
            return ordenes;
        }

        public Dictionary<String, List<PedidoRequest>> listPedidos(List<OrdenRequest> ordenes)
        {
            var pedidos = new Dictionary<String, List<PedidoRequest>>();
            foreach (var o in ordenes)
            {
                pedidos[o.idOrden] = (from c in _context.Pedidos
                                      where c.IdOrden == o.idOrden
                                      select new PedidoRequest
                                      {
                                          idPedido = c.idPedido,
                                          paypalID = o.paypalID,
                                          cantidad = c.cantidad,
                                          IdOrden = o.idOrden,
                                          precioUnitario = c.precioUnitario,
                                          IdProducto = c.IdProducto
                                      }).ToList();
            }

            return pedidos;
        }

        public Dictionary<String, List<Producto>> listProductos(Dictionary<String, List<PedidoRequest>> pedidos)
        {
            var productos = new Dictionary<String, List<Producto>>();

            foreach (var o in pedidos)
            {
                var lista = new List<Producto>();
                foreach (var item in o.Value)
                {
                    lista.Add((from c in _context.Productos
                               where c.IdProducto == item.IdProducto
                               select new Producto
                               {
                                   IdProducto = c.IdProducto,
                                   NombreProducto = c.NombreProducto,
                                   ImagenUrl = c.ImagenUrl,
                                   IdCategoria = c.IdCategoria,
                                   IdProveedor = c.IdProveedor
                               }).FirstOrDefault());
                }
                productos[o.Key] = lista;
            }

            return productos;
        }

        public Dictionary<String, List<int>> listCantidades(List<OrdenRequest> ordenes,
            Dictionary<String, List<PedidoRequest>> pedidos)
        {
            var cantidades = new Dictionary<String, List<int>>();
            foreach (var item in ordenes)
            {
                var lista = new List<int>();
                foreach (var p in pedidos[item.idOrden])
                {
                    lista.Add(p.cantidad);
                }
                cantidades[item.idOrden] = lista;
            }
            return cantidades;
        }

        [Route("Admin")]
        [Authorize(Roles = "admin")]
        public ActionResult AdminDashboard() {
            var ordenes = listOrdenesAdmin();
            var pedidos = listPedidos(ordenes);
            var productos = listProductos(pedidos);
            var cantidades = listCantidades(ordenes, pedidos);
            return View(new AllPedidosRequest { ordenes = ordenes, productos = productos, cantidades = cantidades });
        }

        [Route("Admin/informes")]
        [Authorize(Roles = "admin")]
        public ActionResult ConsultarInformes()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
