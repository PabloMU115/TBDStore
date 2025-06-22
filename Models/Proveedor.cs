using System.ComponentModel.DataAnnotations;

namespace TBD.Models
{
    public class Proveedor
    {
        [Key]
        public string IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string DescripcionProveedor { get; set; }
        public string ContactoProveedor { get; set; }
        public string EmailProveedor { get; set; }
        public string Direccion { get; set; }
        public String? FechaCreacion { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}
