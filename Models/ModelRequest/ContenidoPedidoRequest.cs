using TBD.Models.ViewModels;
namespace TBD.Models.ModelRequest
{
    public class ContenidoPedidoRequest
    {
        public string orderID { get; set; }
        public List<ContenidoPedidoViewModel> contenido { get; set; }
    }
}
