using CoelsaCommon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoelsaWebApi.Services
{
    public interface IContactService
    {
        Task<Contact> CreateContact(Contact contact);
        Task<Contact> UpdateContact(Contact contact);
        Task<bool> DeleteContact(int id);
        Task<IEnumerable<Contact>> GetContactsByFilter(ContactFilterModel filter);
        Task<Contact> GetContact(int id);
    }
}
