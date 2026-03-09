using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using LibraryApp.Model;
using LibraryApp.Repository;
using static LibraryApp.Model.Book;

namespace LibraryApp.Controller
{
    public class BookController
    {
        private readonly BookRepository _repository;

        public BookController(BookRepository repository) => _repository = repository;

        public Book CreateBook(string title, string author, int year)
        {
            if (CheckBooksExistence(title, author))
                throw new InvalidOperationException(
                    "A BOOK WITH THIS TITLE BY THE SAME AUTHOR ALREADY EXISTS"
                );
            Book newBook = new(title, author, year);
            return _repository.CreateBook(newBook);
        }

        public IReadOnlyList<Book> ListAllBooks()
        {
            var allBooks = _repository.ListAllBooks();
            if (!allBooks.Any())
                throw new InvalidOperationException("NO BOOKS WERE ADDED TO THE LIBRARY");
            return allBooks;
        }

        public Book GetBookById(int id) =>
            _repository.GetBookById(id)
            ?? throw new KeyNotFoundException($"NO BOOK WITH ID {id} WAS FOUND");

        public IReadOnlyList<Book> GetBooksByTitle(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("YOU MUST TYPE A NAME TO SEARCH THE BOOKS");

            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var books = _repository
                .ListAllBooks()
                .Where(book =>
                    compareInfo.IndexOf(
                        book.Title.Trim(),
                        searchTerm.Trim(),
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                    ) >= 0
                )
                .ToList();
            if (!books.Any())
                throw new InvalidOperationException("NO BOOKS WERE FOUND WITH THIS TITLE");
            return books.AsReadOnly();
        }

        public IReadOnlyList<Book> GetBooksByAuthor(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("YOU MUST TYPE A NAME TO SEARCH THE BOOKS");

            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var books = _repository
                .ListAllBooks()
                .Where(book =>
                    compareInfo.IndexOf(
                        book.Author.Trim(),
                        searchTerm.Trim(),
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                    ) >= 0
                )
                .ToList();
            if (!books.Any())
                throw new InvalidOperationException("NO BOOKS WERE FOUND WITH THIS AUTHOR");
            return books.AsReadOnly();
        }

        public void UpdateBookStatus(Book bookToUpdate, BookStatus newStatus)
        {
            if (bookToUpdate.Status == newStatus)
                throw new InvalidOperationException(
                    $"BOOK STATUS IS ALREADY {newStatus.ToString().ToUpper()}."
                );
            _repository.UpdateBookStatus(bookToUpdate, newStatus);
        }

        public void DeleteBook(Book bookToDelete)
        {
            if (bookToDelete.Status != BookStatus.Available)
                throw new InvalidOperationException(
                    "ONLY BOOKS THAT ARE AVAILABLE IN THE LIBRARY CAN BE DELETED"
                );
            _repository.DeleteBook(bookToDelete);
        }

        private bool CheckBooksExistence(string title, string author)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            return (
                _repository
                    .ListAllBooks()
                    .Any(book =>
                        compareInfo.IndexOf(
                            book.Title.Trim() ?? string.Empty,
                            title.Trim(),
                            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                        ) >= 0
                    )
                && _repository
                    .ListAllBooks()
                    .Any(book =>
                        compareInfo.IndexOf(
                            book.Author.Trim() ?? string.Empty,
                            author.Trim(),
                            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                        ) >= 0
                    )
            );
        }
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString());
            var attribute = member[0].GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }
    }
}
