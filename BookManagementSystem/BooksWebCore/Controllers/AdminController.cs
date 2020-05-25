using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class AdminController : Controller
    {

        BookManagerRecordCreator creator;
        public AdminController(BookManagerRecordCreator creator)
        {
            this.creator = creator;
        }

        public IActionResult AddWellknownAuthors()
        {
            this.creator.AddWellknownAuthors(); //will add the author to my db
            return RedirectToAction("List", "Author");
        }

        public IActionResult AddWellknownBooks()
        {
            this.creator.AddWellKnownBooks(); //will add the author to my db
            return RedirectToAction("List", "Book");
        }
    }
}
