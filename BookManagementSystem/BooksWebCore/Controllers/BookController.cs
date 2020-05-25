using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class BookController : Controller
    {
        IBookManager manager;
        public BookController(IBookManager manager)
        {
            this.manager=manager;
        }

        public IActionResult List()
        {
            return View("Index",manager.GetAllBooks());
        }


    }
}
