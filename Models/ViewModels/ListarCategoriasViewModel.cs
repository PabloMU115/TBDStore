using Microsoft.AspNetCore.Mvc;
using TBD.Data;


namespace TBD.Models.ViewModels
{
    public class ListarCategoriasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ListarCategoriasViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categorias = _context.Categorias
                .Select(c => new CategoriaViewModel
                {
                    IdCategoria = c.IdCategoria,
                    NombreCategoria = c.NombreCategoria
                }).ToList();

            return View(categorias);
        }
    }
}
