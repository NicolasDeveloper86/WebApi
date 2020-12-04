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

            services.AddDbContext<CoelsaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CoelsaConnection"))
            );

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IValidator<Contact>, ContactValidator>();

            services
                .AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter());
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


                    bool isNotInMemory = !context.Database.IsInMemory();

                    if (isNotInMemory && contextEnsureDeleteAndMigrate.ContextEnsureDeleteAndMigrate)
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
