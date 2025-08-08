using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelUpdate;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "usuario")]
    public class OrdenApiController : ControllerBase
    {
        ApplicationDbContext context;
        UserManager<Usuario> usersManager;

        public OrdenApiController(ApplicationDbContext _context, UserManager<Usuario> _userManager)
        {
            context = _context;
            usersManager = _userManager;
        }

        public async void actuaLizarHistorial(string id) {
            var pedidos = context.Pedidos.Where(p => p.IdOrden == id).ToList();
            foreach (var item in pedidos)
            {
                var producto = context.HistorialVentas.FirstOrDefault(p => p.IdProducto == item.IdProducto);

                var venta = new HistorialVentas
                {
                    Id = Guid.NewGuid() + "",
                    IdProducto = item.IdProducto,
                    fechaVenta = DateTime.Now,
                    cantidadVendida = item.cantidad
                };
                if (producto == null)
                {
                    context.HistorialVentas.Add(venta);
                }
                else 
                {
                    if (producto.fechaVenta.Month < DateTime.Now.Month)
                    {
                        context.HistorialVentas.Add(venta);
                    }
                    else 
                    {
                        producto.cantidadVendida += item.cantidad;
                        context.HistorialVentas.Update(producto);
                    }
                }
                context.SaveChanges();
            }
        }

        [HttpPut("cancel")]
        public ActionResult cancelarOrden([FromBody] string idOrden)
        {
            Orden orden = context.Ordenes.FirstOrDefault(p => p.idOrden == idOrden);
            List<Pedido> pedidos = context.Pedidos.Where(z => z.IdOrden == orden.idOrden).ToList();
            foreach (var item in pedidos)
            {
                var producto = context.Productos.FirstOrDefault(p => p.IdProducto == item.IdProducto);
                producto.StockDisponible += item.cantidad;
                context.Update(producto);
            }
            
            orden.Estado = EstadoPedido.Cancelado;
            orden.fechaCancelado = DateTime.Now;
            context.Update(orden);
            
            context.SaveChanges();
            return Ok();
        }

        [HttpPut("confirm")]
        public ActionResult confirmarOrden([FromBody] string idOrden)
        {
            Orden orden = context.Ordenes.FirstOrDefault(p => p.idOrden == idOrden);
            orden.Estado = EstadoPedido.Entregado;
            orden.fechaRecibido = DateTime.Now;
            context.Update(orden);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut("confirmarEnvio")]
        public ActionResult confirmarEnvio([FromBody] OrdenUpdate dto)
        {
            var orden = context.Ordenes.FirstOrDefault(p => p.idOrden.Equals(dto.idOrden));
            orden.fechaEnviado = DateTime.Now;
            orden.numeroDeGuia = dto.numeroDeGuia;
            orden.Estado = EstadoPedido.Enviado;
            context.Update(orden);
            context.SaveChanges();
            actuaLizarHistorial(dto.idOrden);
            return Ok();
        }

        [HttpPut("confirmarReembolso")]
        public ActionResult confirmarReembolso([FromBody] string dto)
        {
            var orden = context.Ordenes.FirstOrDefault(p => p.idOrden.Equals(dto));
            orden.fechaReembolsado = DateTime.Now;
            orden.Estado = EstadoPedido.Reembolsado;
            context.Update(orden);
            context.SaveChanges();
            return Ok();
        }

    }
}
