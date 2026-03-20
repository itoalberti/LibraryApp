namespace LibraryApp.Domain
{
    public class Newspaper : Publication
    {
        public DateOnly IssueDate { get; }
        public int Year => IssueDate.Year;

        // TUDO
        public Newspaper(string title, DateOnly issueDate)
            : base(title) => IssueDate = issueDate;
    }
}
