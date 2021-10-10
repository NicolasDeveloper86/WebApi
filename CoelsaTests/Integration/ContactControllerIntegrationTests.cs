using CoelsaCommon.Models;
using CoelsaWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoelsaTests.Integration
{
    public class ContactControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ContactControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GetContacts_Should_Get_Contacts()
        {
            var client = await _factory.CreateHttpClient();

            var response = await client.GetAsync("/api/contacts");

            var content = await response.Content.ReadAsStringAsync();

            var contentSerialized = JsonConvert.DeserializeObject<List<Contact>>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(3, contentSerialized.Count);

        }

        [Fact]
        public async Task GetContact_Should_Get_Contact_By_Id()
        {
            var client = await _factory.CreateHttpClient();

            var response = await client.GetAsync("/api/contact/3");

            var content = await response.Content.ReadAsStringAsync();

            var contentSerialized = JsonConvert.DeserializeObject<Contact>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull(contentSerialized);
            Assert.Equal("C", contentSerialized.Company);
            Assert.Equal("ClientOne@C.com", contentSerialized.Email);
            Assert.Equal("Client", contentSerialized.FirstName);
            Assert.Equal("Three", contentSerialized.LastName);
            Assert.Equal("12324345", contentSerialized.PhoneNumber);
        }

        [Fact]
        public async Task DeleteContact_Should_Delete_Contact_By_Id()
        {
            var client = await _factory.CreateHttpClient();

            var response = await client.DeleteAsync("/api/contact/3");

            var content = await response.Content.ReadAsStringAsync();

            var contentSerialized = JsonConvert.DeserializeObject<bool>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(contentSerialized);
        }

        [Fact]
        public async Task AddContact_Should_Add_Contact()
        {
            var client = await _factory.CreateHttpClient();

            var contact = new Contact() { Company = "D", Email = "ClientOne@D.com", FirstName = "Client", LastName = "Four", PhoneNumber = "1234567890" };
            
            var contactContent = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/contact", contactContent);

            var content = await response.Content.ReadAsStringAsync();

            var contentSerialized = JsonConvert.DeserializeObject<Contact>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(0, contentSerialized.Id);
        }
    }
}
