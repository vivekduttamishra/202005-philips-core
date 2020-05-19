using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace BooksWebFull.Controllers
{
    public class AuthorController : Controller
    {

        IAuthorManager authorManager;

        public AuthorController()
        {
            var store = BookStore.Load(System.Web.HttpContext.Current.Server.MapPath("/App_Data/books.db"));
            var rep = new FlatFileAuthorRepository(store);
            authorManager = new SimpleAuthorManager(rep);
        }

        #region dummy data - not requried

        Author[] dummyAuthors =
           {
                new Author()
                {
                    Id="jeffrey-archer",
                    Name="Jeffrey Archer",
                    Biography="Contemporary Best Seller",
                    Email="jeffrey.archer@gmail.com",
                    BirthDate=new DateTime(1946,1,1)
                },
                new Author()
                {
                    Id="alexandre-dumas",
                    Name="Alexandre Dumas",
                    Biography="One of the altime greatest classic author in English and French",

                    BirthDate=new DateTime(1805,1,1),
                    DeathDate=new DateTime(1875,1,1)
                }
            };

        #endregion
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

        [NonAction]
        public ActionResult Add(Author author) //model binding
        {
            
            return View("Details", author);
        }

        [NonAction]
        public ActionResult Add01(String name, String bio, String email, DateTime dob, DateTime? deathdate, string photograph)
        {
            var author = new Author()
            {
                Name = name,
                Biography = bio,
                Email = email,
                Photograph = photograph,
                BirthDate = dob,
                DeathDate = deathdate
            };

            return View("Details", author);

        }
    }
}