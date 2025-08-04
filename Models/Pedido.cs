using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class Pedido
    {
        [Key]
        public string idPedido { get; set; }

        public int cantidad { get; set; }
        public int precioUnitario { get; set; }

        // Clave foránea a Proveedor
        [Required]
        public string IdProducto { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto Producto { get; set; }

        [Required]
        public string IdOrden { get; set; }

        [ForeignKey(nameof(IdOrden))]
        public Orden Orden { get; set; }
    }
}
