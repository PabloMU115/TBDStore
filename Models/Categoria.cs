using System.ComponentModel.DataAnnotations;

namespace TBD.Models
{
    public class Categoria
    {
        [Key]
        public string IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}
