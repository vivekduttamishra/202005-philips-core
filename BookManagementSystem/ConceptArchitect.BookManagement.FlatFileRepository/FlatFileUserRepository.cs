using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ConceptArchitect.BookManagement.FlatFileRepository
{

    [Serializable]
    public class UserStore
    {
        internal Dictionary<string, User> users = new Dictionary<string, User>();

        string path;

        public void Save()
        {
            using (var file = new StreamWriter(path))
            {
                var fmt = new BinaryFormatter();
                fmt.Serialize(file.BaseStream, this);
            }
        }

        public static UserStore Load(string path)
        {
            UserStore store = null;
            try
            {
                using (var file = new StreamReader(path))
                {
                    var fmt = new BinaryFormatter();
                    store = (UserStore)fmt.Deserialize(file.BaseStream);

                }

            }
            catch
            {
                store = new UserStore();
            }
            store.path = path;

            return store;
        }



    }
    public class FlatFileUserRepository : IRepository<User, string>
    {
        UserStore store;
        public FlatFileUserRepository(UserStore store)
        {
            this.store = store;
        }
        public string Add(User entity)
        {
            store.users[entity.Email.ToLower()] = entity;
            return entity.Email.ToLower();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            return store.users.Values.ToList();
        }

        public IList<User> GetAll(Func<User, bool> matcher)
        {
            throw new NotImplementedException();
        }

        public User GetById(string id)
        {
            id = id.ToLower();
            if (store.users.ContainsKey(id))
                return store.users[id];
            else
                return null;
        }

        public User GetOne(Func<User, bool> matcher)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            store.Save();
        }

        public void Update(string id, User updatedEntity, Action<User, User> mergeDetails)
        {
            throw new NotImplementedException();
        }
    }
}
