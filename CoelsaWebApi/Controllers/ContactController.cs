using System.Collections.Generic;
using System.Threading.Tasks;
using CoelsaCommon.Models;
using CoelsaWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoelsaWebApi.Controllers
{
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [Route("api/contacts")]
        public async Task<IEnumerable<Contact>> GetContactsByFilter([FromQuery] ContactFilterModel filter)
        {
            return await _contactService.GetContactsByFilter(filter);
        }

        [HttpGet]
        [Route("api/contact/{id}")]
        public async Task<Contact> GetContact(int id)
        {
            return await _contactService.GetContact(id);
        }

        [HttpPost]
        [Route("api/contact")]
        public async Task<Contact> AddContact([FromBody] Contact contact)
        {
            return await _contactService.CreateContact(contact);
        }

        [HttpPatch]
        [Route("api/contact")]
        public async Task<Contact> UpdateContact([FromBody] Contact contact)
        {
            return await _contactService.UpdateContact(contact);
        }

        [HttpDelete]
        [Route("api/contact/{id}")]
        public async Task<bool> DeleteContact(int id)
        {
            return await _contactService.DeleteContact(id);
        }
    }
}
