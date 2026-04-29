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
                $"\n ============== 📚 📖 LIBRARY APP 📖 📚 ==============",
                ConsoleColor.Red
            );
            ColorChanges.WriteInColor($"\nOPTIONS", ConsoleColor.Cyan);
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
                        ExecuteAction(CreatePublication, "\n✔️ Item was created successfully ✔️\n");
                        break;
                    case "2":
                        ExecuteAction(PrintAll, "\n");
                        break;
                    case "3":
                        ExecuteAction(() => GetItemById(), "\n✔️ Your item was found ✔️\n");
                        break;
                    case "4":
                        ExecuteAction(GetItemsByTitle, "\n✔️ Items found with this title ✔️\n");
                        break;
                    case "5":
                        ExecuteAction(GetBooksByAuthor, "\n✔️ Books found with this author ✔️\n");
                        break;
                    case "6":
                        ExecuteAction(LendItem, "\n✔️ Item was lent successfully ✔️\n");
                        break;
                    case "7":
                        ExecuteAction(ReturnItem, "\n✔️ Item was returned successfully ✔️\n");
                        break;
                    case "8":
                        ExecuteAction(
                            SendItemToRenovation,
                            "\n✔️ Item was sent to renovation ✔️\n"
                        );
                        break;
                    case "9":
                        ExecuteAction(DeleteItem, "\n✔️ Item was deleted sucessfully ✔️\n");
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

    public void ExecuteAction(Action action, string successMessage)
    {
        try
        {
            action();
            ColorChanges.WriteInColor($"{successMessage}", ConsoleColor.Green);
            Thread.Sleep(600);
        }
        catch (Exception e)
        {
            ColorChanges.WriteInColor($"\n{e.Message}\n", ConsoleColor.Red);
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
        PrintHeader();
        PrintBook(book);
        PrintLine();
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
        PrintHeader();
        PrintMagazine(magazine);
        PrintLine();
    }

    public void CreateNewspaper(string title)
    {
        Console.WriteLine("Type in the issue date of the newspaper (DD/MM/YYYY):");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", out DateOnly issueDate))
            throw new FormatException("🚫 You must type in a date 🚫");
        Newspaper newspaper = new(title, issueDate);
        _controller.CreatePublication(newspaper);
        PrintHeader();
        PrintNewspaper(newspaper);
        PrintLine();
    }

    public Publication GetItemById()
    {
        Console.Write($"Type in the item ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("🚫 ID must be an integer 🚫");
        var pub = _controller.GetPublicationByID(id);
        PrintHeader();
        PrintItem(pub);
        PrintLine();
        return pub;
    }

    public void GetItemsByTitle()
    {
        Console.Write($"Type in the publication title: ");
        string title = Console.ReadLine();
        var items = _controller.Find(p =>
            p.Title.Contains(title, StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(title)
        );
        PrintHeader();
        PrintAllItems(items);
        PrintLine();
    }

    public void GetBooksByAuthor()
    {
        Console.Write($"Type in the name of the author of the book: ");
        string author = Console.ReadLine().Trim();
        var books = _controller.Find(p =>
            p is Book b
            && b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(author)
        );
        PrintHeader();
        PrintAllItems(books);
        PrintLine();
    }

    public void PrintAll()
    {
        var allItems = _controller.Find(p => p.Id != 0);
        PrintHeader();
        foreach (Publication pub in allItems)
            PrintItem(pub);
        PrintLine();
    }

    public void LendItem()
    {
        Publication pub = GetItemById();
        if (pub.Status != PublicationStatus.Available)
            throw new InvalidOperationException("🚫 This item is not available for lending 🚫");
        _controller.UpdatePublicationStatus(pub, PublicationStatus.Borrowed);
    }

    public void ReturnItem()
    {
        Publication pub = GetItemById();
        if (pub.Status != PublicationStatus.Borrowed & pub.Status != PublicationStatus.InRenovation)
            throw new InvalidOperationException(
                "🚫 This item is already available in the library 🚫"
            );
        _controller.UpdatePublicationStatus(pub, PublicationStatus.Available);
    }

    public void SendItemToRenovation()
    {
        Publication pub = GetItemById();
        if (pub.Status == PublicationStatus.InRenovation)
            throw new InvalidOperationException("🚫 This item is already in renovation 🚫");
        _controller.UpdatePublicationStatus(pub, PublicationStatus.InRenovation);
    }

    public void DeleteItem()
    {
        Console.Write($"Type in the ID of the book to be deleted: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new FormatException("🚫 Item ID must be an integer 🚫");
        Publication pub = _controller.GetPublicationByID(id);
        _controller.DeletePublication(pub);
    }

    public void PrintAllItems(IReadOnlyList<Publication> publications)
    {
        foreach (Publication pub in publications)
            PrintItem(pub);
    }

    public void PrintBook(Book book) =>
        Console.Write(
            $"\n{book.Id, 3} | {book.Title, -45}| Book      | {book.Author, -24}|            | {book.Year, 4} | {book.Status.GetDisplayName(), -14}|"
        );

    public void PrintMagazine(Magazine magazine) =>
        Console.Write(
            $"\n{magazine.Id, 3} | {magazine.Title, -45}| Magazine  |                         | {magazine.Issue, -10} | {magazine.Year, 4} | {magazine.Status.GetDisplayName(), -14}|"
        );

    public void PrintNewspaper(Newspaper newspaper) =>
        Console.Write(
            $"\n{newspaper.Id, 3} | {newspaper.Title, -45}| Newspaper |                         | {newspaper.IssueDate, -10} | {newspaper.Year, 4} | {newspaper.Status.GetDisplayName(), -14}|"
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

    public void PrintHeader()
    {
        PrintLine();
        ColorChanges.WriteInColor(
            $"\n ID | TITLE                                        | TYPE      | AUTHOR                  |   ISSUE    | YEAR | STATUS        |",
            ConsoleColor.Cyan
        );
    }

    public void PrintLine() =>
        ColorChanges.WriteInColor(
            $"\n------------------------------------------------------------------------------------------------------------------------------",
            ConsoleColor.Green
        );
}
