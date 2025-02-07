using ElectronicLibrary.Data;
using ElectronicLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new CatalogViewModel
            {
                Books = await _context.Books
                    .Where(b => b.IsPopular || b.IsNew)
                    .OrderByDescending(b => b.PublicationDate)
                    .Take(6)
                    .ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}