using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class SimpleAuthorManager : IAuthorManager
    {

        //Author Manager knows there is a Repository.
        //It shouldn't know which Repository as Repository may change in future
        //This is known as Dependency Inversion --> not connected to actual implmentation that may change tomorrow
        IRepository<Author, string> authorRepository;

        //We must supply the repository needed by the model
        //This is known as Dependency Injection
        public SimpleAuthorManager(IRepository<Author,string> authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public IList<Author> GetAuthorByName(string name)
        {
            name = name.ToLower();
            return authorRepository.GetAll(a => a.Name.ToLower() == name);
        }

        public string AddAuthor(Author author)
        {
            if (author == null)
                return null;
            if (!ValidateId(author))
                return null;

            //Now we need to save this in our storage
            //That is the job of a repository
            var id=authorRepository.Add(author);
            authorRepository.Save();


            //after saving let us return author id
            return id;

        }

        private bool ValidateId(Author author)
        {
            var id = author.Id;
            Author existingAuthor = null;
            if(!string.IsNullOrEmpty(id))
            {
                existingAuthor = GetAuthorById(id); //is there  an exisitng author with same id
                return existingAuthor == null;
            }

            //id is not given let us generate it
            var genId = author.Name.ToLower().Replace(' ', '-');
             existingAuthor = GetAuthorById(genId);
            if(existingAuthor==null)
            {
                author.Id = genId;
                return true;
            }

            int count = 1;
            while (true)
            {
                var i = genId + count;
                if(GetAuthorById(i)==null)
                {
                    author.Id = i;
                    return true;
                }
                count++;
            }
        }

        public void DeleteAuthor(string authorId)
        {
            authorRepository.Delete(authorId);
            authorRepository.Save();
        }

        public IList<Author> GetAllAuthors()
        {
            return authorRepository.GetAll();
        }

        public Author GetAuthorById(string id)
        {

            return authorRepository.GetById( IdTool.Normalize(id)  );
        }

        public IList<Author> GetAuthors(string q)
        {
            q = q.ToLower();
            return authorRepository.GetAll(
                a => a.Name.ToLower().Contains(q) || 
                a.Biography.ToLower().Contains(q) || 
                a.Email.ToLower() == q);
        }

        public IList<Author> GetLivingAuthors()
        {
            return authorRepository.GetAll(a => a.DeathDate == null);
        }

        public void UpdateAuthor(Author newInfo)
        {
            authorRepository.Update(newInfo.Id, newInfo, (o, n) =>
            {
                //name and id will not be changed.
                o.Biography = n.Biography;
                o.BirthDate = n.BirthDate;
                o.DeathDate = n.DeathDate;
                o.Email = n.Email;
            });
            authorRepository.Save();
        }
    }
}
