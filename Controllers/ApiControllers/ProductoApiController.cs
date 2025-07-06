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
    [Authorize(Roles = "admin")]
    public class ProductoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string nombreCategoria(string id)
        {
            var categoria = (from p in _context.Categorias
                             where p.IdCategoria == id
                             select new Categoria
                             {
                                 IdCategoria = p.IdCategoria,
                                 NombreCategoria = p.NombreCategoria
                             }).FirstOrDefault();

            return categoria.NombreCategoria;
        }

        private string nombreProveedor(string id)
        {
            var proveedor = (from p in _context.Proveedores
                             where p.IdProveedor == id
                             select new Proveedor
                             {
                                 IdProveedor = p.IdProveedor,
                                 NombreProveedor = p.NombreProveedor
                             }).FirstOrDefault();

            return proveedor.NombreProveedor;
        }

        [HttpGet("{id}")]
        public ActionResult ConsultarProducto(string id)
        {
            var producto = (from p in _context.Productos
                            where p.IdProducto == id
                            select new Producto
                            {
                                IdProducto = p.IdProducto,
                                NombreProducto = p.NombreProducto,
                                Precio = p.Precio,
                                IdCategoria = p.IdCategoria,
                                IdProveedor = p.IdProveedor,
                                StockDisponible = p.StockDisponible,
                                ImagenUrl = p.ImagenUrl,
                                Descripcion = p.Descripcion
                            }).FirstOrDefault();

            return Ok(new { result = true, producto });
        }

        [HttpPost]
        public ActionResult CrearProducto([FromBody] ProductoCreate nuevoProducto)
        {
            Producto producto = new Producto 
            {
                IdProducto = nuevoProducto.IdProducto,
                NombreProducto = nuevoProducto.NombreProducto,
                Precio = nuevoProducto.Precio,
                StockDisponible = nuevoProducto.StockDisponible,
                ImagenUrl = nuevoProducto.ImagenUrl,
                Descripcion = nuevoProducto.Descripcion,
                IdCategoria = nuevoProducto.IdCategoria,
                IdProveedor = nuevoProducto.IdProveedor
            };

            _context.Productos.Add(producto);
            _context.SaveChanges();
            return Ok(new
            {
                result = true,
                producto = new
                {
                    ID = nuevoProducto.IdProducto,
                    Nombre = nuevoProducto.NombreProducto,
                    precio = nuevoProducto.Precio,
                    Stock = nuevoProducto.StockDisponible,
                    idCategoria = nuevoProducto.IdCategoria,
                    idProveedor = nuevoProducto.IdProveedor,
                    Categoria = nombreCategoria(nuevoProducto.IdCategoria)
                }
            });
        }


        [HttpDelete]
        public ActionResult EliminarProducto([FromBody] ProductoRequest deleteProducto)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == deleteProducto.IdProducto);
            _context.Productos.Remove(producto);
            _context.SaveChanges();


            return Ok(new { result = true, message = "Usuario eliminado correctamente." });
        }

        [HttpPut("{id}")]
        public ActionResult EditarProducto(string id, [FromBody] ProductoUpdate updateProducto)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == id);

            producto.NombreProducto = updateProducto.NombreProducto;
            producto.Precio = updateProducto.Precio;
            producto.IdCategoria = updateProducto.IdCategoria;
            producto.IdProveedor = updateProducto.IdProveedor;
            producto.StockDisponible = updateProducto.StockDisponible;
            producto.ImagenUrl = updateProducto.ImagenUrl;
            producto.Descripcion = updateProducto.Descripcion;

            _context.Productos.Update(producto);
            _context.SaveChanges();

            return Ok(new { result = true });
        }

    }
}
