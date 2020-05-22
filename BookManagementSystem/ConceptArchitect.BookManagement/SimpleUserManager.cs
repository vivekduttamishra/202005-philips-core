using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class SimpleUserManager : IUserAdmin
    {
        IRepository<User, string> users;
        public SimpleUserManager(IRepository<User,string>users)
        {
            this.users = users;
        }
        public User Authenticate(string id, string password)
        {
            var user = users.GetById(id);
            if (user == null)
                return null; //validation failed. wrong id
            if (user.Password == password)
                return user;
            else
                return null; //validation failed. wrong password
        }

        public bool ChangePassword(string id, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAllUsers()
        {
            return users.GetAll();
        }

        public User GetUserById(string id)
        {
            return users.GetById(id);
        }

        public bool Register(User user)
        {
            if (user == null)
                return false;

            var existing = users.GetById(user.Email);
            if (existing != null)
                return false;
            users.Add(user);
            users.Save();
            return true;
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
