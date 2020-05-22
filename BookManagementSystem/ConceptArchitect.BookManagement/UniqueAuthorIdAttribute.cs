using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class UniqueAuthorIdAttribute : ValidationAttribute
    {

        //Can't inject dependency to a validation attribute
        //IAuthorManager manager;
        //public UniqueAuthorIdAttribute(IAuthorManager manager)
        //{
        //    this.manager = manager;
        //}


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value==null)
                return ValidationResult.Success; //validation 

            var id = (string)value;
            //I need to consult Model to check if this id is existing or not
            //I need IAuthorManager object. How do I use dependency Injection

            IAuthorManager manager = (IAuthorManager) validationContext.GetService(typeof(IAuthorManager)) ;
            


            var author = manager.GetAuthorById(id);
            if (author == null)  //there is no other author with same id
                return ValidationResult.Success;  //id is unqiue
            else //oops! not unique id
                return new ValidationResult($"Id is assigned to : {author.Name}, {author.Biography}");

        }
    }
}
