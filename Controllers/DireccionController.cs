using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;

namespace TBD.Controllers
{
    public class DireccionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public DireccionController(ApplicationDbContext context,
            UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "usuario")]
        [Route("Direcciones")]
        public async Task<ActionResult> GestionarDirecciones()
        {
            var user = _userManager.GetUserId(User);
            var direcciones = _context.Direcciones.Where(p => p.IdUsuario == user).ToList().OrderByDescending(p => p.esDeterminada).ToList();
            return View(direcciones);
        }
    }
}
