// using System.ComponentModel.DataAnnotations;
// using System.Globalization;
// using System.Reflection;
// using LibraryApp.Domain;
// using LibraryApp.Repository;

// namespace LibraryApp.Controller
// {
//     public class BookController
//     {
//         private readonly IPublicationRepository _bookRepo;

//         public BookController(IPublicationRepository bookRepo) => _bookRepo = bookRepo;

//         public IReadOnlyList<Book> GetBooksByAuthor(string searchTerm)
//         {
//             if (string.IsNullOrWhiteSpace(searchTerm))
//                 throw new ArgumentException("YOU MUST TYPE A NAME TO SEARCH THE BOOKS");

//             var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
//             var books = _bookRepo
//                 .ListAll()
//                 .Where(book =>
//                     compareInfo.IndexOf(
//                         book.Author.Trim(),
//                         searchTerm.Trim(),
//                         CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
//                     ) >= 0
//                 )
//                 .ToList();
//             if (!books.Any())
//                 throw new InvalidOperationException("NO BOOKS WERE FOUND WITH THIS AUTHOR");
//             return books.AsReadOnly();
//         }

//         public void UpdatePublicationStatus(Publication pubToUpdate, PublicationStatus newStatus)
//         {
//             if (pubToUpdate.Status == newStatus)
//                 throw new InvalidOperationException(
//                     $"PUBLICATION STATUS IS ALREADY {newStatus.ToString().ToUpper()}."
//                 );
//             _bookRepo.UpdateStatus(pubToUpdate, newStatus);
//         }

//         public void DeletePublication(Publication pubToDelete)
//         {
//             if (pubToDelete.Status != PublicationStatus.Available)
//                 throw new InvalidOperationException(
//                     "ONLY ITEMS THAT ARE AVAILABLE IN THE LIBRARY CAN BE DELETED"
//                 );
//             _bookRepo.Delete(pubToDelete);
//         }
//     }

//     public static class EnumExtensions
//     {
//         public static string GetDisplayName(this Enum value)
//         {
//             var member = value.GetType().GetMember(value.ToString());
//             var attribute = member[0].GetCustomAttribute<DisplayAttribute>();
//             return attribute?.Name ?? value.ToString();
//         }
//     }
// }
