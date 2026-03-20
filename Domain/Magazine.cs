namespace LibraryApp.Domain
{
    public class Magazine : Publication
    {
        public int Issue { get; }
        public int Year { get; }

        public Magazine(string title, int issue, int year)
            : base(title)
        {
            Issue = issue;
            Year = year;
        }
    }
}
