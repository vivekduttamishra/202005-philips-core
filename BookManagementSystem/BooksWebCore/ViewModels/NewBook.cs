using ConceptArchitect.BookManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.ViewModels
{
    public class NewBook: Book
    {
        [Required]
        //[ExistingAuthor]
        public string AuthorId { get; set; }

    }
}
