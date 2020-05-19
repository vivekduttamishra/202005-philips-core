using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConceptArchitect.BookManagement.FlatFileRepository
{
    [Serializable]
    public class BookStore
    {
        internal Dictionary<string, Book> books = new Dictionary<string, Book>();
        internal Dictionary<string, Author> authors = new Dictionary<string, Author>();
        
        string path;

        public void Save()
        {
            using (var file = new StreamWriter(path))
            {
                var fmt = new BinaryFormatter();
                fmt.Serialize(file.BaseStream, this);
            }
        }

        public static BookStore Load(string path)
        {
            BookStore store = null;
            try
            {
                using(var file=new StreamReader(path))
                {
                    var fmt = new BinaryFormatter();
                    store = (BookStore)fmt.Deserialize(file.BaseStream);

                }
                
            }
            catch
            {
                store = new BookStore();
            }
            store.path = path;

            return store;
        }



    }
}
