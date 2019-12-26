using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using IpFinderREST.Models;
using Microsoft.EntityFrameworkCore;

namespace IpFinderREST
{
    public static class Helpers
    {
        public static ApplicationContext GetContext()
        {
            var connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                var options = optionsBuilder.UseNpgsql(connectionString).Options;
                return new ApplicationContext(options);
            }
            else
            {
                return null;
            }
        }
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory() + @"\Properties");
            try
            {
                builder.AddJsonFile("launchSettings.json");
                var config = builder.Build();
                return config.GetConnectionString("DefaultConnection");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
