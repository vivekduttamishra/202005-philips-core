using ConceptArchitect.BookManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BookWebApiServerFull.Controllers
{
    public class AuthorsController: ApiController
    {

        #region dummy data - not requried

        Author[] authors =
           {
                new Author()
                {
                    Id="jeffrey-archer",
                    Name="Jeffrey Archer",
                    Biography="Contemporary Best Seller",
                    Email="jeffrey.archer@gmail.com",
                    BirthDate=new DateTime(1946,1,1)
                },
                new Author()
                {
                    Id="alexandre-dumas",
                    Name="Alexandre Dumas",
                    Biography="One of the altime greatest classic author in English and French",

                    BirthDate=new DateTime(1805,1,1),
                    DeathDate=new DateTime(1875,1,1)
                }
            };

        #endregion
        public IEnumerable<Author> Get()
        {
            return authors;
        }

        public Author Get(string id)
        {
            return authors.FirstOrDefault(a => a.Id == id);
        }
    }
}