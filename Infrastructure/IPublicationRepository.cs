using LibraryApp.Domain;

namespace LibraryApp.Repository
{
    public interface IPublicationRepository
    {
        Publication Create(Publication pub);
        IReadOnlyList<Publication> ListAll();
        Publication? GetByID(int id);

        // IReadOnlyList<Publication> SearchByTitle();
        void UpdateStatus(Publication pub, PublicationStatus status);
        void Delete(Publication pub);
    }
}
