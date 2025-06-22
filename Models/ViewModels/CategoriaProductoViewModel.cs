namespace TBD.Models.ViewModels
{
    public class CategoriaProductoViewModel
    {
        public Categoria Categoria{ get; set; }
        public List<Producto> Productos { get; set; }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }

        public string filter{ get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
