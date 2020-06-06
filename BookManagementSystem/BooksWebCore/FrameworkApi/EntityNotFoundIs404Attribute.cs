using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.FrameworkApi
{
    public class EntityNotFoundIs404Attribute : CustomExceptionMapperAttribute
    {
        public EntityNotFoundIs404Attribute() : base(typeof(EntityNotFoundException),404)
        {
        }

        public override void UpdateContext(ExceptionContext context)
        {
            EntityNotFoundException ex = context.Exception as EntityNotFoundException;
            if (ex != null)
            {
                context.ModelState.AddModelError("Entity", ex.Entity.Name);
                context.ModelState.AddModelError("Id", ex.Id);
            }
        }
    }
}
