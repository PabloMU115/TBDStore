using TBD.Models.ViewModels;
namespace TBD.Models.ModelRequest
{
    public class AllPedidosRequest
    {
        public List<OrdenRequest> ordenes { get; set; }
        public Dictionary<String, List<Producto>> productos { get; set; }
    }
}
