using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class HistorialVentas
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        public DateTime fechaVenta { get; set; }
        public int cantidadVendida { get; set; }

        // Clave foránea a Proveedor
        [Required]
        public string IdProducto { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto Producto { get; set; }

        //Añadir de vuelta en caso de querer agregar seguimiento de que usuario a comprado que item en el sistema
        // Clave foránea a Categoria
        //[Required]
        //public string IdUsuario { get; set; }

        //[ForeignKey(nameof(IdUsuario))]
        //public Usuario Usuario { get; set; }
    }
}
