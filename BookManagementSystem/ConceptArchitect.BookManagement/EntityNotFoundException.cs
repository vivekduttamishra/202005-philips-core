using System;
using System.Runtime.Serialization;

namespace ConceptArchitect.BookManagement
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public Type Entity { get;  }
        public  string Id { get; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(Type Entity, string Id):base($"No {Entity.Name} with Id ${Id}")
        {
            this.Entity = Entity;
            this.Id = Id;
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}