using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWebCore.FrameworkApi;
using BooksWebCore.Hubs;
using BooksWebCore.ViewModels;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BooksWebCore.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : Controller
    {
        IBookManager bookManager;
        IAuthorManager authorManager;
        IHubContext<BookHub> bookHub;
        public BooksController(IBookManager bookManager,IAuthorManager authorManager, IHubContext<BookHub> bookHub)
        {
            this.bookManager = bookManager;
            this.authorManager = authorManager;
            this.bookHub = bookHub;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            await Task.Delay(3000);
            return Ok(bookManager.GetAllBooks());
        }

        [HttpGet("{id}")]
        [NullIsError404(Reason ="No Such Book")]
        [EntityNotFoundIs404]
        public async Task<IActionResult> GetBookById(string id)
        {
            await Task.Delay(3000);
            var book = bookManager.GetBookById(id);
            return Ok(book);
        }

        [HttpGet("by/{author}")]
        [Authorize]
        public async Task<IActionResult> GetBookByAuthor(string author)
        {
            await Task.Delay(3000);
            var books = bookManager.GetBooksByAuthor(author);
            return Ok(books);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult AddBook([FromBody]NewBook vm)
        {
            int x = 0; 

            if(true)
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
    

    
        
        [HttpGet("new/{title}")]
        public async Task<IActionResult> AddDummyBook(string title)
        {
            Book newBook = new Book()
            {
                Id=title.ToLower().Replace(" ","-"),
                Title=title,
                Author=authorManager.GetAuthorById("jeffrey-archer"),
                Description="Dummy Book",
                Price=100,
                CoverPage="cover.jpg"
            };

            bookManager.AddBook(newBook);

            await bookHub.Clients.All.SendAsync("BookAdded", newBook);


            return RedirectToAction("List", "Book");
        }
    
    
    }
}