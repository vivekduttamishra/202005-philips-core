using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApp.Models
{
    public interface IPeopleManager
    {
        int AddPerson(Person person);
        IList<Person> GetAllPeople();
        Person GetPerson(int id);

        Task<IList<Person>> GetAllAsync();
    }
}
