using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class AgeRangeAttribute: ValidationAttribute
    {
        public int MinAge { get; set; } = 0;
        public int MaxAge { get; set; } = 100;

        public string MinAgeErrorMessage { get; set; } = "Age should be atleast {0} years";
        public string MaxAgeErrorMessage { get; set; } = "Age shouldn't exceed {0} years";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //value is the current input. But we need to see the whole Model
            //Not one property of

            var author = validationContext.ObjectInstance as Author;
            if (author == null) //current object is not an author
                return ValidationResult.Success; //for me it is not a failure

            if(author.Age < MinAge)
            {
                var error = string.Format(MinAgeErrorMessage, MinAge);
                return new ValidationResult(error); //return this error
            }

            if(author.Age>=MaxAge)
            {
                var error = string.Format(MaxAgeErrorMessage, MaxAge);
                return new ValidationResult(error);
            }
                
            // all is well

            return ValidationResult.Success; //No Error
        }
    }
}
