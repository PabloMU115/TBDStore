using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class ContadorVistas
    {
        // Clave foránea a Usuario
        [Key]
        [Required]
        [StringLength(255)]
        public string idVista { get; set; }

        // Clave foránea a Producto
        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public int cantidad { get; set; } = 0;

    }
}
