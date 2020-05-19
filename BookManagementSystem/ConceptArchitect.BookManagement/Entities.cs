using System;
using System.Collections;
using System.Collections.Generic;

namespace ConceptArchitect.BookManagement
{
    public class Author
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? DeathDate { get; set; } //may be null for living authors

        public string  Email { get; set; } //may be null

        public string Photograph { get; set; }

        public IList<Book> Books { get; set; } = new List<Book>();


        public int Age
        {
            get
            {
                DateTime lastDate = DeathDate ?? DateTime.Now;
                var age = lastDate - BirthDate;

                return age.Days / 365; 
            }
        }

    }

    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public int Price { get; set; }

        public string Description { get; set; }

        public string CoverPage { get; set; }
    }

    public class User
    {
        public string Name;
        public string Email { get; set; } //login id
        public string Password { get; set; } 
    }



}
