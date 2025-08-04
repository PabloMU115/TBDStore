namespace TBD.Models.ModelRequest
{
    public class VentasRequest
    {
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; } = 0;
        public int cantidadVendidos { get; set; } = 0;
        public decimal ventas { get; set; } = 0;
    }

}
