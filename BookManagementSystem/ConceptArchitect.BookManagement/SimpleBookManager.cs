using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConceptArchitect.BookManagement
{
    public class SimpleBookManager : IBookManager
    {
        public IRepository<Book,string> BookRepository { get; set; }

        public SimpleBookManager(IRepository<Book,string> repository)
        {
            BookRepository = repository;
        }


        #region IBookManager Members

        public string AddBook(Book book)
        {
            if (book == null) return null;
            if (book.Author == null) return null;
            if (!ValidateId(book)) return null;

            return BookRepository.Add(book);
        }

        private bool ValidateId(Book book)
        {
            if(book.Id!=null)
            {
                if (GetBookById(book.Id) != null)
                    return false;
                else
                    return true;
            }
            else
            {
                string i = IdTool.Normalize(book.Title);
                if(GetBookById(i)==null)
                {
                    book.Id = i;
                    return true;
                }
                else
                {
                    int c = 1;
                    while (GetBookById(i + "-" + c) != null)
                        c++;
                    book.Id = i + "-" + c;
                    return true;
                }
            }
        }


        public Book GetBookById(string id)
        {
            var book=BookRepository.GetById(id);
            if (book == null)
                return null;
            else
                return book;
        }

        public IList<Book> GetAllBooks()
        {
            return BookRepository.GetAll();
        }

        public IList<Book> GetBooksByAuthor(string author)
        {
            author = IdTool.Normalize(author);
            return BookRepository.GetAll(b => IdTool.Normalize(b.Author.Name).Equals(author));
        }

        public IList<Book> GetBooksInPriceRange(int min, int max)
        {
            return BookRepository.GetAll(b=>b.Price>=min && b.Price<max);
        }

        public IList<Book> GetBooksWithTag(string tag)
        {
            tag = tag.ToLower();
            return BookRepository.GetAll(b => b.Tags.ToLower().Contains(tag));
        }

        public IList<Book> Search(string term)
        {
            term = term.ToLower();
            return BookRepository.GetAll(b => b.Title.ToLower().Contains(term) ||
                                             b.Description.ToLower().Contains(term) ||
                                             b.Tags.ToLower().Contains(term) ||
                                             b.Author.Name.ToLower().Contains(term)
                                             );
        }

        public Book GetBookByTitle(string title)
        {
            title = IdTool.Normalize(title);
            return BookRepository.GetOne(b => IdTool.Normalize(b.Title).Equals(title));
        }

        public bool UpdateBook(Book book)
        {
            BookRepository.Update(book.Id, book, (o, n) =>
            {
                o.Price = n.Price;
                o.Description = n.Description;
                o.Title = n.Title;
                o.Tags = n.Tags;
                o.CoverPage = n.CoverPage;
                //can't modify o.Author and o.Id
            });
            return true;
        }

        public bool DeleteBook(string bookId)
        {
            BookRepository.Delete(bookId);
            return true;
        }

        public void Save()
        {
            BookRepository.Save();
        }

        #endregion
    }
}
