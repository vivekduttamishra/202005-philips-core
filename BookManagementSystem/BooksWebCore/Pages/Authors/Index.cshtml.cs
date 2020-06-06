using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksWebCore.Pages
{
    public class AuthorListModel : PageModel
    {

        public IList<Author>    Authors { get; set; }
        IAuthorManager manager;
        public string PageTitle { get; set; }
        public AuthorListModel(IAuthorManager manager)
        {
            this.manager = manager;
        }

        public void OnGet()
        {
            Authors = manager.GetAllAuthors();
            PageTitle = "List of Authors";
        }
    }
}