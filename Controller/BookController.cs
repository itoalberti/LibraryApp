using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using LibraryApp.Domain;
using LibraryApp.Repository;

namespace LibraryApp.Controller
{
    public class BookController
    {
        private readonly IPublicationRepository _bookRepo;

        public BookController(IPublicationRepository bookRepo) => _bookRepo = bookRepo;

        public IReadOnlyList<Book> GetBooksByAuthor(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("🚫 You must type something to search the books 🚫");

            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var books = _bookRepo
                .ListAll()
                .OfType<Book>()
                .Where(book =>
                    compareInfo.IndexOf(
                        book.Author.Trim(),
                        searchTerm.Trim(),
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                    ) >= 0
                )
                .ToList();
            if (!books.Any())
                throw new InvalidOperationException("🚫 No books by this author were found 🚫");
            return books.AsReadOnly();
        }

        // 		public Book? GetBookByISBN(isbn)
        // {
        // }

        public Book CreateBook(Book book)
        {
            if (book is null)
                throw new ArgumentNullException($"🚫 {nameof(book)} 🚫");
            return (Book)_bookRepo.Create(book);
        }
    }
}
