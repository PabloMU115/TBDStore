namespace TBD.Models.ModelRequest
{
    public class CheckoutSuccessRequest
    {
        public List<Producto> productos { get; set; }
        public string ordenId { get; set; }
        public List<int> cantidades { get; set; }
    }
}
