using CoelsaCommon.Models;
using CoelsaData;
using CoelsaWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoelsaTests.Integration
{
    static class IntegrationTestsExtensions
    {
        internal static async Task<HttpClient> CreateHttpClient(this WebApplicationFactory<Startup> factory,
            Action<CoelsaContext> dbSetup = null,
            Action<IServiceCollection> serviceSetup = null,
            string dbName = null)
        {
            if (dbName == null)
            {
                dbName = Guid.NewGuid().ToString();
            }
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType ==
                        typeof(DbContextOptions<CoelsaContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<CoelsaContext>(options =>
                        options
                        .UseInMemoryDatabase(dbName));

                    var context = services.BuildServiceProvider().GetService<CoelsaContext>();
                    
                    AddTestData(context);

                    if (serviceSetup != null)
                    {
                        serviceSetup(services);
                    }

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<CoelsaContext>();
                        db.Database.EnsureCreated();

                        if (dbSetup != null)
                        {
                            dbSetup(db);
                        }
                    }
                });
            })
            .CreateClient();

            return client;
        }

        private static void AddTestData(CoelsaContext context)
        {
            var contacts = new List<Contact>
            {
                new Contact() { Company = "A", Email = "ClientOne@A.com", FirstName = "Client", LastName = "One", PhoneNumber = "243434" },
                new Contact() { Company = "B", Email = "ClientOne@B.com", FirstName = "Client", LastName = "Two", PhoneNumber = "878978" },
                new Contact() { Company = "C", Email = "ClientOne@C.com", FirstName = "Client", LastName = "Three", PhoneNumber = "12324345" }
            };

            context.AddRange(contacts);
            context.SaveChanges();
        }
    }
}
