using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class WishListController : Controller
    {
        public ApplicationDbContext _context;
        public UserManager<Usuario> _userManager;
        public WishListController(ApplicationDbContext context, UserManager<Usuario> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [Route("/WishList")]
        public ActionResult Index()
        {
            var idUsuario = _userManager.GetUserId(User);

            var lista = (from l in _context.ListaDeseos
                         join producto in _context.Productos on 
                         l.IdProducto equals producto.IdProducto
                         where l.IdUsuario.Equals(idUsuario)
                         select new WishListViewModel { 
                             Id = producto.IdProducto,
                             Nombre = producto.NombreProducto,
                             Stock = producto.StockDisponible,
                             Imagen = producto.ImagenUrl,
                             Precio = producto.Precio,
                             IdCategoria = producto.IdCategoria
                         }).ToList();

            return View(lista);
        }
    }
}
