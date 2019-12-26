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
        //public DbSet<Country> Countries { get; set; }
        public DbSet<IP> IPs { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<Subdivision> Subdivisions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseNpgsql(optionsBuilder);
        }
        public static void ClearTable(ApplicationContext context, string tableName)
        {
            try
            {
                context.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Problem with clear table {tableName}\n{ex.ToString()}");
            }
        }
    }
}
