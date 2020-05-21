using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class AuthorController : Controller
    {

        IAuthorManager authorManager;
        
        //.NET core dependency injection will inject my dependency
        public AuthorController(IAuthorManager authorManager)
        {
            this.authorManager = authorManager;
        }

      
        public ViewResult List()
        {
            var authors = authorManager.GetAllAuthors();
            return View(authors);
        }


        public ActionResult Details(string id)
        {
            //business logic shouldn't be part of the controller
            //var author = dummyAuthors.FirstOrDefault(a => a.Id == id);

            var author = authorManager.GetAuthorById(id);

            return View(author);
        }


        [HttpGet]
        public ActionResult Create()
        {
            var author = new Author();
            return View(author);
        }

        [HttpPost]
        public ActionResult Create(Author author) //model binding
        {
            authorManager.AddAuthor(author);
            //return View("Details", author);
            return RedirectToAction("List");
        }


        public ActionResult Delete(string id)
        {
            //authorManager.DeleteAuthor(id);
            //return RedirectToAction("List");
            var author = authorManager.GetAuthorById(id);
            return View(author);
        }

        [HttpPost]
        public ActionResult Delete(string id, Author author) //dummy is to change c# singature. it will get a null value
        {
            authorManager.DeleteAuthor(id);
            return RedirectToAction("List");
        }

        [HttpDelete] //Not supported in web app
        public ActionResult Delete(string id, string dummy) //dummy is to change c# singature. it will get a null value
        {
            return Content($"Author {id} deleted using DELETE");
        }


    }
}
