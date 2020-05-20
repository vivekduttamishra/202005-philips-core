using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    
    public interface IAuthorManager 
    {
        string AddAuthor(Author author); // add an author and return an author id

        Author GetAuthorById(string id);

        IList<Author> GetAllAuthors();

        IList<Author> GetAuthors(string q);

        IList<Author> GetLivingAuthors();

        void UpdateAuthor(Author newInfo);

        void DeleteAuthor(string authorId);

    }
}
