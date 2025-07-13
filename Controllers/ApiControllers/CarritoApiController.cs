using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelCreate;
using TBD.Models.ModelUpdate;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = "usuario")]
    public class CarritoApiController : ControllerBase
    {
        ApplicationDbContext _context;
        UserManager<Usuario> _userManager;
        public CarritoApiController(ApplicationDbContext context, UserManager<Usuario> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> consultarItem(string id) 
        {
            var idUsuario = _userManager.GetUserId(User);
            Carrito item = _context.Carrito.FirstOrDefault(p => 
            p.IdProducto.Equals(id) &&
            p.IdUsuario.Equals(idUsuario));

            if (item == null)
            {
                return NotFound(new { result = false });
            }

            return Ok(new { result = true});
        }

        [HttpPost]
        public async Task<IActionResult> añadirAlCarrito([FromBody] CarritoCreate c)
        {
            var carrito = new Carrito 
            {
                Cantidad = c.Cantidad,
                IdProducto = c.IdProducto,
                IdUsuario = _userManager.GetUserId(User)
            };

            await _context.Carrito.AddAsync(carrito);
            await _context.SaveChangesAsync();

            return Ok(new { result = true});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> modificarCantidad(string id, [FromBody] CarritoUpdate carrito) 
        {
            var idUsuario = _userManager.GetUserId(User);
            var item = await _context.Carrito.FirstOrDefaultAsync(p => 
            p.IdProducto.Equals(id) &&
            p.IdUsuario.Equals(idUsuario));

            var cantidad = (from p in _context.Productos 
                            where p.IdProducto.Equals(item.IdProducto) 
                            select new Producto 
                            {
                                StockDisponible = p.StockDisponible
                            }).FirstOrDefault().StockDisponible;

            if (item == null) 
            {
                return NotFound(new { result = false });
            }

            if ((item.Cantidad + carrito.cantidad) <= cantidad)
            {
                item.Cantidad += carrito.cantidad;
            }
            else 
            {
                item.Cantidad = cantidad;
            }
                _context.Carrito.Update(item);
            await _context.SaveChangesAsync();

            return Ok(new { result = true});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> eliminarItem(string id) 
        {
            var idUsuario = _userManager.GetUserId(User);
            var item = await _context.Carrito.
                FirstOrDefaultAsync(p => p.IdUsuario.Equals(idUsuario) && 
                p.IdProducto.Equals(id));
            if (item == null) 
            {
                return NotFound(new { result = false});
            }

            _context.Carrito.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { result = true});
        }
    }
}
