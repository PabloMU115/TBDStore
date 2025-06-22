using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TBD.Models
{
    public class ProveedorCreate
    {
        public string IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string DescripcionProveedor { get; set; }
        public string ContactoProveedor { get; set; }
        public string EmailProveedor { get; set; }
        public string Direccion { get; set; }
        public String? FechaCreacion { get; set; }
    }
}
