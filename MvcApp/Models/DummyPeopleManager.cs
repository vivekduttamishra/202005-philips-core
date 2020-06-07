using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApp.Models
{
    public class DummyPeopleManager: IPeopleManager
    {
        int id;
        Dictionary<int, Person> people = new Dictionary<int, Person>();

        public DummyPeopleManager()
        {
            AddPerson(new Person() { Name = "Vivek", Email = "vivek@conceptarchitect.in" });
            AddPerson(new Person() { Name = "Shweta", Email = "shweta@conceptarchitect.in" });
            AddPerson(new Person() { Name = "Shivanshi", Email = "shivanshi@vnc.in" });
            AddPerson(new Person() { Name = "Sanjay", Email = "sanjay@gmail.com" });
        }

        public int AddPerson(Person person)
        {
            person.Id = ++id;
            people[person.Id] = person;
            return person.Id;
        }

        public IList<Person> GetAllPeople()
        {
            return people.Values.ToList();
        }

        public Person GetPerson(int id)
        {
            if (people.ContainsKey(id))
                return people[id];
            else
                return null;
        }

        public async Task<IList<Person>> GetAllAsync()
        {
            await Task.Delay(5000);
            return people.Values.ToList();
        }
    }
}
