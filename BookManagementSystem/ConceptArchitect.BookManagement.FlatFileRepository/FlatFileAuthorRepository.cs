using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConceptArchitect.BookManagement.FlatFileRepository
{
    public class FlatFileAuthorRepository : IRepository<Author, string>
    {
        BookStore store;
        public FlatFileAuthorRepository(BookStore store) //dependency injection
        {
            this.store = store;
        }

        public string Add(Author author )
        {
            if (string.IsNullOrEmpty(author.Id))
                return null;
            store.authors[author.Id.ToLower()] = author;
            return author.Id.ToLower();
        }

        public void Delete(string id)
        {
            id = id.ToLower();
            if (store.authors.ContainsKey(id))
                store.authors.Remove(id);
        }

        public IList<Author> GetAll()
        {
            return store.authors.Values.ToList();
        }

        public IList<Author> GetAll(Func<Author, bool> matcher)
        {
            return (from author in store.authors.Values where matcher(author) select author).ToList();
        }

        public Author GetById(string id)
        {
            id = id.ToLower();
            if (store.authors.ContainsKey(id))
                return store.authors[id];
            else
                return null;
        }

        public Author GetOne(Func<Author, bool> matcher)
        {
            return store.authors.Values.FirstOrDefault(matcher);
        }

        public void Save()
        {
            store.Save();
        }

        public void Update(string id, Author updatedEntity, Action<Author, Author> mergeDetails)
        {
            var old = GetById(id);
            if (old == null)
                return;

            mergeDetails(old, updatedEntity);
        }
    }
}
