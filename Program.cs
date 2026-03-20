using LibraryApp.Controller;
using LibraryApp.Repository;

var repository = new InMemoryPublicationRepository();
var controller = new PublicationController(repository);
var menu = new Menu(controller);
menu.ShowMenu();
