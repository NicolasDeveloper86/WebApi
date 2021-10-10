using CoelsaCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoelsaData.Repositories
{
    public interface IContactRepository : IDisposable
    {
        Task<IEnumerable<Contact>> GetContactsBy(Expression<Func<Contact, bool>> filter = null,
            Func<IQueryable<Contact>, IOrderedQueryable<Contact>> orderBy = null,
            int? skip = null,
            int? take = null);
        Task<Contact> InsertContact(Contact contact);
        Task<bool> DeleteContact(int id);
        Task<Contact> UpdateContact(Contact contact);
        Task<Contact> GetContactById(int id);
        Task Save();
    }
}
