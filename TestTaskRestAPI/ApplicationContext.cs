using System;
using Microsoft.EntityFrameworkCore;
using TestTaskRestAPI.DataModel;

namespace TestTaskRestAPI
{
    public class ApplicationContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<IP> IPs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        }
    }
}
