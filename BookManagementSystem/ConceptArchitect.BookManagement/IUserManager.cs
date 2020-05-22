using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public interface IUserManager
    {
        bool Register(User user);
        User Authenticate(string id, string password);

        bool ChangePassword(string id, string currentPassword, string newPassword);

        void UpdateUser( User user);

        User GetUserById(string id);

    }

    public interface IUserAdmin: IUserManager
    { 
        IList<User> GetAllUsers(); //only for Admin
        //only for Admin
    }
}
