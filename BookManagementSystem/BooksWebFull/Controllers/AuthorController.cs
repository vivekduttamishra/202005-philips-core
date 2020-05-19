using ConceptArchitect.BookManagement;
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
        Author[] authors =
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
        public ViewResult List()
        {
           
            return View(authors);
        }


        public ActionResult Details(string id)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);

            return View(author);
        }

        public ActionResult Create()
        {
            var author = new Author();
            return View(author);
        }

        public ActionResult Add(String name, String bio, String email, DateTime dob, DateTime? deathdate, string photograph)
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