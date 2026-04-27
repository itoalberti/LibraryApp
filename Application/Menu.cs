using System.Globalization;
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
                $"\n ============== 📚 📖 LIBRARY APP 📖 📚 ==============\n",
                ConsoleColor.Red
            );
            ColorChanges.WriteInColor($"OPTIONS", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n1  | Add a publication", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n2  | List all publications", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n3  | Find publication by ID", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n4  | Find publications by title", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n5  | Find books by author", ConsoleColor.Cyan);
            ColorChanges.WriteInColor(
                $"\n6  | Lend publication to library user",
                ConsoleColor.Cyan
            );
            ColorChanges.WriteInColor($"\n7  | Return item", ConsoleColor.Cyan);
            ColorChanges.WriteInColor($"\n8  | Send item to renovation", ConsoleColor.Cyan);
            ColorChanges.WriteInColor(
                $"\n9  | Remove item from library database",
                ConsoleColor.Cyan
            );
            ColorChanges.WriteInColor($"\n0  | Exit", ConsoleColor.Cyan);
            Console.Write("\nType in the option you want: ");
            string option = Console.ReadLine().Trim();

            try
            {
                switch (option)
                {
                    case "1":
                        CreatePublication();
                        Thread.Sleep(600);
                        break;
                    case "2":
                        PrintAll();
                        Thread.Sleep(600);
                        break;
                    case "3":
                        GetItemById();
                        Thread.Sleep(600);
                        break;
                    case "4":
                        GetItemsByTitle();
                        Thread.Sleep(600);
                        break;
                    case "5":
                        GetBooksByAuthor();
                        break;
                    case "6":
                        LendItem();
                        Thread.Sleep(600);
                        break;
                    case "7":
                        ReturnItem();
                        Thread.Sleep(600);
                        break;
                    case "8":
                        SendItemToRenovation();
                        Thread.Sleep(600);
                        break;
                    case "9":
                        DeleteItem();
                        break;
                    case "0":
                        return;
                }
            }
            catch (Exception e)
            {
                ColorChanges.WriteInColor($"\n{e}\n", ConsoleColor.Red);
            }
        }
    }

    public void ExecuteAction() { }

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
    }

    public void CreateBook(string title)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        Console.WriteLine("Type in the name of the author of the book:");
        string name = textInfo.ToTitleCase(Console.ReadLine().Trim().ToLower());
        Console.WriteLine("Type in the publication year of the book:");
        if (!int.TryParse(Console.ReadLine(), out int year))
            throw new FormatException("🚫 Publication year must be an integer 🚫");
        Book book = new(title, name, year);
        _controller.CreatePublication(book);
        ColorChanges.WriteInColor(
            $"\n------------------------------------------- ✔️ BOOK WAS CREATED SUCCESSFULLY! ✔️ ---------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintBook(book);
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
        ColorChanges.WriteInColor(
            $"\n----------------------------------------- ✔️ MAGAZINE WAS CREATED SUCCESSFULLY! ✔️ ------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintMagazine(magazine);
    }

    public void CreateNewspaper(string title)
    {
        Console.WriteLine("Type in the issue date of the newspaper (DD/MM/YYYY):");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", out DateOnly issueDate))
            throw new FormatException("🚫 You must type in a date 🚫");
        Newspaper newspaper = new(title, issueDate);
        _controller.CreatePublication(newspaper);
        ColorChanges.WriteInColor(
            $"\n----------------------------------------- ✔️ NEWSPAPER WAS CREATED SUCCESSFULLY! ✔️ ------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintNewspaper(newspaper);
    }

    public Publication GetItemById()
    {
        Console.Write($"Type in the item ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("🚫 ID must be an integer 🚫");
        var pub = _controller.GetPublicationByID(id);
        PrintHeader();
        PrintItem(pub);
        return pub;
    }

    public void GetItemsByTitle()
    {
        Console.Write($"Type in the publication title: ");
        string title = Console.ReadLine();
        var items = _controller.Find(p =>
            p.Title.Contains(title, StringComparison.OrdinalIgnoreCase)
        );
        ColorChanges.WriteInColor(
            $"\n----------------------------------------------------- ✔️ ITEMS FOUND ✔️ ------------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllItems(items);
    }

    public void GetBooksByAuthor()
    {
        Console.Write($"Type in the name of the author of the book: ");
        string author = Console.ReadLine().Trim();
        var books = _controller.Find(p =>
            p is Book b && b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)
        );
        ColorChanges.WriteInColor(
            $"\n------------------------------------------------------- ✔️ BOOKS FOUND -------------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        // foreach (Book book in books)
        //     PrintBook(book);
        PrintAllItems(books);
    }

    public void PrintAll()
    {
        var allItems = _controller.Find(p => p.Id != 0);
        ColorChanges.WriteInColor(
            $"\n------------------------------------------------------ ALL PUBLICATIONS ------------------------------------------------------\n",
            ConsoleColor.Green
        );
        PrintHeader();
        PrintAllItems(allItems);
    }

    public void LendItem()
    {
        Publication pub = GetItemById();
        if (pub.Status != PublicationStatus.Available)
            throw new InvalidOperationException("This item is not available for lending.");
        _controller.UpdatePublicationStatus(pub, PublicationStatus.Borrowed);
        ColorChanges.WriteInColor(
            $"\n----------------------------------------------- ✔️ ITEM SUCCESSFULLY LENT ✔️ -------------------------------------------------\n",
            ConsoleColor.Green
        );
    }

    public void ReturnItem()
    {
        Publication pub = GetItemById();
        if (pub.Status != PublicationStatus.Borrowed & pub.Status != PublicationStatus.InRenovation)
            throw new InvalidOperationException(
                "🚫 This item is already available in the library 🚫"
            );
        _controller.UpdatePublicationStatus(pub, PublicationStatus.Available);
        ColorChanges.WriteInColor(
            $"\n---------------------------------------------- ✔️ ITEM SUCCESSFULLY RETURNED ✔️ ----------------------------------------------\n",
            ConsoleColor.Green
        );
    }

    public void SendItemToRenovation()
    {
        Publication pub = GetItemById();
        if (pub.Status == PublicationStatus.InRenovation)
            throw new InvalidOperationException("🚫 This item is already in renovation 🚫");
        _controller.UpdatePublicationStatus(pub, PublicationStatus.InRenovation);
        ColorChanges.WriteInColor(
            $"\n--------------------------------------------- ✔️ ITEM WAS SENT TO RENOVATION ✔️ ----------------------------------------------\n",
            ConsoleColor.Green
        );
    }

    public void DeleteItem()
    {
        Console.Write($"Type in the ID of the book to be deleted: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("🚫 Item ID must be an integer 🚫");
        Publication pub = _controller.GetPublicationByID(id);
        _controller.DeletePublication(pub);
        ColorChanges.WriteInColor(
            $"\n✔️ {pub.GetType().Name} \"{pub.Title}\" was deleted successfully ✔️\n",
            ConsoleColor.Green
        );
    }

    public void PrintAllItems(IReadOnlyList<Publication> publications)
    {
        foreach (Publication pub in publications)
            PrintItem(pub);
        PrintFooter();
    }

    public void PrintAllBooks(IReadOnlyList<Book> books)
    {
        foreach (Book book in books)
            PrintBook(book);
        PrintFooter();
    }

    public void PrintBook(Book book) =>
        Console.WriteLine(
            $"{book.Id, 3} | {book.Title, -45}| Book      | {book.Author, -24}|            | {book.Year, 4} | {book.Status.GetDisplayName(), -14}|"
        );

    public void PrintMagazine(Magazine magazine) =>
        Console.WriteLine(
            $"{magazine.Id, 3} | {magazine.Title, -45}| Magazine  |                         | {magazine.Issue, -10} | {magazine.Year, 4} | {magazine.Status.GetDisplayName(), -14}|"
        );

    public void PrintNewspaper(Newspaper newspaper) =>
        Console.WriteLine(
            $"{newspaper.Id, 3} | {newspaper.Title, -45}| Newspaper |                         | {newspaper.IssueDate, -10} | {newspaper.Year, 4} | {newspaper.Status.GetDisplayName(), -14}|"
        );

    public void PrintItem(Publication pub)
    {
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
            default:
                throw new InvalidOperationException("🚫 Unknown publication type 🚫");
        }
    }

    public void PrintHeader() =>
        ColorChanges.WriteInColor(
            $" ID | TITLE                                        | TYPE      | AUTHOR                  |   ISSUE    | YEAR | STATUS        |\n",
            ConsoleColor.Cyan
        );

    public void PrintFooter() =>
        ColorChanges.WriteInColor(
            $"------------------------------------------------------------------------------------------------------------------------------\n",
            ConsoleColor.Green
        );
}
