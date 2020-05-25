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
                },
                new Author()
                {
                    Id="john-grisham",
                    Name="John Grisham",
                    Biography="Leading author of legal fiction",

                    BirthDate=new DateTime(1956,1,1)
                    
                },new Author()
                {
                    Id="conan-doyle",
                    Name="Sir Aruthur Dr Conan Doyle",
                    Biography="The author of altime greatest detective 'Sherlock Holmes'",

                    BirthDate=new DateTime(1880,1,1),
                    DeathDate=new DateTime(1923,1,1)
                }
            };

        #endregion

        //uses conventional route ---> [Route("api/authors")]
        public IEnumerable<Author> Get()
        {
            return authors;
        }

        //uses conventional route ---> [Route("api/authors/{id}")]
        public Author Get(string id)
        {
            return authors.FirstOrDefault(a => a.Id == id);
        }

        [Route("api/authors/born-after/{year}")]
        public IList<Author> GetAuthorsBornAfter(int year)
        {
            return authors.Where(a => a.BirthDate.Year >= year).ToList();
        }

        [Route("api/authors/living")]
        public IList<Author> GetLivingAuthors()
        {
            return authors.Where(a => a.DeathDate == null).ToList();
        }
    }
}