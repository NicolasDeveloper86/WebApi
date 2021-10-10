using CoelsaCommon.Models;
using CoelsaCommon.Validation;
using CoelsaData.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoelsaWebApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IValidator<Contact> _contactValidation;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository,
            IValidator<Contact> _contactValidator,
            ILoggerFactory loggerFactory)
        {
            _contactRepository = contactRepository;
            _contactValidation = _contactValidator;
            _logger = loggerFactory.CreateLogger<ContactService>();
        }

        /// <summary>
        /// Creates a contact validating the contact and calling the repository
        /// </summary>
        /// <param name="contact">The contact to be inserted</param>
        /// <returns>The contact that was inserted</returns>
        public async Task<Contact> CreateContact(Contact contact)
        {
            _logger.LogInformation($"Executing CreateContact on {nameof(ContactService)} with contact: {JsonConvert.SerializeObject(contact)}");

            _contactValidation.Validate(contact);

            var result = await _contactRepository.InsertContact(contact);

            _logger.LogInformation($"Finalizing executing CreateContact with result {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// Deletes a contact calling the repository
        /// </summary>
        /// <param name="id">The id of the contact to delete</param>
        /// <returns>returns true if it was correctly deleted</returns>
        public async Task<bool> DeleteContact(int id)
        {
            _logger.LogInformation($"Executing DeleteContact on {nameof(ContactService)} with id: {id}");

            if (id <= 0)
            {
                throw new ValidationException("Id cannot be less or equal than 0", string.Empty);
            }

            var result = await _contactRepository.DeleteContact(id);

            _logger.LogInformation($"Finalizing executing DeleteContact with result {result}");

            return result;
        }

        /// <summary>
        /// Gets a single contact by it's Id, calling the repository
        /// </summary>
        /// <param name="id">The id of the contact to get</param>
        /// <returns>The contact</returns>
        public async Task<Contact> GetContact(int id)
        {
            _logger.LogInformation($"Executing GetContact on {nameof(ContactService)} with id: {id}");

            if (id <= 0)
            {
                throw new ValidationException("Id cannot be less or equal than 0", string.Empty);
            }

            var result = await _contactRepository.GetContactById(id);

            _logger.LogInformation($"Finalizing executing GetContact with result {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// Gets a IEnumerable of Contacts by filter, calling the repository
        /// </summary>
        /// <param name="filter">The filter to be applied in the context</param>
        /// <returns>A IEnumerable of Contacts</returns>
        public async Task<IEnumerable<Contact>> GetContactsByFilter(ContactFilterModel filter)
        {
            _logger.LogInformation($"Executing GetContactsByFilter on {nameof(ContactService)} with filter: {JsonConvert.SerializeObject(filter)}");

            Expression<Func<Contact, bool>> expressionFilter = c => c.Company.ToLower().Contains(filter.Term.ToLower());

            int skip = (filter.Page - 1) * filter.Limit;

            var result = await _contactRepository.GetContactsBy(filter: expressionFilter, take: filter.Limit, skip: skip);

            _logger.LogInformation($"Finalizing executing GetContactsByFilter, number of contacts returned: {result.ToList().Count()}");

            return result;
        }

        /// <summary>
        /// Updates a contact, validating it first and saving it, calling the repository
        /// </summary>
        /// <param name="contact">The contact to update</param>
        /// <returns>The contact updated</returns>
        public async Task<Contact> UpdateContact(Contact contact)
        {
            _logger.LogInformation($"Executing UpdateContact on {nameof(ContactService)} with filter: {JsonConvert.SerializeObject(contact)}");

            _contactValidation.Validate(contact);

            var result = await _contactRepository.UpdateContact(contact);

            _logger.LogInformation($"Finalizing executing GetContact with result {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
