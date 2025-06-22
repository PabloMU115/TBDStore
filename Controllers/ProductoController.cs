using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TBD.Data;
using TBD.Models;
using TBD.Models.ViewModels;
using TBD.Models.ModelRequest;

namespace TBD.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Tienda/Item/{idCategoria}-{id}-{nombre}")]
        public ActionResult GetProducto(string id, string nombre, string idCategoria)
        {
            var productos = (from p in _context.Productos
                             join proveedor in _context.Proveedores on p.IdProveedor equals proveedor.IdProveedor
                             join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                             where p.IdCategoria.Equals(idCategoria) &&
                             !p.IdProducto.Equals(id)
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
                             }).OrderBy(x => Guid.NewGuid()).Take(10).ToList();

            var productoQuery = (from p in _context.Productos
                                 join proveedor in _context.Proveedores on p.IdProveedor equals proveedor.IdProveedor
                                 join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                                 where p.IdProducto == id
                                 select new ProductoViewModel
                                 {
                                     Id = p.IdProducto,
                                     Nombre = p.NombreProducto,
                                     Precio = p.Precio,
                                     Descripcion = p.Descripcion,
                                     Stock = p.StockDisponible,
                                     Imagen = p.ImagenUrl,
                                     Proveedor = proveedor.NombreProveedor,
                                     Categoria = categoria.NombreCategoria,
                                     IdProveedor = p.IdProveedor,
                                     IdCategoria = p.IdCategoria,
                                     lista = productos
                                 }).FirstOrDefault();

            return View(productoQuery);
        }

        [HttpGet]
        [Route("Tienda/Item/buscarProductos/{filter}")]
        public IActionResult BuscarProductos(string idCategoria, string busqueda, string filter, int pageNumber = 1, bool showAll = false)
        {
            int pageSize = showAll ? 0 : 9; // si showAll es true, se muestran todos los productos
            IQueryable<ProductoViewModel> productosQuery = null;
            if (!string.IsNullOrWhiteSpace(idCategoria))
            {
                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    productosQuery = (from p in _context.Productos
                                      join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                                      where p.IdCategoria.Contains(idCategoria)
                                      select new ProductoViewModel
                                      {
                                          Id = p.IdProducto,
                                          Nombre = p.NombreProducto,
                                          Precio = p.Precio,
                                          Descripcion = p.Descripcion,
                                          Stock = p.StockDisponible,
                                          Imagen = p.ImagenUrl,
                                          IdProveedor = p.IdProveedor,
                                          IdCategoria = p.IdCategoria,
                                          NombreCategoria = categoria.NombreCategoria
                                      });
                }
                else 
                {
                    productosQuery = (from p in _context.Productos
                                      join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                                      where ((p.NombreProducto.Contains(busqueda) ||
                                      p.Descripcion.Contains(busqueda))) && p.IdCategoria.Contains(idCategoria)
                                      select new ProductoViewModel
                                      {
                                          Id = p.IdProducto,
                                          Nombre = p.NombreProducto,
                                          Precio = p.Precio,
                                          Descripcion = p.Descripcion,
                                          Stock = p.StockDisponible,
                                          Imagen = p.ImagenUrl,
                                          IdProveedor = p.IdProveedor,
                                          IdCategoria = p.IdCategoria,
                                          NombreCategoria = categoria.NombreCategoria
                                      });
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    productosQuery = (from p in _context.Productos
                                      join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                                      select new ProductoViewModel
                                      {
                                          Id = p.IdProducto,
                                          Nombre = p.NombreProducto,
                                          Precio = p.Precio,
                                          Descripcion = p.Descripcion,
                                          Stock = p.StockDisponible,
                                          Imagen = p.ImagenUrl,
                                          IdProveedor = p.IdProveedor,
                                          IdCategoria = p.IdCategoria,
                                          NombreCategoria = categoria.NombreCategoria
                                      });
                }
                else 
                {
                    productosQuery = (from p in _context.Productos
                                      join categoria in _context.Categorias on p.IdCategoria equals categoria.IdCategoria
                                      where ((p.NombreProducto.Contains(busqueda) ||
                                      p.Descripcion.Contains(busqueda)))
                                      select new ProductoViewModel
                                      {
                                          Id = p.IdProducto,
                                          Nombre = p.NombreProducto,
                                          Precio = p.Precio,
                                          Descripcion = p.Descripcion,
                                          Stock = p.StockDisponible,
                                          Imagen = p.ImagenUrl,
                                          IdProveedor = p.IdProveedor,
                                          IdCategoria = p.IdCategoria,
                                          NombreCategoria = categoria.NombreCategoria
                                      });
                }
            }

            var nombre = "";

            if (!string.IsNullOrWhiteSpace(idCategoria))
            {
                nombre = _context.Categorias.Where(p => p.IdCategoria == idCategoria).FirstOrDefault().NombreCategoria;
            }

            // total de productos
            int totalProductos = productosQuery.Count();

            List<ProductoViewModel> productos = productosQuery.ToList();

            if (pageSize == 0)
            {
                // sin paginación: mostrar todos 
                switch (filter)
                {
                    case ("1"):
                        productos = productosQuery
                                .OrderBy(p => p.Nombre)
                                .ToList();
                        break;

                    case ("2"):
                        productos = productosQuery
                                .OrderByDescending(p => p.Nombre)
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
                                .OrderBy(p => p.Nombre)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                        break;

                    case ("2"):
                        productos = productosQuery
                                .OrderByDescending(p => p.Nombre)
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


            var modelo = new BusquedaProductoViewModel()
            {
                Productos = productos,
                Busqueda = busqueda,
                id = idCategoria,
                NombreCategoria = nombre,
                PageNumber = pageNumber,
                TotalPages = pageSize == 0 ? 1 : (int)Math.Ceiling(totalProductos / (double)pageSize),
                ItemsPerPage = pageSize,
                TotalItems = totalProductos,
                idCategoria = idCategoria,
                filter = filter
            };

            return View(modelo);
        }

        [Route("/GestionarProductos")]
        public ActionResult GestionarProductos()
        {
            var categorias = (from p in _context.Categorias
                              select new CategoriaViewModel
                              {
                                  IdCategoria = p.IdCategoria,
                                  NombreCategoria = p.NombreCategoria
                              }).ToList();

            var proveedores = (from p in _context.Proveedores
                              select new ProveedorViewModel
                              {
                                  IdProveedor = p.IdProveedor,
                                  NombreProveedor = p.NombreProveedor
                              }).ToList();

            var productos = (from p in _context.Productos
                             join c in _context.Categorias on p.IdCategoria equals c.IdCategoria
                             join proveedor in _context.Proveedores on p.IdProveedor equals proveedor.IdProveedor
                             select new ProductoViewModel
                             {
                                 Id = p.IdProducto,
                                 Nombre = p.NombreProducto,
                                 Precio = p.Precio,
                                 Descripcion = p.Descripcion,
                                 Stock = p.StockDisponible,
                                 Imagen = p.ImagenUrl,
                                 IdProveedor = p.IdProveedor,
                                 IdCategoria = p.IdCategoria,
                                 NombreCategoria = c.NombreCategoria,
                                 NombreProveedor = proveedor.NombreProveedor
                             }).ToList();

            var todos = new AgregarProductoViewModel()
            {
                Productos = productos,
                Categorias = categorias,
                Proveedores = proveedores
            };

            return View(todos);
        }

    }
}
