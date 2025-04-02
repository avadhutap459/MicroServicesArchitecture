using Mango.Service.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EmailLogger> emaillogger { get; set; }
    }
}
