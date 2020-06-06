using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConceptArchitect.BookManagement.FlatFileRepository
{
    public class FlatFileBookRepository : IRepository<Book, string>
    {
        BookStore store;
        IRepository<Author, string> authorRepository;
        public FlatFileBookRepository(BookStore store,IRepository<Author,String> authorRepository) //dependency injection
        {
            this.store = store;
            this.authorRepository= authorRepository;
        }

        public string Add(Book book )
        {
            if (string.IsNullOrEmpty(book.Id))
                return null;

            var author = authorRepository.GetOne(a => a.Name.ToLower() == book.Author.Name.ToLower());
            if (author == null)
                return null; //add failed

            store.books[book.Id.ToLower()] = book;

            author.Books.Add(book); //add current book to the author collection of books
            authorRepository.Save();


            return book.Id.ToLower();
        }

        public void Delete(string id)
        {
            var book = GetById(id);
            if (book == null)
                return;

            store.books.Remove(book.Id.ToLower());
            var author = book.Author;
            book.Author.Books.Remove(book); //remove this book from author collection
            authorRepository.Save();

        }

        public IList<Book> GetAll()
        {
            return store.books.Values.ToList();
        }

        public IList<Book> GetAll(Func<Book, bool> matcher)
        {
            return (from book in store.books.Values where matcher(book) select book).ToList();
        }

        public Book GetById(string id)
        {
            id = id.ToLower();
            if (store.books.ContainsKey(id))
                return store.books[id];
            else
                throw new EntityNotFoundException(Entity: typeof(Book), Id: id);//return null;
        }

        public Book GetOne(Func<Book, bool> matcher)
        {
            return store.books.Values.FirstOrDefault(matcher);
        }

        public void Save()
        {
            store.Save();
        }

        public void Update(string id, Book updatedEntity, Action<Book, Book> mergeDetails)
        {
            var old = GetById(id);
            if (old == null)
                return;

            mergeDetails(old, updatedEntity);
        }
    }
}
