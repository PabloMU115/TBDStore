using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class ProductoUpdate
    {
        [StringLength(45)]
        public string NombreProducto { get; set; }

        [Range(0, 9999999.99)]
        public decimal Precio { get; set; } = 0;

        [StringLength(350)]
        public string Descripcion { get; set; }

        [Range(0, int.MaxValue)]
        public int StockDisponible { get; set; }

        //agregar luego
        //public DateTime fechaAgregado { get; set; }

        [Url]
        [StringLength(2083)] // Limita la longitud máxima válida para URLs
        public string ImagenUrl { get; set; }

        // Clave foránea a Proveedor
        public string IdProveedor { get; set; }

        // Clave foránea a Categoria
        public string IdCategoria { get; set; }
    }
}
