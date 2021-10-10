using CoelsaCommon.Models;
using CoelsaCommon.Validation;
using CoelsaData;
using CoelsaData.Repositories;
using CoelsaWebApi.Filters;
using CoelsaWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CoelsaWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<CoelsaContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CoelsaConnection")));

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IValidator<Contact>, ContactValidator>();

            ILoggerFactory logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>();

            services.AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter(logger));
                })
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coelsa API V1.0");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<CoelsaContext>();
                    var contextEnsureDeleteAndMigrate = Configuration.GetSection("ContextOptions").Get<ContextOptions>();

                    if (!context.Database.ProviderName.Contains("Microsoft.EntityFrameworkCore.InMemory") && contextEnsureDeleteAndMigrate.ContextEnsureDeleteAndMigrate)
                    {
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
