﻿using BooksWebCore.FrameworkApi;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{

    [Route("api/authors")]
    //[ApiController]
    public class AuthorsController : Controller
    {
        IAuthorManager authorManager;
        public AuthorsController(IAuthorManager manager)
        {
            this.authorManager = manager;
        }

        public IActionResult GetAllAuthors()
        {
            return Ok(authorManager.GetAllAuthors());
        }

        [Route("old/{id}")]  //this path fragment is in combination with controller level Route[]
        public IActionResult GetAuthorByIdOldVersion(string id)
        {
            var author= authorManager.GetAuthorById(id);
            if (author != null)
                return Ok(author);   //Happy Path
            else 
            {
                //return NoFound(); //return status 404 without anydata
                //return NotFound($"No Author with {id} found");   //return with simple message --> text/plain
                return NotFound(new { Error = "No Author With Given Id", Id = id }); //return with structure message --> application/json
            }
        }

        [NullIsError404]
        [Route("{id}")]  //this path fragment is in combination with controller level Route[]
        public IActionResult GetAuthorById(string id)
        {
            var author = authorManager.GetAuthorById(id);
            return Ok(author);   //Happy Path
        }

        [Route("{id}/email")]
        public IActionResult GetAuthorsEmail(string id)
        {
            var author = authorManager.GetAuthorById(id);
            if (author == null)
                return NotFound(new { Error = "No Author with Given Id", Id = id });
            else
                return Ok(author.Email);

        }


        [Route("{id}/biography")]
        
        public IActionResult GetAuthorsBiography(string id)
        {
            var author = authorManager.GetAuthorById(id);
            if (author == null)
                return NotFound(new { Error = "No Author with Given Id", Id = id });
            else
                return Ok(author.Biography);

        }

        [Route("{id}/books")]
        [Authorize]  //<--- Actually this is just authenticating
        public IActionResult GetBooksByAuthors(string id)
        {
            var author = authorManager.GetAuthorById(id);
            if (author == null)
                return NotFound(new { Error = "No Author with Given Id", Id = id });
            else
                return Ok(author.Books);

        }

        [HttpPost]
        [Authorize(Roles ="Admin")] //<--- Authorization for only Admins
        public IActionResult AddAuthor([FromBody]Author author)
        {
            if(ModelState.IsValid)
            {
             
                authorManager.AddAuthor(author);
                //return Created($"/api/authors/{author.Id}",author);
                return CreatedAtAction(nameof(GetAuthorById), new { Id=author.Id},author);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
