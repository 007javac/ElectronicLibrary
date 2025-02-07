using ElectronicLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

            // Настройка связи между Book и Category
            modelBuilder.Entity<Book>()             
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryName)
                .HasPrincipalKey(c => c.Name)
                .IsRequired();

            // Начальные данные для категорий
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Фантастика" },
                new Category { Id = 2, Name = "Детективы" },
                new Category { Id = 3, Name = "Классика" }
            );
        }
    }
}