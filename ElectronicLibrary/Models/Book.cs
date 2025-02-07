using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Название книги должно содержать от 2 до 200 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        [StringLength(100, ErrorMessage = "Имя автора не должно превышать 100 символов")]
        public string Author { get; set; }

        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        public string Description { get; set; }

        [Url(ErrorMessage = "Некорректный URL обложки")]
        public string CoverImageUrl { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        [StringLength(100, ErrorMessage = "Имя категории не должно превышать 100 символов")]
        public string CategoryName { get; set; }

        public Category? Category { get; set; }

        [Required(ErrorMessage = "Дата публикации обязательна")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный формат даты")]
        public DateTime PublicationDate { get; set; }

        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }
    }
}