using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models.ModelRequest
{
    public class OrdenRequest
    {
        public string idOrden { get; set; }
        public string paypalID { get; set; }
        public string direccion { get; set; }
        public string numeroDeGuia { get; set; }
        public DateTime fechaPedido { get; set; }
        public DateTime fechaEnviado { get; set; }
        public EstadoPedido Estado { get; set; }
    }

}
