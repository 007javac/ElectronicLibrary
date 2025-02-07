using ElectronicLibrary.Models;

namespace ElectronicLibrary.ViewModels
{
    public class CatalogViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string? SelectedCategory { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}