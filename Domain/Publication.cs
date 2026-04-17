namespace LibraryApp.Domain
{
    public abstract class Publication
    {
        public int Id { get; internal set; }
        public string Title { get; }
        public PublicationStatus Status { get; private set; }

        public Publication(string title)
        {
            Title = title;
            Status = PublicationStatus.Available;
        }

        internal Publication(int id, string title)
        {
            Id = id;
            Title = title;
            Status = PublicationStatus.Available;
        }

        public void UpdateStatus(PublicationStatus newStatus) => Status = newStatus;
    }
}
