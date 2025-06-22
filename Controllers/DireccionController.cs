using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TBD.Controllers
{
    public class DireccionController : Controller
    {  
        [Route("Direcciones")]
        public ActionResult GestionarDirecciones()
        {
            return View();
        }
    }
}
