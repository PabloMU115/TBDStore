using System.Drawing.Printing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;
using TBD.Models.ModelRequest;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult CrearCategoria([FromBody] CategoriaCreate nuevaCategoria)
        {
            Categoria categoria = new Categoria 
            {
                IdCategoria = nuevaCategoria.IdCategoria,
                NombreCategoria = nuevaCategoria.NombreCategoria
            };

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return Ok(new { result = true,
                categoria = new
                {
                    idCategoria = nuevaCategoria.IdCategoria,
                    nombreCategoria = nuevaCategoria.NombreCategoria
                }
            });
        }

        [HttpDelete]
        public ActionResult EliminarCategoria([FromBody] CategoriaRequest deleteCategoria)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.IdCategoria == deleteCategoria.IdCategoria);

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(new
            {
                result = true
            });
        }

        [HttpPut("{id}")]
        public ActionResult EditarCategoria(string id, [FromBody]CategoriaUpdate updateCategoria)
        {
            Categoria categoria = new Categoria 
            {
            IdCategoria = id,
            NombreCategoria = updateCategoria.NombreCategoria
            };

            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return Ok(new { result = true} );
        }



    }
}
