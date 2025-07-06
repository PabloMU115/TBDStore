using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CarritoController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "usuario")]
        [Route("/cart")]
        public ActionResult Index()
        {
            var idUsuario = _userManager.GetUserId(User);
            var items = (from c in _context.Carrito
                         where c.IdUsuario.Equals(idUsuario)
                         join
                     p in _context.Productos on c.IdProducto equals p.IdProducto

                         select new CarritoViewModel
                         {
                             IdProducto = p.IdProducto,
                             NombreProducto = p.NombreProducto,
                             StockProducto = p.StockDisponible,
                             PrecioProducto = p.Precio,
                             ImagenProducto = p.ImagenUrl,
                             IdCategoria = p.IdCategoria,
                             CantidadProducto = c.Cantidad
                         }).ToList();
            return View(items);
        }
    }
}
