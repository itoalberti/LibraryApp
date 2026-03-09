using System.Globalization;
using System.Runtime.CompilerServices;
using LibraryApp.Controller;
using LibraryApp.Model;
using LibraryApp.UI;

public class Menu
{
    private readonly BookController _controller;

    public Menu(BookController controller) => _controller = controller;

    public void ShowMenu()
    {
        while (true)
        {
            ColorChanges.WriteInColor(
                $"\n =========== 📚 📖 LIBRARY APP 📖 📚 ===========\n",
                ConsoleColor.Blue
            );
            Console.WriteLine($"OPTIONS");
            Console.WriteLine($"1  | Add a book");
            Console.WriteLine($"2  | List all books");
            Console.WriteLine($"3  | Find book by ID");
            Console.WriteLine($"4  | Find books by title");
            Console.WriteLine($"5  | Find books by author");
            Console.WriteLine($"6  | Lend book to library user");
            Console.WriteLine($"7  | Return book");
            Console.WriteLine($"8  | Send book to renovation");
            Console.WriteLine($"9  | Remove book from the library database");
            Console.WriteLine($"0  | Exit");
            Console.Write("Type in the option you want: ");
            string option = Console.ReadLine().Trim();

            try
            {
                switch (option)
                {
                    case "1":
                        CreateBook();
                        break;
                    case "2":
                        ListAllBooks();
                        break;
                    case "3":
                        GetBookById();
                        break;
                    case "4":
                        GetBooksByTitle();
                        break;
                    case "5":
                        GetBooksByAuthor();
                        break;
                    case "6":
                        BorrowBook();
                        break;
                    case "7":
                        ReturnBook();
                        break;
                    case "8":
                        SendBookToRenovation();
                        break;
                    case "9":
                        DeleteBook();
                        break;
                    case "0":
                        return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }

    public void CreateBook()
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        Console.Write($"\nType in the name of the book: ");
        string title = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        Console.Write($"Type in the name of the book's author: ");
        string author = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        Console.Write($"Type in the publication year: ");
        if (!int.TryParse(Console.ReadLine(), out int year))
            throw new FormatException("🚫 Publication year must be an integer 🚫");
        var book = _controller.CreateBook(title, author, year);
        ColorChanges.WriteInColor(
            "\n----------------------------------- ✔️ BOOK WAS CREATED SUCCESSFULLY! ✔️ -----------------------------------\n",
            ConsoleColor.Green
        );

        PrintHeader();
        PrintBook(book);
    }

    public Book GetBookById()
    {
        Console.Write($"Type in the book ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("ID must be an integer");
        var book = _controller.GetBookById(id);
        // Console.WriteLine(
        ColorChanges.WriteInColor(
            $"\n----------------------------------------- ✔️ WE FOUND YOUR BOOK ✔️ -----------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintBook(book);
        ColorChanges.WriteInColor(
            $"-------------------------------------------------------------------------------------------------------------",
            ConsoleColor.Green
        );
        return book;
    }

    public void GetBooksByTitle()
    {
        Console.Write($"Type in the book title: ");
        string title = Console.ReadLine();
        var books = _controller.GetBooksByTitle(title);
        // Console.WriteLine(

        ColorChanges.WriteInColor(
            $"\n------------------------------------------------ BOOKS FOUND ------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllBooks(books);
    }

    public void GetBooksByAuthor()
    {
        Console.Write($"Type in the name of the author of the book: ");
        string author = Console.ReadLine();
        var books = _controller.GetBooksByAuthor(author);
        ColorChanges.WriteInColor(
            $"\n------------------------------------------------ BOOKS FOUND ------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllBooks(books);
    }

    public void ListAllBooks()
    {
        var allBooks = _controller.ListAllBooks();
        ColorChanges.WriteInColor(
            $"\n------------------------------------------------- ALL BOOKS -------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllBooks(allBooks);
    }

    public void ChangeBookStatus(Book.BookStatus bookStatus, string message)
    {
        Console.Write($"Type in the book ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new KeyNotFoundException("INVALID ID. IT MUST BE AN INTEGER");
        Book book = _controller.GetBookById(id);
        _controller.UpdateBookStatus(book, bookStatus);
        ColorChanges.WriteInColor($"\n{message}\n", ConsoleColor.Green);
    }

    public void BorrowBook() =>
        ChangeBookStatus(Book.BookStatus.Borrowed, "✔️ BOOK SUCCESSFULLY BORROWED ✔️");

    public void ReturnBook() =>
        ChangeBookStatus(Book.BookStatus.Available, "✔️ BOOK SUCCESSFULLY RETURNED ✔️");

    public void SendBookToRenovation() =>
        ChangeBookStatus(Book.BookStatus.InRenovation, "✔️ BOOK SENT TO RENOVATION ✔️");

    public void DeleteBook()
    {
        Console.Write($"Type in the ID of the book to be deleted: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new KeyNotFoundException("THERE IS NOT A BOOK WITH THIS ID");
        Book book = _controller.GetBookById(id);
        _controller.DeleteBook(book);
        ColorChanges.WriteInColor(
            $"\n✔️ BOOK \"{book.Title}\" WAS DELETED SUCCESSFULLY ✔️\n",
            ConsoleColor.Green
        );
    }

    public void PrintAllBooks(IReadOnlyList<Book> books)
    {
        foreach (Book book in books)
            PrintBook(book);
        ColorChanges.WriteInColor(
            $"-------------------------------------------------------------------------------------------------------------\n",
            ConsoleColor.Green
        );
    }

    public void PrintBook(Book book) =>
        Console.WriteLine(
            $"{book.BookId, 3} | {book.Title, -45} | {book.Author, -30} | {book.Year, 3} | {book.Status.GetDisplayName(), -14}|"
        );

    public void PrintHeader() =>
        ColorChanges.WriteInColor(
            $" ID | TITLE                                         | AUTHOR                         | YEAR | STATUS        |\n",
            ConsoleColor.Cyan
        );
}
