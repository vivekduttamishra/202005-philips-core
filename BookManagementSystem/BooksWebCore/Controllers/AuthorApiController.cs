using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //<---brings in conventional routing to this controller
    public class AuthorApiController:Controller
    {
        IAuthorManager manager;
        public AuthorApiController(IAuthorManager manager)
        {
            this.manager = manager;
        }


        public IActionResult Get()
        {
            return Ok(manager.GetAllAuthors());
        }

    }
}
