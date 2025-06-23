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
    public class CarritoApiController : ControllerBase
    {
        ApplicationDbContext _context;
        UserManager<Usuario> _userManager;
        public CarritoApiController(ApplicationDbContext context, UserManager<Usuario> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> añadirAlCarrito([FromBody] CarritoCreate c)
        {
            var carrito = new Carrito 
            {
                Cantidad = c.Cantidad,
                IdProducto = c.IdProducto,
                IdUsuario = c.IdUsuario
            };

            await _context.Carrito.AddAsync(carrito);
            await _context.SaveChangesAsync();

            return Ok(new { result = true});
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> modificarCantidad(string id, [FromBody] CarritoUpdate carrito) 
        {
            var idUsuario = "4448fa3d-d307-4160-a507-8f8b166857ff";
            //var idUsuario = _userManager.GetUserId(User);
            var item = await _context.Carrito.FirstOrDefaultAsync(p => 
            p.IdProducto.Equals(id) &&
            p.IdUsuario.Equals(idUsuario));

            if (item == null) 
            {
                return NotFound(new { result = false });
            }

            item.Cantidad = carrito.cantidad;
            _context.Carrito.Update(item);
            await _context.SaveChangesAsync();

            return Ok(new { result = true});
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> eliminarItem(string id) 
        {
            var idUsuario = "4448fa3d-d307-4160-a507-8f8b166857ff";
            //var idUsuario = _userManager.GetUserId(User);
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
