using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TBD.Models
{
    public class WishList
    {
        //Restaurar en caso de que se quieran implementar variables de productos
        //[Key]
        //[Required]
        //[StringLength(36)]
        //public string IdCarrito { get; set; }

        // Clave foránea a Usuario
        [Required]
        [StringLength(20)]
        public required string IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }

        // Clave foránea a Producto
        [Required]
        [StringLength(20)]
        public required string IdProducto { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto Producto { get; set; }
    }
}
