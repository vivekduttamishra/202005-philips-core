using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MvcApp.Hubs;
using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApp.Controllers
{
    [ApiController]
    [Route("api/people")]
    public class PeopleController:Controller
    {
        IPeopleManager manager;
        IHubContext<PersonHub> personHub;
        public PeopleController(IPeopleManager manager,IHubContext<PersonHub> personHub)
        {
            this.personHub = personHub;
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(manager.GetAllPeople());
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetAllAsync()
        {
            var people = await manager.GetAllAsync();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Person p = manager.GetPerson(id);
            if (p == null)
                return NotFound(new { Id = id, Error = "Not Found" });
            else
                return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Person person)
        {
            if (ModelState.IsValid)
            {
                manager.AddPerson(person);
                //return Created($"/api/people/{person.Id}", person);
                await personHub.Clients.All.SendAsync("PersonAdded", person);
                return CreatedAtAction("GetById", new { Id = person.Id }, person);
            }
            else
            {
                return  BadRequest(ModelState);
            }
        }



    }
}
