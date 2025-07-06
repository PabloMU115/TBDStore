using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TBD.Models
{
    public class Direccion
    {
        [Key]
        [Required]
        [StringLength(36)]
        public string IdDireccion { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(11)]
        public string CedulaUsuario { get; set; }

        [Required]
        [StringLength(8)]
        public string NumeroUsuario { get; set; }

        [StringLength(100)]
        public string provincia { get; set; }

        [StringLength(100)]
        public string canton { get; set; }

        public int esDeterminada { get; set; } = 0;

        [Required]
        [StringLength(300)]
        public string DetallesDireccion { get; set; }

        // Clave foránea a Usuario
        [Required]
        [StringLength(255)]
        public string IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }
    }
}
