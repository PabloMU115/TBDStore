using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;

namespace TBD.Models.ViewModels
{
    public class TotalCarritoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TotalCarritoViewComponent(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var idUsuario = _userManager.GetUserId(User as ClaimsPrincipal);
            var items = (from c in _context.Carrito
                         join p in _context.Productos on c.IdProducto equals p.IdProducto
                         where p.StockDisponible > 0 && c.IdUsuario.Equals(idUsuario)
                         select c).ToList();
            var total = 0;
            foreach (var c in items) {
                total += c.Cantidad;
            }
            return View(total);
        }
    }
}
