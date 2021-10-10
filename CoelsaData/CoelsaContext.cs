using CoelsaCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace CoelsaData
{
    public class CoelsaContext : DbContext
    {
        public CoelsaContext(DbContextOptions<CoelsaContext> options):base(options) {}
        public DbSet<Contact> Contacts { get; set; }
    }
}
