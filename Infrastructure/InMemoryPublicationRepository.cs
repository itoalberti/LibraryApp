using LibraryApp.Domain;

namespace LibraryApp.Repository
{
    public class InMemoryPublicationRepository : IPublicationRepository
    {
        private readonly List<Publication> _publications = new();

        private int _nextId = 1;

        public Publication Create(Publication pub)
        {
            pub.Id = _nextId++;
            _publications.Add(pub);
            return pub;
        }

        public IReadOnlyList<Publication> ListAll() => _publications.AsReadOnly();

        public Publication? GetByID(int id) => _publications.FirstOrDefault(p => p.Id == id);

        public void UpdateStatus(Publication pub, PublicationStatus status)
        {
            var existing = GetByID(pub.Id);
            if (existing is null)
                return;
            existing.UpdateStatus(status);
        }

        public void Delete(Publication pub) => _publications.Remove(pub);
    }
}
