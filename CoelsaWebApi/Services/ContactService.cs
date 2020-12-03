using CoelsaCommon.Models;
using CoelsaCommon.Validation;
using CoelsaData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoelsaWebApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IValidator<Contact> _contactValidation;

        public ContactService(IContactRepository contactRepository,
            IValidator<Contact> _contactValidator)
        {
            _contactRepository = contactRepository;
            _contactValidation = _contactValidator;
        }

        /// <summary>
        /// Creates a contact validating the contact and calling the repository
        /// </summary>
        /// <param name="contact">The contact to be inserted</param>
        /// <returns>The contact that was inserted</returns>
        public async Task<Contact> CreateContact(Contact contact)
        {
            _contactValidation.Validate(contact);

            return await _contactRepository.InsertContact(contact);
        }

        /// <summary>
        /// Deletes a contact calling the repository
        /// </summary>
        /// <param name="id">The id of the contact to delete</param>
        /// <returns>returns true if it was correctly deleted</returns>
        public async Task<bool> DeleteContact(int id)
        {
            return await _contactRepository.DeleteContact(id);
        }

        /// <summary>
        /// Gets a single contact by it's Id, calling the repository
        /// </summary>
        /// <param name="id">The id of the contact to get</param>
        /// <returns>The contact</returns>
        public async Task<Contact> GetContact(int id)
        {
            return await _contactRepository.GetContactById(id);
        }

        /// <summary>
        /// Gets a IEnumerable of Contacts by filter, calling the repository
        /// </summary>
        /// <param name="filter">The filter to be applied in the context</param>
        /// <returns>A IEnumerable of Contacts</returns>
        public async Task<IEnumerable<Contact>> GetContactsByFilter(ContactFilterModel filter)
        {
            Expression<Func<Contact, bool>> expressionFilter = c => c.Company.ToLower().Contains(filter.Term.ToLower());
            
            int skip = (filter.Page - 1) * filter.Limit;

            return await _contactRepository.GetContactsBy(filter: expressionFilter, take: filter.Limit, skip: skip);
        }

        /// <summary>
        /// Updates a contact, validating it first and saving it, calling the repository
        /// </summary>
        /// <param name="contact">The contact to update</param>
        /// <returns>The contact updated</returns>
        public async Task<Contact> UpdateContact(Contact contact)
        {
            _contactValidation.Validate(contact);

            return await _contactRepository.UpdateContact(contact);
        }
    }
}
