using System;
using System.Collections.Generic;

namespace ConceptArchitect.BookManagement.EFRep
{
    public class EFAuthorRepository : IRepository<Author, string>
    {
        //BookContext context; //-> EF context class
        //public EFAuthorRepository(BookContext context)
        //{
        //    this.context = context;
        //}


        public string Add(Author entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Author> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<Author> GetAll(Func<Author, bool> matcher)
        {
            throw new NotImplementedException();
        }

        public Author GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Author GetOne(Func<Author, bool> matcher)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Author updatedEntity, Action<Author, Author> mergeDetails)
        {
            throw new NotImplementedException();
        }
    }
}
