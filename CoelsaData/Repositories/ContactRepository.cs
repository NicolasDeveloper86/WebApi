using CoelsaCommon.Models;
using CoelsaCommon.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoelsaData.Repositories
{
    public class ContactRepository : IContactRepository, IDisposable
    {
        private readonly CoelsaContext _coelsaContext;
        private bool disposed = false;

        public ContactRepository(CoelsaContext coelsaContext)
        {
            _coelsaContext = coelsaContext;
        }

        /// <summary>
        /// Deletes a Contact from the context
        /// </summary>
        /// <param name="id">The id of the contact</param>
        /// <returns>A boolean indicating if it was deleteds or not, true if it was deleted</returns>
        public async Task<bool> DeleteContact(int id)
        {
            Contact entityToDelete = await _coelsaContext.Contacts.FindAsync(id);

            _coelsaContext.Remove(entityToDelete);

            await Save();

            Contact isDeleted = await GetContactById(id);

            return isDeleted == null;
        }

        /// <summary>
        /// Gets a Contact based on it's Id
        /// </summary>
        /// <param name="id">The Id of the contact</param>
        /// <returns>The Contact</returns>
        public async Task<Contact> GetContactById(int id)
        {
            var contact = await _coelsaContext.Contacts.FindAsync(id);

            return contact;
        }

        /// <summary>
        /// Inserts a new Contact
        /// </summary>
        /// <param name="contact">The contact Object to insert</param>
        /// <returns>The inserted Contact</returns>
        public async Task<Contact> InsertContact(Contact contact)
        {
            await _coelsaContext.Contacts.AddAsync(contact);

            await Save();

            return contact;
        }

        /// <summary>
        /// Makes the save changes to the Context
        /// </summary>
        /// <returns>an Asynchronous call to SaveChanges</returns>
        public async Task Save()
        {
            await _coelsaContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a Contact
        /// </summary>
        /// <param name="contact">The contact to Update</param>
        /// <returns>The Updated Contact</returns>
        public async Task<Contact> UpdateContact(Contact contact)
        {
            _coelsaContext.Update(contact);

            await Save();

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

            return contacts;
        }
    }
}
