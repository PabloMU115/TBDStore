using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelRequest;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]")]
    //[Authorize(Roles = "usuario")]
    public class PedidosApiController : ControllerBase
    {
        ApplicationDbContext context;
        UserManager<Usuario> usersManager;

        public PedidosApiController(ApplicationDbContext _context, UserManager<Usuario> _userManager) {
            context = _context;
            usersManager = _userManager;
        }

        //[HttpGet("all")]
        //public IActionResult todos() {
        //    var idUsario = usersManager.GetUserId(User);
        //    var pedidos = new List<List<PedidoRequest>>();
        //    var ordenes = (from o in context.Ordenes where o.IdUsuario == idUsario
        //                   select new OrdenRequest 
        //                   {
        //                       idOrden = o.idOrden,
        //                       paypalID = o.paypalID,
        //                       direccion = o.direccion,
        //                       fechaPedido = o.fechaPedido,
        //                       fechaEnviado = o.fechaEnviado,
        //                       Estado = o.Estado,
        //                   }).ToList();
        //    foreach (var o in ordenes)
        //    {
        //        pedidos.Add((from c in context.Pedidos
        //                     where c.IdOrden == o.idOrden
        //                     select new PedidoRequest
        //                     {
        //                         idPedido = c.idPedido,
        //                         paypalID = o.paypalID,
        //                         cantidad = c.cantidad,
        //                         IdOrden = o.idOrden,
        //                         precioUnitario = c.precioUnitario,
        //                         IdProducto = c.IdProducto
        //                     }).ToList());
        //    }
        //    return Ok(new AllPedidosRequest { ordenes = ordenes, pedidos = pedidos });
        //}
    }
}
