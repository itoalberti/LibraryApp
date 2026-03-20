using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using LibraryApp.Domain;
using LibraryApp.Repository;

namespace LibraryApp.Controller
{
    public class MagazineController
    {
        private readonly IPublicationRepository _magazineRepo;

        public MagazineController(IPublicationRepository magazineRepo) =>
            _magazineRepo = magazineRepo;

        // public IReadOnlyList<Magazine> GetBooksByAuthor(string searchTerm)
        // {
        //     if (string.IsNullOrWhiteSpace(searchTerm))
        //         throw new ArgumentException("🚫 You must type something to search the books 🚫");

        //     var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        //     var books = _bookRepo
        //         .ListAll()
        //         .OfType<Book>()
        //         .Where(book =>
        //             compareInfo.IndexOf(
        //                 book.Author.Trim(),
        //                 searchTerm.Trim(),
        //                 CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
        //             ) >= 0
        //         )
        //         .ToList();
        //     if (!books.Any())
        //         throw new InvalidOperationException("🚫 No books by this author were found 🚫");
        //     return books.AsReadOnly();
        // }

        // 		public Book? GetBookByISBN(isbn)
        // {
        // }

        public Magazine CreateMagazine(Magazine magazine)
        {
            if (magazine is null)
                throw new ArgumentNullException($"🚫 {nameof(magazine)} 🚫");
            return (Magazine)_magazineRepo.Create(magazine);
        }
    }
}
