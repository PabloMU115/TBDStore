using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models.ModelRequest
{
    public class PedidoRequest
    {
        public string idPedido { get; set; }
        public string paypalID { get; set; }
        public int cantidad { get; set; }
        public int precioUnitario { get; set; }
        public string IdProducto { get; set; }
        public string IdOrden { get; set; }
    }
}
