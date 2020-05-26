using ConceptArchitect.BookManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.ViewModels
{
    public class NewBook: Book
    {
        [ExistingAuthor]
        public string AuthorId { get; set; }

    }
}
