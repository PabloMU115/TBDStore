using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class Carrito
    {
        //Restaurar en caso de que se quieran implementar variables de producto
        //[Key]
        //[Required]
        //[StringLength(36)]
        //public string IdCarrito { get; set; }

        // Cantidad de ese producto en el carrito
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; } = 1; // Valor por defecto

        // Clave foránea a Usuario
        [Required]
        [StringLength(20)]
        public string IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }

        // Clave foránea a Producto
        [Required]
        [StringLength(20)]
        public string IdProducto { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto Producto { get; set; }
    }
}
