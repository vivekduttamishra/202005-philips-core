using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWebCore.FrameworkApi;
using BooksWebCore.ViewModels;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebCore.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : Controller
    {
        IBookManager bookManager;
        IAuthorManager authorManager;
        public BooksController(IBookManager bookManager,IAuthorManager authorManager)
        {
            this.bookManager = bookManager;
            this.authorManager = authorManager;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(bookManager.GetAllBooks());
        }

        [HttpGet("{id}")]
        [NullIsError404(Reason ="No Such Book")]
        [EntityNotFoundIs404]
        public IActionResult GetBookById(string id)
        {
            var book = bookManager.GetBookById(id);
            return Ok(book);
        }

        [HttpGet("by/{author}")]
        public  IActionResult GetBookByAuthor(string author)
        {
            var books = bookManager.GetBooksByAuthor(author);
            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody]NewBook vm)
        {
            

            if(ModelState.IsValid)
            {
                var book = new Book()
                {
                    Id = vm.Id,
                    Title = vm.Title,
                    Price = vm.Price,
                    Description = vm.Description,
                    CoverPage = vm.CoverPage,
                    Tags = vm.Tags,
                    Author = authorManager.GetAuthorById(vm.AuthorId) 
                };

                bookManager.AddBook(book);
                vm.Id = book.Id;
                return Created($"/api/books/{book.Id}", vm);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}