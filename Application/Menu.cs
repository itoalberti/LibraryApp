using System.Globalization;
using System.IO.Compression;
using LibraryApp.Controller;
using LibraryApp.Domain;
using LibraryApp.UI;

public class Menu
{
    private readonly PublicationController _controller;

    public Menu(PublicationController controller) => _controller = controller;

    public void ShowMenu()
    {
        while (true)
        {
            ColorChanges.WriteInColor(
                $"\n ============= 📚 📖 LIBRARY APP 📖 📚 =============\n",
                ConsoleColor.Green
            );
            Console.WriteLine($"OPTIONS");
            Console.WriteLine($"1  | Add a publication");
            Console.WriteLine($"2  | List all publications");
            Console.WriteLine($"3  | Find publication by ID");
            Console.WriteLine($"4  | Find publications by title");
            Console.WriteLine($"5  | Find books by author");
            Console.WriteLine($"6  | Lend publication to library user");
            Console.WriteLine($"7  | Return item");
            Console.WriteLine($"8  | Send item to renovation");
            Console.WriteLine($"9  | Remove item from library database");
            Console.WriteLine($"0  | Exit");
            Console.Write("Type in the option you want: ");
            string option = Console.ReadLine().Trim();

            try
            {
                switch (option)
                {
                    case "1":
                        CreatePublication();
                        break;
                    case "2":
                        ListAllItems();
                        break;
                    case "3":
                        GetItemById();
                        break;
                    case "4":
                        // GetItemsByTitle();
                        break;
                    case "5":
                        // GetBooksByAuthor();
                        break;
                    case "6":
                        // BorrowItem();
                        break;
                    case "7":
                        // ReturnItem();
                        break;
                    case "8":
                        // SendItemToRenovation();
                        break;
                    case "9":
                        // DeleteItem();
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

    public void CreatePublication()
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        Console.WriteLine($"Choose the type of publication:");
        foreach (PublicationType t in Enum.GetValues<PublicationType>())
            Console.WriteLine($"{(int)t}) {t}");
        Console.Write($"Option: ");
        if (
            !int.TryParse(Console.ReadLine(), out int type)
            || !Enum.IsDefined(typeof(PublicationType), type)
        )
            throw new FormatException("🚫 Invalid publication type 🚫");
        Console.WriteLine($"Type in the {((PublicationType)type).ToString().ToLower()} title:");
        string title = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        var pubType = (PublicationType)type;

        switch (pubType)
        {
            case PublicationType.Book:
                CreateBook(title);
                break;
            case PublicationType.Magazine:
                CreateMagazine(title);
                break;
            case PublicationType.Newspaper:
                CreateNewspaper(title);
                break;
        }

        // Console.Write($"Type in the name of the book's author: ");
        // string author = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        // Console.Write($"Type in the publication year: ");
        // if (!int.TryParse(Console.ReadLine(), out int year))
        //     throw new FormatException("🚫 Publication year must be an integer 🚫");
        // var book = _controller.CreateBook(title, author, year);
        // ColorChanges.WriteInColor(
        //     "\n----------------------------------- ✔️ BOOK WAS CREATED SUCCESSFULLY! ✔️ -----------------------------------\n",
        //     ConsoleColor.Green
        // );

        // PrintHeader();
        // PrintBook(book);
    }

    public void CreateBook(string title)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        Console.WriteLine("Type in the name of the author of the book:");
        string name = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        Console.WriteLine("Type in the publication year of the book:");
        if (!int.TryParse(Console.ReadLine(), out int year))
            throw new FormatException("🚫 Publication year must be an integer 🚫");
        // Console.WriteLine("Type in the ISBN of the book:");
        // if (!Isbn.TryParse(Console.ReadLine(), out Isbn isbn))
        //     throw new FormatException("🚫 Invalid ISBN format 🚫");
        // Book book = new(title, name, year, isbn);
        Book book = new(title, name, year);
        _controller.CreatePublication(book);
    }

    public void CreateMagazine(string title)
    {
        Console.WriteLine("Type in the issue of the magazine:");
        if (!int.TryParse(Console.ReadLine(), out int issue))
            throw new FormatException("🚫 Issue must be an integer 🚫");
        Console.WriteLine("Type in the publication year of the magazine:");
        if (!int.TryParse(Console.ReadLine(), out int year))
            throw new FormatException("🚫 Publication year must be an integer 🚫");
        Magazine magazine = new(title, issue, year);
        _controller.CreatePublication(magazine);
    }

    public void CreateNewspaper(string title)
    {
        Console.WriteLine("Type in the issue date of the newspaper (DD/MM/YYYY):");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", out DateOnly issueDate))
            throw new FormatException("🚫 You must type in a date 🚫");
        Newspaper newspaper = new(title, issueDate);
        _controller.CreatePublication(newspaper);
    }

    public Publication GetItemById()
    {
        Console.Write($"Type in the item ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("ID must be an integer");
        var book = _controller.GetPublicationByID(id);
        // Console.WriteLine(
        ColorChanges.WriteInColor(
            $"\n----------------------------------------- ✔️ WE FOUND YOUR BOOK ✔️ -----------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        // PrintBook(book);
        ColorChanges.WriteInColor(
            $"-------------------------------------------------------------------------------------------------------------",
            ConsoleColor.Green
        );
        return book;
    }

    // public void GetItemsByTitle()
    // {
    //     Console.Write($"Type in the book title: ");
    //     string title = Console.ReadLine();
    //     var books = _controller.GetBooksByTitle(title);
    //     // Console.WriteLine(

    //     ColorChanges.WriteInColor(
    //         $"\n------------------------------------------------ BOOKS FOUND ------------------------------------------------\n",
    //         ConsoleColor.Green
    //     );
    //     PrintHeader();
    //     PrintAllBooks(books);
    // }

    // public void GetBooksByAuthor()
    // {
    //     Console.Write($"Type in the name of the author of the book: ");
    //     string author = Console.ReadLine();
    //     var books = _controller.GetBooksByAuthor(author);
    //     ColorChanges.WriteInColor(
    //         $"\n------------------------------------------------ BOOKS FOUND ------------------------------------------------\n",
    //         ConsoleColor.Green
    //     );
    //     PrintHeader();
    //     PrintAllBooks(books);
    // }

    public void ListAllItems()
    {
        var allItems = _controller.ListAllPublications();
        ColorChanges.WriteInColor(
            $"\n--------------------------------------------------- ALL PUBLICATIONS ---------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllItems(allItems);
    }

    // public void ChangeBookStatus(Publication.BookStatus bookStatus, string message)
    // {
    //     Console.Write($"Type in the book ID: ");
    //     if (!int.TryParse(Console.ReadLine(), out int id))
    //         throw new KeyNotFoundException("INVALID ID. IT MUST BE AN INTEGER");
    //     Publication book = _controller.GetBookById(id);
    //     _controller.UpdateBookStatus(book, bookStatus);
    //     ColorChanges.WriteInColor($"\n{message}\n", ConsoleColor.Green);
    // }

    // public void BorrowItem() =>
    //     ChangeBookStatus(Publication.BookStatus.Borrowed, "✔️ BOOK SUCCESSFULLY BORROWED ✔️");

    // public void ReturnItem() =>
    //     ChangeBookStatus(Publication.BookStatus.Available, "✔️ BOOK SUCCESSFULLY RETURNED ✔️");

    // public void SendItemToRenovation() =>
    //     ChangeBookStatus(Publication.BookStatus.InRenovation, "✔️ BOOK SENT TO RENOVATION ✔️");

    // public void DeleteItem()
    // {
    //     Console.Write($"Type in the ID of the book to be deleted: ");
    //     if (!int.TryParse(Console.ReadLine(), out int id))
    //         throw new KeyNotFoundException("THERE IS NOT A BOOK WITH THIS ID");
    //     Publication book = _controller.GetBookById(id);
    //     _controller.DeleteBook(book);
    //     ColorChanges.WriteInColor(
    //         $"\n✔️ BOOK \"{book.Title}\" WAS DELETED SUCCESSFULLY ✔️\n",
    //         ConsoleColor.Green
    //     );
    // }

    public void PrintAllItems(IReadOnlyList<Publication> publications)
    {
        foreach (Publication pub in publications)
            switch (pub)
            {
                case Book book:
                    PrintBook(book);
                    break;
                case Magazine magazine:
                    PrintMagazine(magazine);
                    break;
                case Newspaper newspaper:
                    PrintNewspaper(newspaper);
                    break;
            }
        ColorChanges.WriteInColor(
            $"------------------------------------------------------------------------------------------------------------------------\n",
            ConsoleColor.Green
        );
    }

    public void PrintBook(Book book) =>
        Console.WriteLine(
            $"{book.Id, 3} | {book.Title, -40}| Book      | {book.Author, -24}|            | {book.Year, 4} | {book.Status.GetDisplayName(), -14}|"
        );

    public void PrintMagazine(Magazine magazine) =>
        Console.WriteLine(
            $"{magazine.Id, 3} | {magazine.Title, -40}| Magazine  |                         | {magazine.Issue, -10} | {magazine.Year, 4} | {magazine.Status.GetDisplayName(), -14}|"
        );

    public void PrintNewspaper(Newspaper newspaper) =>
        Console.WriteLine(
            $"{newspaper.Id, 3} | {newspaper.Title, -40}| Newspaper |                         | {newspaper.IssueDate, -10} | {newspaper.Year, 4} | {newspaper.Status.GetDisplayName(), -14}|"
        );

    public void PrintHeader() =>
        ColorChanges.WriteInColor(
            // $" ID | TITLE                                   | AUTHOR                  | ISSUE | YEAR | STATUS        |\n",
            $" ID | TITLE                                   | TYPE      | AUTHOR                  |   ISSUE    | YEAR | STATUS        |\n",
            ConsoleColor.Cyan
        );
}
