using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ConceptArchitect.BookManagement
{
    [Serializable]
    [AgeRange(MinAge =5, MaxAge =110)]
    public class Author 
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(2000,MinimumLength =10,ErrorMessage ="Biography Must be between 10-2000 chars")]
        public string Biography { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeathDate { get; set; } //may be null for living authors

        [EmailAddress]
        public string  Email { get; set; } //may be null

        [ImageUrl(ValidExtensions ="jpg,gif,png")]
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
    [Serializable]
    public class Book
    {
        
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public Author Author { get; set; }
        
        [Range(0,5000)]
        [Required]
        public int Price { get; set; }

        [Required]
        [StringLength(2000,MinimumLength=10)]
        public string Description { get; set; }

        public string CoverPage { get; set; }
    }

    [Serializable]
    public class User
    {
        [Required]
        public string Name;

        [EmailAddress]
        public string Email { get; set; } //login id

        [Required]
        public string Password { get; set; } 
    }



}
