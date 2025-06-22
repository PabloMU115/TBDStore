using Microsoft.AspNetCore.Identity;

namespace TBD.Models
{
    public class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; }
        public int Estado { get; set; }
        public String FechaCreacion { get; set; }
        public String FechaEliminacion { get; set; }
        public ICollection<WishList> ListaDeseos { get; set; }
        public ICollection<Carrito> Carrito { get; set; }
        public ICollection<Direccion> Direcciones { get; set; }
    }
}
