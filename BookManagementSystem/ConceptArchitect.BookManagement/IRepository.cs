using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public interface IRepository<Entity,Id>
    {
        Id Add(Entity entity);
        Entity GetById(Id id);
        IList<Entity> GetAll();
        IList<Entity> GetAll(Func<Entity, bool> matcher);

        Entity GetOne(Func<Entity, bool> matcher);

        void Update(Id id, Entity updatedEntity, Action<Entity, Entity> mergeDetails);

        void Delete(Id id);

        void Save(); //save the details
    }
}
