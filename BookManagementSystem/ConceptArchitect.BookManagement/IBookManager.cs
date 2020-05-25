using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public interface IBookManager 
    {
        string AddBook(Book book);

        Book GetBookById(string id);
        
        IList<Book> GetAllBooks();

        IList<Book> GetBooksByAuthor(string author);
        
        IList<Book> GetBooksInPriceRange(int min, int max);

        IList<Book> GetBooksWithTag(string tag);

        IList<Book> Search(string term);

        Book GetBookByTitle(string title);

        bool UpdateBook(Book book);

        bool DeleteBook(string bookId);

        void Save();
    }
}
