using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
        public ActionResult List(string id)
        {
            if (id == null)
            {
                var accept = Request.Headers["accept"];
                if (accept != null)
                {
                    accept = accept.ToLower();
                    if (accept.Contains("json"))
                        id = "json";
                    else if (accept.Contains("application/xml"))
                        id = "xml";
                    else
                        id = "html";
                }
                else
                {
                    id = "json";
                }
            }

            var authors = authorManager.GetAllAuthors();

            if (id == "json")
                return Json(authors, JsonRequestBehavior.AllowGet);
            else if (id == "xml")
                return AuthorListXml(authors);
            else
                return View(authors);

        }

        private ActionResult AuthorListXml(IEnumerable<Author> authors)
        {
            var authorXml = new XElement("Authors",
                    from author in authors
                    select new XElement("Author",
                            new XAttribute("Id",author.Id),
                            new XElement("Name", author.Name),
                            new XElement("Biography", author.Biography),
                            new XElement("BirthDate", author.BirthDate),
                            new XElement("DeathDate", author.DeathDate),
                            new XElement("Email", author.Email),
                            new XElement("Photograph", author.Photograph)));

            return Content(authorXml.ToString(), "application/xml");
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
            ViewResult r = null;
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