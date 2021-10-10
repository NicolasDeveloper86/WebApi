using CoelsaCommon.Models;
using CoelsaData;
using CoelsaData.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CoelsaTests
{
    public class ContactRepositoryTest
    {
        private CoelsaContext _coelsaContext;
        private ContactRepository _contactRepository;
        private Mock<NullLoggerFactory> _loggerFactoryMock;

        private void Init()
        {
            var contextOptions = new DbContextOptionsBuilder<CoelsaContext>()
                .UseInMemoryDatabase("contacts-db").Options;

            _coelsaContext = new CoelsaContext(contextOptions);
            _coelsaContext.Database.EnsureDeleted();
            _coelsaContext.Database.EnsureCreated();

            _loggerFactoryMock = new Mock<NullLoggerFactory>();
            _contactRepository = new ContactRepository(_coelsaContext, _loggerFactoryMock.Object);
        }

        [Fact]
        public async Task GetContactsBy_Should_Get_All_Contacts()
        {
            //Arrange
            Init();

            var contacts = new List<Contact>
            {
                new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" },
                new Contact() { Company = "B", Email = "ClientOne@B.com", FirstName = "Client", LastName = "Two", PhoneNumber = "878978" },
                new Contact() { Company = "C", Email = "ClientOne@C.com", FirstName = "Client", LastName = "Three", PhoneNumber = "12324345" }
            };

            await _coelsaContext.AddRangeAsync(contacts);
            await _coelsaContext.SaveChangesAsync();

            //Act
            List<Contact> contactsFromDB = (List<Contact>)await _contactRepository.GetContactsBy();

            //Assert
            Assert.Equal(3, contactsFromDB.Count);
        }

        [Fact]
        public async Task GetContactsBy_Should_Get_Contacts_In_Company_That_Have_At_Least_An_A()
        {
            //Arrange
            Init();

            var contacts = new List<Contact>
            {
                new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" },
                new Contact() { Company = "B", Email = "ClientOne@B.com", FirstName = "Client", LastName = "Two", PhoneNumber = "878978" },
                new Contact() { Company = "Basync", Email = "ClientOne@A.com", FirstName = "Client", LastName = "Three", PhoneNumber = "12324345" }
            };

            await _coelsaContext.AddRangeAsync(contacts);
            await _coelsaContext.SaveChangesAsync();

            ContactFilterModel filterModel = new ContactFilterModel
            {
                Term = "a"
            };

            Expression<Func<Contact, bool>> expressionToSearch = c => c.Company.ToLower().Contains(filterModel.Term.ToLower());
            Func<IQueryable<Contact>, IOrderedQueryable<Contact>> expressionOrderBy = c => c.OrderBy(c => c.Id);

            //Act
            List<Contact> contactsFromDB = (List<Contact>)await _contactRepository.GetContactsBy(expressionToSearch, orderBy: expressionOrderBy);

            //Assert
            Assert.Equal(2, contactsFromDB.Count);

            Assert.Equal(1, contactsFromDB[0].Id);
            Assert.Equal("A", contactsFromDB[0].Company);
            Assert.Equal("ClientOne@A.com", contactsFromDB[0].Email);
            Assert.Equal("Client", contactsFromDB[0].FirstName);
            Assert.Equal("One", contactsFromDB[0].LastName);
            Assert.Equal("243434", contactsFromDB[0].PhoneNumber);

            Assert.Equal(3, contactsFromDB[1].Id);
            Assert.Equal("Basync", contactsFromDB[1].Company);
            Assert.Equal("ClientOne@A.com", contactsFromDB[1].Email);
            Assert.Equal("Client", contactsFromDB[1].FirstName);
            Assert.Equal("Three", contactsFromDB[1].LastName);
            Assert.Equal("12324345", contactsFromDB[1].PhoneNumber);
        }

        [Fact] 
        public async Task GetContactsBy_Should_Paginate_And_Return_Second_Page_With_Three_Records()
        {
            //Arrange
            Init();

            var contacts = new List<Contact>
            {
                //Page 1
                new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" },
                new Contact() { Company = "B", Email = "ClientOne@B.com", FirstName = "Client", LastName = "Two", PhoneNumber = "878978" },
                new Contact() { Company = "C", Email = "ClientOne@A.com", FirstName = "Client", LastName = "Three", PhoneNumber = "12324345" },

                //Page 2
                new Contact() { Company = "D", Email = "ClientOne@D.com", FirstName = "Client", LastName = "Four", PhoneNumber = "4" },
                new Contact() { Company = "E", Email = "ClientOne@E.com", FirstName = "Client", LastName = "Five", PhoneNumber = "5" },
                new Contact() { Company = "F", Email = "ClientOne@F.com", FirstName = "Client", LastName = "Six", PhoneNumber = "6" }
            };

            await _coelsaContext.AddRangeAsync(contacts);
            await _coelsaContext.SaveChangesAsync();

            ContactFilterModel filterModel = new ContactFilterModel
            {
                Limit = 3,
                Page = 2
            };

            Func<IQueryable<Contact>, IOrderedQueryable<Contact>> expressionOrderBy = c => c.OrderBy(c => c.Id);

            //Act
            List<Contact> contactsFromDB = (List<Contact>)await _contactRepository.GetContactsBy(take: filterModel.Limit, skip: (filterModel.Page - 1) * filterModel.Limit, orderBy: expressionOrderBy);

            //Assert
            Assert.Equal(3, contactsFromDB.Count);

            Assert.Equal(4, contactsFromDB[0].Id);
            Assert.Equal("D", contactsFromDB[0].Company);
            Assert.Equal("ClientOne@D.com", contactsFromDB[0].Email);
            Assert.Equal("Client", contactsFromDB[0].FirstName);
            Assert.Equal("Four", contactsFromDB[0].LastName);
            Assert.Equal("4", contactsFromDB[0].PhoneNumber);

            Assert.Equal(5, contactsFromDB[1].Id);
            Assert.Equal("E", contactsFromDB[1].Company);
            Assert.Equal("ClientOne@E.com", contactsFromDB[1].Email);
            Assert.Equal("Client", contactsFromDB[1].FirstName);
            Assert.Equal("Five", contactsFromDB[1].LastName);
            Assert.Equal("5", contactsFromDB[1].PhoneNumber);

            Assert.Equal(6, contactsFromDB[2].Id);
            Assert.Equal("F", contactsFromDB[2].Company);
            Assert.Equal("ClientOne@F.com", contactsFromDB[2].Email);
            Assert.Equal("Client", contactsFromDB[2].FirstName);
            Assert.Equal("Six", contactsFromDB[2].LastName);
            Assert.Equal("6", contactsFromDB[2].PhoneNumber);

        }

        [Fact]
        public async void InsertContact_Should_Insert_Contact_And_Return_It()
        {
            //Arrange
            Init();

            var contact = new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" };

            int previousId = contact.Id;

            //Act
            var contactReturned = await _contactRepository.InsertContact(contact);

            Assert.NotEqual(previousId, contactReturned.Id);
            Assert.Equal(1, contact.Id);
            Assert.Equal("A", contactReturned.Company);
            Assert.Equal("ClientOne@A.com", contactReturned.Email);
            Assert.Equal("Client", contactReturned.FirstName);
            Assert.Equal("One", contactReturned.LastName);
            Assert.Equal("243434", contactReturned.PhoneNumber);
        }

        [Fact]
        public async void DeleteContact_Should_Delete_Contact_And_Return_True_If_Successful()
        {
            //Arrange
            Init();

            var contact = new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" };

            await _coelsaContext.AddAsync(contact);
            await _coelsaContext.SaveChangesAsync();

            //Act
            var isDeleted = await _contactRepository.DeleteContact(contact.Id);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async void UpdateContact_Should_Update_Contact_And_Return_The_Contact_If_Sucessful()
        {
            //Arrange
            Init();

            var contact = new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" };

            await _coelsaContext.AddAsync(contact);
            await _coelsaContext.SaveChangesAsync();

            var contactToUpdate = _coelsaContext.Contacts.Find(contact.Id);

            //Because it's in memory, the context will change when we change the properties here
            contactToUpdate.Email = "NewClientOneEmail@A.com";
            contactToUpdate.FirstName = "ANewName";

            //Act
             var contactUpdated = await _contactRepository.UpdateContact(contact);

            //Assert
            Assert.Equal("NewClientOneEmail@A.com", contactUpdated.Email);
            Assert.Equal("ANewName", contactUpdated.FirstName);
                
        }
    }
}
