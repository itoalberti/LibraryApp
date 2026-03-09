using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Model
{
    public class Book
    {
        public int BookId { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }
        public BookStatus Status { get; private set; }

        public enum BookStatus
        {
            Available = 1,
            Reserved = 2,
            Borrowed = 3,

            [Display(Name = "In Renovation")]
            InRenovation = 4,
        }

        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
        }

        public void SetIdStatus(int bookId)
        {
            BookId = bookId;
            Status = BookStatus.Available;
        }

        public void UpdateBookStatus(BookStatus newStatus) => Status = newStatus;
    }
}
