using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UpdateDBConsoleApp.DataModel;

namespace UpdateDBConsoleApp
{
    class ApplicationContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<IPs> IPs { get; set; }
        public DbSet<LocalizedNames> LocalizedNames { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=password");
        }
    }
}
