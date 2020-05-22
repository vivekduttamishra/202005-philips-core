using BooksWebCore.ViewModels;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class UserController : Controller
    {
        IUserAdmin userManager;
        public UserController(IUserAdmin manager)
        {
            this.userManager = manager;
        }

        public ActionResult Register()
        {
            var user = new RegisterUserViewModel();
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel vm)
        {
            

            if(ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    Password = vm.Password,
                    PhotoUrl = vm.PhotoUrl,
                    FacebookId = vm.FacebookId,
                    TwitterId = vm.TwitterId

                };
                var success = userManager.Register(user);
                if (success)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError("Email", "Duplicate Email");
                }
            }

            Response.StatusCode = 400;
            return View();
        }

        public ActionResult Login()
        {
            var user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var u = userManager.Authenticate(user.Email, user.Password);
                if (u != null)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("*", "Invalid Credentials");
                
            }

            Response.StatusCode = 400;
            return View();
        }

        public ActionResult Index()
        {
            var users = userManager.GetAllUsers();

            return View(users);
        }
       




    }
}
