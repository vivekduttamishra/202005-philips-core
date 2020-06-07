using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class ExistingAuthorAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (true)
                return ValidationResult.Success;
            //if (value == null)
             //   return new ValidationResult("Author Name is Required");

            var authorId = value.ToString().ToLower().Replace(' ', '-');

            
            var authorManager = (IAuthorManager)validationContext.GetService(typeof(IAuthorManager));

            var author = authorManager.GetAuthorById(authorId);

            if (author == null)
                return new ValidationResult("No Author with Id :" + authorId);
            else
                return ValidationResult.Success;
        }
    }
}
