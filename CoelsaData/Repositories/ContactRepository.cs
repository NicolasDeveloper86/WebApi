using CoelsaCommon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoelsaData.Repositories
{
    public class ContactRepository : IContactRepository, IDisposable
    {
        private readonly CoelsaContext _coelsaContext;
        private bool disposed = false;
        private ILogger<ContactRepository> _logger;

        public ContactRepository(CoelsaContext coelsaContext,
            ILoggerFactory loggerFactory)
        {
            _coelsaContext = coelsaContext;
            _logger = loggerFactory.CreateLogger<ContactRepository>();
        }

        /// <summary>
        /// Deletes a Contact from the context
        /// </summary>
        /// <param name="id">The id of the contact</param>
        /// <returns>A boolean indicating if it was deleteds or not, true if it was deleted</returns>
        public async Task<bool> DeleteContact(int id)
        {
            _logger.LogInformation($"Executing CreateContact on {nameof(ContactRepository)}");

            Contact entityToDelete = await _coelsaContext.Contacts.FindAsync(id);

            if(entityToDelete == null)
            {
                _logger.LogWarning($"CreateContact Executed. No entities to be deleted using id: {id}");

                return false;
            }

            _coelsaContext.Remove(entityToDelete);

            await Save();

            _logger.LogWarning($"CreateContact Executed");

            return true;
        }

        /// <summary>
        /// Gets a Contact based on it's Id
        /// </summary>
        /// <param name="id">The Id of the contact</param>
        /// <returns>The Contact</returns>
        public async Task<Contact> GetContactById(int id)
        {
            _logger.LogInformation($"Executing GetContactById on {nameof(ContactRepository)} with id: {id}");

            var contact = await _coelsaContext.Contacts.FindAsync(id);

            _logger.LogInformation($"GetContactById Executed");

            return contact;
        }

        /// <summary>
        /// Inserts a new Contact
        /// </summary>
        /// <param name="contact">The contact Object to insert</param>
        /// <returns>The inserted Contact</returns>
        public async Task<Contact> InsertContact(Contact contact)
        {
            _logger.LogInformation($"Executing InsertContact on {nameof(ContactRepository)} with contact: {JsonConvert.SerializeObject(contact)}");

            await _coelsaContext.Contacts.AddAsync(contact);

            await Save();

            _logger.LogInformation($"InsertContact Executed");

            return contact;
        }

        /// <summary>
        /// Makes the save changes to the Context
        /// </summary>
        /// <returns>an Asynchronous call to SaveChanges</returns>
        public async Task Save()
        {
            _logger.LogInformation("Saving Changes");

            await _coelsaContext.SaveChangesAsync();

            _logger.LogInformation("Saved");
        }

        /// <summary>
        /// Updates a Contact
        /// </summary>
        /// <param name="contact">The contact to Update</param>
        /// <returns>The Updated Contact</returns>
        public async Task<Contact> UpdateContact(Contact contact)
        {
            _logger.LogInformation($"Executing InsertContact on {nameof(ContactRepository)} with contact: {JsonConvert.SerializeObject(contact)}");

            _coelsaContext.Update(contact);

            await Save();

            _logger.LogInformation("InsertContact Executed");

            return contact;
        }
    
        /// <summary>
        /// Disposes the context
        /// </summary>
        /// <param name="disposing">A boolean indicating if the context must be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _coelsaContext.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Calls the Dispose method and calls the Garbage Collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets contacts by filters
        /// </summary>
        /// <param name="filter">An expression defining a filter to a particular property</param>
        /// <param name="orderBy">Orders the list of contact</param>
        /// <param name="skip">Pagination, this is the page</param>
        /// <param name="take">Pagination, this is how many elements the page must have</param>
        /// <returns>An IEnumerable of Contact</returns>
        public async Task<IEnumerable<Contact>> GetContactsBy(Expression<Func<Contact, bool>> filter = null,
            Func<IQueryable<Contact>, IOrderedQueryable<Contact>> orderBy = null,
            int? skip = null, 
            int? take = null)
        {
            _logger.LogInformation($"Executing GetContactsBy on {nameof(ContactRepository)}");

            IQueryable<Contact> query = _coelsaContext.Contacts;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if(take != null)
            {
                query = query.Take(take.Value);
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }

            var contacts = await query.ToListAsync();

            _logger.LogInformation("GetContactsBy Executed");

            return contacts;
        }
    }
}
