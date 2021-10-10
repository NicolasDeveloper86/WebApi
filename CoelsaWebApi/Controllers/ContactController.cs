using System.Collections.Generic;
using System.Threading.Tasks;
using CoelsaCommon.Models;
using CoelsaWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoelsaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _contactLogger;

        public ContactController(IContactService contactService,
                                 ILoggerFactory loggerFactory)
        {
            _contactService = contactService;
            _contactLogger = loggerFactory.CreateLogger<ContactController>();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContactsByFilter([FromQuery] ContactFilterModel filter)
        {
            _contactLogger.LogInformation($"Executing GetContactsByFilter with parameters: {JsonConvert.SerializeObject(filter)}");
            
            var result = await _contactService.GetContactsByFilter(filter);

            _contactLogger.LogInformation($"Executed method GetContactsByFilter successfully, returning status: " +
                $"{StatusCodes.Status200OK}");

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            _contactLogger.LogInformation($"Executing GetContact with Id: {id}");

            var result = await _contactService.GetContact(id);

            _contactLogger.LogInformation($"Executed method GetContact successfully, returning status: " +
                $"{StatusCodes.Status200OK}, with data {JsonConvert.SerializeObject(result)}");

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contact>> AddContact([FromBody] Contact contact)
        {
            _contactLogger.LogInformation($"Executing AddContact with Contact: {JsonConvert.SerializeObject(contact)}");

            var result = await _contactService.CreateContact(contact);

            _contactLogger.LogInformation($"Executed method AddContact successfully, returning status: " +
                $"{StatusCodes.Status201Created}, with data {JsonConvert.SerializeObject(result)}");

            return CreatedAtAction("AddContact", result);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contact>> UpdateContact([FromBody] Contact contact)
        {
            _contactLogger.LogInformation($"Executing UpdateContact with Contact to update: {JsonConvert.SerializeObject(contact)}");

            var result = await _contactService.UpdateContact(contact);

            _contactLogger.LogInformation($"Executed method UpdateContact successfully, returning status: " +
               $"{StatusCodes.Status200OK}, with data to return: {JsonConvert.SerializeObject(result)}");

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteContact(int id)
        {
            _contactLogger.LogInformation($"Executing DeleteContact with Contact Id {JsonConvert.SerializeObject(id)}");

            var result = await _contactService.DeleteContact(id);

            if(!result)
            {
                _contactLogger.LogInformation($"Executed method DeleteContact successfully, returning status: " +
                $"{StatusCodes.Status404NotFound}");

                return NotFound();
            }

            _contactLogger.LogInformation($"Executed method DeleteContact successfully, returning status: " +
                $"{StatusCodes.Status200OK}");

            return Ok();
        }
    }
}
