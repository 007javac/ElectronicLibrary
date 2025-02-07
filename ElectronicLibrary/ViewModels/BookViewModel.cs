using ElectronicLibrary.Models;

namespace ElectronicLibrary.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}