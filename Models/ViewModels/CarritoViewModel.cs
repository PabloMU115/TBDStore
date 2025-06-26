using Microsoft.AspNetCore.Mvc;

namespace TBD.Models.ViewModels
{
    public class CarritoViewModel
    {
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public int StockProducto { get; set; }
        public string ImagenProducto { get; set; }
        public string IdCategoria { get; set; }
        public int CantidadProducto { get; set; }
    }
}
