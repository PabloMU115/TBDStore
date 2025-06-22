namespace TBD.Models.ViewModels
{
    public class BusquedaProductoViewModel
    {
        public List<ProductoViewModel> Productos { get; set; }

        public string Busqueda { get; set; }
        public string id { get; set; }
        public string idCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string filter { get; set; }

        // -------------- Paginación --------------------
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

}
