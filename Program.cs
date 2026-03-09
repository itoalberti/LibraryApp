using LibraryApp.Controller;
using LibraryApp.Repository;

var repository = new BookRepository();
var controller = new BookController(repository);
var menu = new Menu(controller);
menu.ShowMenu();
