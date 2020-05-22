using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class ImageUrlAttribute : ValidationAttribute
    {

        public string ValidExtensions { get; set; } = "jpg,png";

        public ImageUrlAttribute()
        {
            ErrorMessage = "Url Must be a valid Image";
        }

        public override bool IsValid(object value)
        {
            if (value == null) //user has not supplied the value
                return true; // It is not a url so not invalid url


            var urlParts = value.ToString().ToLower().Split('.');
            var _extensions = ValidExtensions.ToLower().Split(',');

            var ext = urlParts[urlParts.Length - 1]; //last part of url

            var result = _extensions.Contains(ext);
            return result;
        }
    }
}
