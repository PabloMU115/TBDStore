using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;
using TBD.Models.ModelRequest;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorApiController : ControllerBase
    {
        ApplicationDbContext _context;
        public ProveedorApiController(ApplicationDbContext context) {
        _context = context;
        }

        [HttpPost]
        public ActionResult CrearProveedor([FromBody] ProveedorCreate nuevoProveedor) 
        {
            Proveedor proveedor = new Proveedor
            {
                IdProveedor = nuevoProveedor.IdProveedor,
                NombreProveedor = nuevoProveedor.NombreProveedor,
                DescripcionProveedor = nuevoProveedor.DescripcionProveedor,
                ContactoProveedor = nuevoProveedor.ContactoProveedor,
                EmailProveedor = nuevoProveedor.EmailProveedor,
                Direccion = nuevoProveedor.Direccion,
                FechaCreacion = nuevoProveedor.FechaCreacion
            };
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();

            return Ok(new { result = true, 
                proveedor = new
                {
                    nuevoProveedor.IdProveedor,
                    nuevoProveedor.NombreProveedor
                }
            });
        }

        [HttpPut("{id}")]
        public ActionResult EditarProveedor(string id, [FromBody] ProveedorUpdate editarProveedor)
        {
            Proveedor proveedor = new Proveedor 
            {
                IdProveedor = id,
                NombreProveedor = editarProveedor.NombreProveedor,
                DescripcionProveedor = editarProveedor.DescripcionProveedor,
                ContactoProveedor = editarProveedor.ContactoProveedor,
                EmailProveedor = editarProveedor.EmailProveedor,
                Direccion = editarProveedor.Direccion
            };
            _context.Proveedores.Update(proveedor);
            _context.SaveChanges();

            return Ok(new { result = true,
                proveedor = new
                {
                    editarProveedor.NombreProveedor
                }
            });
        }

        [HttpDelete]
        public ActionResult EliminarProveedor([FromBody] ProveedorRequest eliminarProveedor)
        {
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.IdProveedor == eliminarProveedor.IdProveedor);
            _context.Proveedores.Remove(proveedor);
            _context.SaveChanges();

            return Ok(new { result = true });
        }

        [HttpGet("{id}")]
        public ActionResult getProveedorPorId(string id) 
        {
            var query = _context.Proveedores.FirstOrDefault(p => p.IdProveedor.Equals(id));

            return Ok(new { result = true, 
                proveedor = new
                {
                    query.NombreProveedor,
                    query.ContactoProveedor,
                    query.DescripcionProveedor,
                    query.EmailProveedor,
                    query.Direccion,
                    query.FechaCreacion,
                } });
        }

    }
}
