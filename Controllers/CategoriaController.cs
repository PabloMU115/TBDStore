using System.Drawing.Printing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;

namespace TBD.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        { 
            _context = context;
        }

        public IQueryable<Producto> obtenerProductos(string id)
        {
            var productosQuery = (from p in _context.Productos
                                  join proveedor in _context.Proveedores
                                  on p.IdProveedor equals proveedor.IdProveedor
                                  where p.IdCategoria == id
                                  select new Producto
                                  {
                                      IdProducto = p.IdProducto,
                                      NombreProducto = p.NombreProducto,
                                      Precio = p.Precio,
                                      Descripcion = p.Descripcion,
                                      StockDisponible = p.StockDisponible,
                                      ImagenUrl = p.ImagenUrl,
                                      IdProveedor = p.IdProveedor,
                                      IdCategoria = p.IdCategoria
                                  });
            return productosQuery;
        }

        public IQueryable<Categoria> obtenerCategoriaPorId(string id)
        {
            var categoriasQuery = (from p in _context.Categorias
                                  where p.IdCategoria == id
                                  select new Categoria
                                  {
                                      IdCategoria = p.IdCategoria,
                                      NombreCategoria = p.NombreCategoria
                                  });
            return categoriasQuery;
        }

        [HttpGet]
        // GET: CategoriaController
        [Route("Tienda/Categorias/{id}-{nombre}/Page/{pageNumber}/showAll/{showAll}")]
        public ActionResult Index(string id, string nombre, string filter, int pageNumber = 1, bool showAll = false)
        {
            int pageSize = showAll ? 0 : 9; // si showAll es true, se muestran todos los productos

            var categoria = obtenerCategoriaPorId(id).FirstOrDefault();

            var productosQuery = obtenerProductos(id);

            // total de productos
            int totalProductos = productosQuery.Count();

            List<Producto> productos = productosQuery.ToList();

            if (pageSize == 0)
            {
                // sin paginación: mostrar todos 
                switch (filter)
                {
                    case ("1"):
                        productos = productosQuery
                                .OrderBy(p => p.NombreProducto)
                                .ToList();
                        break;

                    case ("2"):
                        productos = productosQuery
                                .OrderByDescending(p => p.NombreProducto)
                                .ToList();
                        break;

                    case ("3"):
                        productos = productosQuery
                                .OrderBy(p => p.Precio)
                                .ToList();
                        break;

                    case ("4"):
                        productos = productosQuery
                                .OrderByDescending(p => p.Precio)
                                .ToList();
                        break;
                }
            }
            else
            {
                // productos paginados usando Skip y Take
                switch (filter)
                {
                    case ("1"):
                        productos = productosQuery
                                .OrderBy(p => p.NombreProducto)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                        break;

                    case ("2"):
                        productos = productosQuery
                                .OrderByDescending(p => p.NombreProducto)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                        break;

                    case ("3"):
                        productos = productosQuery
                                .OrderBy(p => p.Precio)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                        break;

                    case ("4"):
                        productos = productosQuery
                                .OrderByDescending(p => p.Precio)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                        break;
                }
            }

            var modelo = new CategoriaProductoViewModel()
            {
                Categoria = categoria,
                Productos = productos,
                PageNumber = pageNumber,
                TotalPages = pageSize == 0 ? 1 : (int)Math.Ceiling(totalProductos / (double)pageSize),
                ItemsPerPage = pageSize,
                TotalItems = totalProductos,
                filter = filter
            };

            return View(modelo);
        }


        [Route("Tienda/Todas-las-Categorias/Page/{pageNumber}/showAll/{showAll}")]
        public ActionResult ListarCategorias(int pageNumber = 1, bool showAll = false)
        {
            int pageSize = showAll ? 0 : 12; // si showAll es true, se muestran todos los productos

            var categorias = (from c in _context.Categorias
                              select new Categoria
                              {
                                  IdCategoria = c.IdCategoria,
                                  NombreCategoria = c.NombreCategoria
                              });

            // total de productos
            int totalProductos = categorias.Count();

            List<Categoria> listaCategorias = categorias.ToList();

            if (pageSize == 0)
            {
                listaCategorias = categorias
                        .OrderByDescending(p => p.NombreCategoria)
                        .ToList();
            }
            else
            {
                listaCategorias = categorias
                                .OrderBy(p => p.NombreCategoria)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }

            var modelo = new TodasCategoriasViewModel()
            {
                Categorias = listaCategorias,
                PageNumber = pageNumber,
                TotalPages = pageSize == 0 ? 1 : (int)Math.Ceiling(totalProductos / (double)pageSize),
                ItemsPerPage = pageSize,
                TotalItems = totalProductos,
            };

            return View(modelo);
        }

        public JsonResult TodasCategorias()
        {
            var categorias = (from c in _context.Categorias
                              select new CategoriaViewModel
                              {
                                  IdCategoria = c.IdCategoria,
                                  NombreCategoria = c.NombreCategoria
                              }).ToList();

            return Json(new { data = categorias });
        }

        [Route("/GestionarCategorias")]
        public ActionResult GestionarCategorias()
        {
            var categorias = (from c in _context.Categorias
                              select new CategoriaViewModel
                              {
                                  IdCategoria = c.IdCategoria,
                                  NombreCategoria = c.NombreCategoria
                              }).ToList();

            List<int> cantidadProductos = new List<int>( new int[categorias.Count()]);
            int apuntador = 0;
            foreach (var c in categorias)
            {
                cantidadProductos[apuntador] = (from p in _context.Productos 
                                                where p.IdCategoria == c.IdCategoria
                                                select new Producto { }).ToList().Count();
                apuntador += 1;
            }

            var todo = new GestionarCategoriasViewModel 
            {
                Categorias = categorias,
                Productos = cantidadProductos
            };

            return View(todo);
        }

    }
}
