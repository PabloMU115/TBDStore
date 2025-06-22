using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class Producto
    {
        [Key]
        public string IdProducto { get; set; }

        [Required]
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
        [Required]
        public string IdProveedor { get; set; }

        [ForeignKey(nameof(IdProveedor))]
        public Proveedor Proveedor { get; set; }

        // Clave foránea a Categoria
        [Required]
        public string IdCategoria { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        public Categoria Categoria { get; set; }
    }
}
