using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TBD.Models
{
    public enum EstadoPedido 
    { 
        Pendiente = 0, 
        Enviado = 1, 
        Cancelado = 2, 
        Entregado = 3 
    }

    public class Orden
    {
        [Key]
        public string idOrden { get; set; }

        [Required]
        [StringLength(255)]
        public string paypalID { get; set; }

        [Required]
        [StringLength(500)]
        public string direccion { get; set; }

        public string numeroDeGuia { get; set; } = "";

        public DateTime fechaPedido { get; set; }
        public DateTime fechaEnviado { get; set; }

        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        [JsonIgnore] // Evita ciclo al serializar Pedido → Orden
        public ICollection<Pedido> Pedidos { get; set; }

        // Clave foránea a Usuario
        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }
    }
}
