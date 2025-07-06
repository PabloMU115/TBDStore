using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")]
    [Authorize(Roles = "usuario")]
    public class DireccionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public DireccionApiController(ApplicationDbContext context,
            UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarDireccion(string id)
        {
            var direccion = await _context.Direcciones.FirstOrDefaultAsync(p => p.IdDireccion == id);

            if (direccion == null)
            {
                return NotFound(new { result = false });
            } 

            return Ok(new { result = true, direccion });
        }

        [HttpPost]
        public async Task<IActionResult> CrearDireccion([FromBody] DireccionCreate d) 
        {
            var idUsuario = _userManager.GetUserId(User);
            var direccion = new Direccion 
            {
                IdDireccion = Guid.NewGuid()+"",
                NombreUsuario = d.nombre,
                CedulaUsuario = d.cedula,
                NumeroUsuario = d.numero,
                DetallesDireccion = d.detalles,
                IdUsuario = idUsuario
            };

            await _context.Direcciones.AddAsync(direccion);
            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarDireccion(string id, [FromBody] DireccionUpdate d) 
        {
            var direccion = await _context.Direcciones.FirstOrDefaultAsync(p => p.IdDireccion == id);

            if (direccion == null) 
            {
                return NotFound(new { result = false });
            }

            direccion.NombreUsuario = d.nombre;
            direccion.CedulaUsuario= d.cedula;
            direccion.NumeroUsuario= d.numero;
            direccion.DetallesDireccion= d.detalles;

            _context.Direcciones.Update(direccion);
            await _context.SaveChangesAsync();

            return Ok(new { result = true});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDireccion(String id) 
        {
            var direccion = await _context.Direcciones.FirstOrDefaultAsync(p => p.IdDireccion == id);

            if (direccion == null)
            {
                return NotFound(new { result = false });
            }

            _context.Direcciones.Remove(direccion);
            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }
    }
}
