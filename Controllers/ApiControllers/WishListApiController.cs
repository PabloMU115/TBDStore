using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ModelCreate;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class WishListApiController : ControllerBase
    {
        ApplicationDbContext _context;
        UserManager<Usuario> _userManager;

        public WishListApiController(ApplicationDbContext context, 
            UserManager<Usuario> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddItemDeseo([FromBody] ItemListaDeseoCreate i) 
        {
            var idUsuario = _userManager.GetUserId(User);
            if (idUsuario == null)
            {
                return NotFound(new { result = false });
            }
            var item = new WishList 
            { 
                IdProducto = i.IdProducto,
                IdUsuario = idUsuario
            };
            await _context.ListaDeseos.AddAsync(item);
            await _context.SaveChangesAsync();

            return Ok( new { result = true});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id) 
        {
            var idUsuario = _userManager.GetUserId(User);
            if (idUsuario == null)
            {
                return NotFound(new { result = false });
            }
            var item = await _context.ListaDeseos.FirstOrDefaultAsync(
                p => p.IdProducto.Equals(id) && p.IdUsuario.Equals(idUsuario));

            if (item == null)
            {
                return NotFound(new { result = false });
            }

            return Ok( new { result = true, item});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id) 
        {
            var idUsuario = _userManager.GetUserId(User);
            if (idUsuario == null)
            {
                return NotFound(new { result = false });
            }
            var i = await _context.ListaDeseos.FirstOrDefaultAsync(p => p.IdProducto.Equals(id) && p.IdUsuario.Equals(idUsuario));
            
            if (i == null)
            {
                return NotFound(new { result = false });
            }

            _context.ListaDeseos.Remove(i);
            _context.SaveChanges();

            return Ok(new { result = true });
        }
    }


}