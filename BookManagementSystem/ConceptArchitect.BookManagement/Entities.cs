using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ConceptArchitect.BookManagement
{
    [Serializable]
    [AgeRange(MinAge =5, MaxAge =110)]
    [DataContract]
    public class Author 
    {
        [UniqueAuthorId]
        [DataMember]
        public string Id { get; set; }

        [Required]
        [DataMember(IsRequired =true)]
        public string Name { get; set; }

        [Required]
        [StringLength(2000,MinimumLength =10,ErrorMessage ="Biography Must be between 10-2000 chars")]
        [DataMember(IsRequired = true)]
        public string Biography { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DataMember(IsRequired = true)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Date)]
        [DataMember(IsRequired = false)]
        public DateTime? DeathDate { get; set; } //may be null for living authors

        [EmailAddress]
        [DataMember]
        public string  Email { get; set; } //may be null

        [ImageUrl(ValidExtensions ="jpg,gif,png")]
        [DataMember]
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
        
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        
        public string Email { get; set; } //login id

        [Required]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }
        
        public string PhotoUrl { get; set; }

        public string FacebookId { get; set; }
        public string TwitterId { get; set; }

    }



}
