namespace LibraryApp.Domain
{
    public abstract class Publication
    {
        public int Id { get; internal set; } // defined in the repository, so the "set" property
        public string Title { get; }
        public PublicationStatus Status { get; private set; } // status is changed along the code execution, so the "set" property

        // used by the application (MVC/Controller)
        public Publication(string title)
        {
            Title = title;
            Status = PublicationStatus.Available;
        }

        // used by the repository (InMemoryPublicationRepository) to create a new Publication with assigned ID
        internal Publication(int id, string title)
        {
            Id = id;
            Title = title;
            Status = PublicationStatus.Available;
        }

        public void UpdateStatus(PublicationStatus newStatus) => Status = newStatus;
    }
}
