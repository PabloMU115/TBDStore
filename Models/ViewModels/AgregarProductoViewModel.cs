using TBD.Models.ViewModels;
namespace TBD.Models.ViewModels
{
    public class AgregarProductoViewModel
    {
        public List<CategoriaViewModel> Categorias { get; set; }
        public List<ProductoViewModel> Productos{ get; set; }
        public List<ProveedorViewModel> Proveedores { get; set; }
    }
}
