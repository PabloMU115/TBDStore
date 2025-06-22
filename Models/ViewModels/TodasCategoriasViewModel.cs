namespace TBD.Models.ViewModels
{
    public class TodasCategoriasViewModel
    {
        public List<Categoria> Categorias { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
