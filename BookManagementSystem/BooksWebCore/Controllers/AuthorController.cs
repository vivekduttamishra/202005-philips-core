using BooksWebCore.FrameworkApi;
using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    [NullIsError404(Reason ="No Such Author")]      //applies or all action in this controller
    public class AuthorController : Controller
    {

        
        IAuthorManager authorManager;
        IStringLocalizer<AuthorController> localizer;
        IStringLocalizer<Shared> sharedLocalizer;
        
        //.NET core dependency injection will inject my dependency
        public AuthorController(IAuthorManager authorManager, 
            IStringLocalizer<AuthorController> localizer,
            IStringLocalizer<Shared> sharedLocalizer)
        {
            this.authorManager = authorManager;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
        }


        public IActionResult ApiList()
        {
            var authors = authorManager.GetAllAuthors();
            return Ok(authors); //return a simple api result (json/xml) back to user with status 200
        }
      
        public IActionResult List()
        {
            var authors = authorManager.GetAllAuthors();

            ViewBag.PageTitle = localizer["PageTitle"];
            ViewBag.NewAuthorLinkText = localizer["NewAuthorLinkText"];
            ViewBag.DetailsText = sharedLocalizer["AuthorDetailsLinkText"];

            return View(authors);
        }

        [NullIsError404(Reason = "Author Not Found")]  //action level attribute overrides controller level attribute
        //[RequiredParameter("id")] <--- if id is null or not given send Error 400
        public ActionResult Details(string id)
        {
            //business logic shouldn't be part of the controller
            //var author = dummyAuthors.FirstOrDefault(a => a.Id == id);

            var author = authorManager.GetAuthorById(id);
            //what if author is null?
            if (author == null)
            {
                //return E404
            }
            return View(author);
        }



        [HttpGet]
        public ActionResult Create()
        {

            //should be created by authenticated users only
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Login", "User");


            var author = new Author();
            return View(author);
        }

        [HttpPost]
        public ActionResult Create(Author author) //model binding
        {
            if(ModelState.IsValid)
            {
                authorManager.AddAuthor(author);
                //return View("Details", author);
                return RedirectToAction("List");
            }
            else
            {
                //send user back to the same page
                Response.StatusCode = 400;
                return View();
            }
            
        }

        //[NullIsError404(Reason = "Author Not Found")]
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
