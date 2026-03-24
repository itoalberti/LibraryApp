using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using LibraryApp.Domain;
using LibraryApp.Repository;

namespace LibraryApp.Controller
{
    public class PublicationController
    {
        private readonly IPublicationRepository _repository;

        public PublicationController(IPublicationRepository repository) => _repository = repository;

        public Publication CreatePublication(Publication pub) => _repository.Create(pub);

        public IReadOnlyList<Publication> ListAllPublications()
        {
            var allPubs = _repository.ListAll();
            if (!allPubs.Any())
                throw new InvalidOperationException("🚫 No items were added to the library 🚫");
            return allPubs;
        }

        public Publication GetPublicationByID(int id) =>
            _repository.GetByID(id)
            ?? throw new KeyNotFoundException($"🚫 No item with ID {id} was found 🚫");

        public IReadOnlyList<Publication> GetByTitle(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException(
                    "🚫 You must type a term to search the library items 🚫"
                );
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var pubs = _repository
                .ListAll()
                .Where(pub =>
                    compareInfo.IndexOf(
                        pub.Title.Trim(),
                        searchTerm.Trim(),
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                    ) >= 0
                )
                .ToList();
            if (!pubs.Any())
                throw new InvalidOperationException(
                    $"🚫 No items containing \"{searchTerm}\"  in the title were found 🚫"
                );
            return pubs.AsReadOnly();
        }

        public void UpdatePublicationStatus(Publication pubToUpdate, PublicationStatus newStatus)
        {
            if (pubToUpdate.Status == newStatus)
                throw new InvalidOperationException(
                    $"🚫 Publication status is already {newStatus.ToString().ToLower()}."
                );
            _repository.UpdateStatus(pubToUpdate, newStatus);
        }

        // public void DeletePublication(Publication pubToDelete)
        // {
        //     if (pubToDelete.Status != PublicationStatus.Available)
        //         throw new InvalidOperationException(
        //             "🚫 Only items that are available in the library can be deleted 🚫"
        //         );
        //     _repository.Delete(pubToDelete);
        // }
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
