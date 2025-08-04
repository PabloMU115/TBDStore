using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ApplicationDbContext context, 
            ILogger<HomeController> logger,
            UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("/Tienda")]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            else {
                var categorias = (from c in _context.Categorias
                                  select new Categoria
                                  {
                                      IdCategoria = c.IdCategoria,
                                      NombreCategoria = c.NombreCategoria
                                  }).ToList();

                List<string> listaId = new List<string>();
                var listaProductoViews = new List<List<ProductoViewModel>>(listaId.Count);

                foreach (var item in categorias)
                {
                    listaId.Add(item.IdCategoria);
                    listaProductoViews.Add(new List<ProductoViewModel>());
                }

                var productos = (from producto in _context.Productos
                                 join proveedor in _context.Proveedores on producto.IdProveedor equals proveedor.IdProveedor
                                 join categoria in _context.Categorias on producto.IdCategoria equals categoria.IdCategoria
                                 select new ProductoViewModel
                                 {
                                     Id = producto.IdProducto,
                                     Nombre = producto.NombreProducto,
                                     Precio = producto.Precio,
                                     Descripcion = producto.Descripcion,
                                     Stock = producto.StockDisponible,
                                     Imagen = producto.ImagenUrl,
                                     Proveedor = proveedor.NombreProveedor,
                                     Categoria = categoria.NombreCategoria,
                                     IdProveedor = producto.IdProveedor,
                                     IdCategoria = producto.IdCategoria
                                 }).ToList();

                for (int i = 0; i < listaId.Count; i++)
                {
                    var id = listaId[i];
                    foreach (var p in productos)
                    {
                        if (p.IdCategoria == id)
                        {
                            listaProductoViews[i].Add(p);
                        }
                    }
                }

                var viewModel = new TiendaViewModel
                {
                    Categorias = categorias,
                    Productos = listaProductoViews
                };

                return View(viewModel);
            }
        }

        [Route("/Admin")]
        [Authorize(Roles = "admin")]
        public ActionResult AdminDashboard() {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
