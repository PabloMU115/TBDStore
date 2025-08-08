using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelRequest;

namespace TBD.Controllers
{
    public class OrdenController : Controller
    {
        ApplicationDbContext _context;
        UserManager<Usuario> _userManager;

        public OrdenController(ApplicationDbContext context,
                                  UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<OrdenRequest> listOrdenes(string idUsuario) {
            var ordenes = new List<OrdenRequest>();
            ordenes = (from o in _context.Ordenes
                       where o.IdUsuario.Equals(idUsuario)
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
                       }).OrderByDescending(p => p.fechaPedido.Month).ThenBy(p => p.fechaPedido.Day).ToList();

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
            Dictionary<String, List<PedidoRequest>> pedidos) {
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

        [Route("ordenes/all")]
        public ActionResult GetOrdenes(string idOrden)
        {
            var idUsario = _userManager.GetUserId(User);
            var ordenes = listOrdenes(idUsario);
            var pedidos = listPedidos(ordenes);
            var productos = listProductos(pedidos);
            var cantidades = listCantidades(ordenes, pedidos);
            return View(new AllPedidosRequest { ordenes = ordenes, productos = productos, cantidades = cantidades});
        }
    }
}
