using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using IpFinderREST.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;

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
        public static bool IsIpInSubnet(this IPAddress address, string subnetMask)
        {
            var slashIdx = subnetMask.IndexOf("/");
            if (slashIdx == -1)
            { 
                throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");
            }

            var maskAddress = IPAddress.Parse(subnetMask.Substring(0, slashIdx));

            if (maskAddress.AddressFamily != address.AddressFamily)
            {
                return false;
            }

            int maskLength = int.Parse(subnetMask.Substring(slashIdx + 1));

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                var maskAddressBits = BitConverter.ToUInt32(maskAddress.GetAddressBytes().Reverse().ToArray(), 0);

                var ipAddressBits = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);

                uint mask = uint.MaxValue << (32 - maskLength);

                return (maskAddressBits & mask) == (ipAddressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                var maskAddressBits = new BitArray(maskAddress.GetAddressBytes());

                var ipAddressBits = new BitArray(address.GetAddressBytes());

                if (maskAddressBits.Length != ipAddressBits.Length)
                {
                    throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");
                }

                for (int i = 0; i < maskLength; i++)
                {
                    if (ipAddressBits[i] != maskAddressBits[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new NotSupportedException("Only InterNetworkV6 or InterNetwork address families are supported.");
        }
    }
}
