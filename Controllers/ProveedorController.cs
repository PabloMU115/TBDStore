using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TBD.Controllers
{
    public class ProveedorController : Controller
    {
        ApplicationDbContext _context;
        public ProveedorController(ApplicationDbContext context) {
        _context = context;
        }


        [Route("/GestionarProveedores")]
        public ActionResult GestionarProveedores() 
        {
            var proveedores = (from p in _context.Proveedores
                               select new Proveedor 
                               { 
                               IdProveedor = p.IdProveedor,
                               NombreProveedor = p.NombreProveedor,
                               ContactoProveedor = p.ContactoProveedor,
                               DescripcionProveedor = p.DescripcionProveedor,
                               EmailProveedor = p.EmailProveedor,
                               Direccion = p.Direccion,
                               FechaCreacion = p.FechaCreacion,
                               }).ToList();

            List<int> productos = new List<int>(new int[proveedores.Count()]);

            int apuntador = 0;

            foreach (var p in proveedores)
            {
                productos[apuntador] = (from pr in _context.Productos
                             where pr.IdProveedor == p.IdProveedor
                             select new Producto { }).ToList().Count();
                apuntador += 1;
            }

            var modelo = new GestionarProveedoresViewModel
            {
                Proveedores = proveedores,
                Productos = productos
            };

            return View(modelo);
        }

    }
}
