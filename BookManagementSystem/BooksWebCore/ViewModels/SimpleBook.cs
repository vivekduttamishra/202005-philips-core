using ConceptArchitect.BookManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.ViewModels
{
        public class SimpleBook
        {
            public string Id { get; set; }

            [Required]

            public string Title { get; set; }

            [Required]
            [ExistingAuthor]
            public string Author { get; set; }

            [Range(5,500)]            
            public int Price { get; set; }

            [Required]
            [StringLength(2000,MinimumLength =10)]
            public string Description { get; set; }
            
            public string Tags { get; set; }

            public string CoverPage { get; set; }
        }
}
