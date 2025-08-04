using Microsoft.AspNetCore.Mvc;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;
using TBD.Models.ModelRequest;
using Microsoft.AspNetCore.Authorization;

namespace TBD.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]
    public class ContadorVistasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContadorVistasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("DaylyVisits")]
        public ActionResult visitasDiarias()
        {
            var cantidadVistas = (from c in _context.ContadorVistas
                                  where c.fecha.Day == DateTime.Now.Day
                                  select c.cantidad).FirstOrDefault();

            return Ok(new { cantidadVistas });
        }

        [HttpGet]
        [Route("MonthlyVisits/{mes}")]
        public ActionResult visitasMensualesTotales(int mes)
        {
            var listaVistas = (from c in _context.ContadorVistas
                                  where c.fecha.Month == mes 
                                  select c).ToList().OrderBy(p => p.fecha);

            var vistasMensuales = 0;

            foreach (var c in listaVistas) {
                vistasMensuales += c.cantidad;
            }

            return Ok(new { vistasMensuales, listaVistas });
        }

        [HttpGet]
        [Route("YearlyVisits")]
        public ActionResult visitasAnuales()
        {
            var listaVistas = (from c in _context.ContadorVistas
                                  where c.fecha.Year == DateTime.Now.Year
                                  select c).ToList();

            var vistasAnuales = 0;


            foreach (var c in listaVistas) {
                vistasAnuales += c.cantidad;
            }

            return Ok(new { vistasAnuales });
        }

        [HttpPost]
        public ActionResult CrearVista([FromBody] ContadorVistas nuevaVista)
        {
            var cantidadVistas = (from c in _context.ContadorVistas
                                  where c.fecha.Day == DateTime.Now.Day
                                  && c.fecha.Month == DateTime.Now.Month
                                  && c.fecha.Year == DateTime.Now.Year
                                  select c).FirstOrDefault();
            if (cantidadVistas == null)
            {
                ContadorVistas vista = new ContadorVistas
                {
                    idVista = nuevaVista.idVista,
                    fecha = nuevaVista.fecha,
                    cantidad = 1
                };

                _context.ContadorVistas.Add(vista);
                _context.SaveChanges();
            }
            else {
                return aumentar(cantidadVistas.idVista);
            }
                return Ok();
        }

        [HttpPut]
        [Route("editar/{id}")]
        public ActionResult aumentar(string id) {
            var visitasActuales = (from c in _context.ContadorVistas
                         where c.fecha.Day == DateTime.Now.Day 
                         && c.fecha.Month == DateTime.Now.Month
                         && c.fecha.Year == DateTime.Now.Year
                         select c).FirstOrDefault();
            visitasActuales.cantidad += 1;
            _context.ContadorVistas.Update(visitasActuales);
            _context.SaveChanges();
            return Ok();
        }
    }
}
