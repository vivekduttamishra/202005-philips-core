using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HelloMVC.Controllers
{
    public class AdminController: Controller
    {
        int id;
        public AdminController(int id)
        {
            this.id = id;
        }

        public AdminController()
        {

        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult UserManagement()
        {
            return View();
        }

        public ViewResult ContentManagement()
        {
            return View();
        }
    }
}