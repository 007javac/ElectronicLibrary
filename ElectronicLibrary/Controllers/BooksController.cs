using ElectronicLibrary.Data;
using ElectronicLibrary.Models;
using ElectronicLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(LibraryContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string category = null)
        {
            var viewModel = new CatalogViewModel
            {
                Books = category == null
                    ? await _context.Books.ToListAsync()
                    : await _context.Books.Where(b => b.CategoryName == category).ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                SelectedCategory = category
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BookViewModel
            {
                Book = new Book { PublicationDate = DateTime.Now },
                Categories = await _context.Categories.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel viewModel)
        {
            viewModel.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                if (viewModel.Book == null)
                {
                    ModelState.AddModelError("", "Данные книги не были предоставлены");
                    return View(viewModel);
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == viewModel.Book.CategoryName);

                if (category == null)
                {
                    ModelState.AddModelError("Book.CategoryName", "Выбранная категория не существует");
                    return View(viewModel);
                }

                viewModel.Book.Category = category;

                _context.Books.Add(viewModel.Book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении книги");
                ModelState.AddModelError("", $"Не удалось сохранить книгу: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            var viewModel = new BookViewModel
            {
                Book = book,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel viewModel)
        {
            if (id != viewModel.Book.Id)
            {
                return NotFound();
            }

            viewModel.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == viewModel.Book.CategoryName);

                if (category == null)
                {
                    ModelState.AddModelError("Book.CategoryName", "Выбранная категория не существует");
                    return View(viewModel);
                }

                viewModel.Book.Category = category;

                _context.Update(viewModel.Book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(viewModel.Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Произошла ошибка при сохранении. Попробуйте еще раз.");
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении книги с Id {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}