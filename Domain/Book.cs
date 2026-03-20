namespace LibraryApp.Domain
{
    public class Book : Publication
    {
        public string Author { get; }
        public int Year { get; }

        // public Isbn Isbn { get; }

        // public Book(string title, string author, int year, Isbn isbn)
        public Book(string title, string author, int year)
            : base(title)
        {
            Author = author;
            Year = year;
            // Isbn = isbn;
        }
    }
}
