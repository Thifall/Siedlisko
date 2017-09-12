using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAcces.DbContext
{
    public class DbContextFactory : IDbContextFactory<SiedliskoContext>
    {
        public SiedliskoContext Create(DbContextFactoryOptions options)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(options.ContentRootPath)
                .AddJsonFile("DbSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"DbSettings.{options.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connectionString = config.GetConnectionString("SqlServerConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find given connection string.");
            }

            var optionsbuilder = new DbContextOptionsBuilder<SiedliskoContext>();
            optionsbuilder.UseSqlServer(connectionString);

            return new SiedliskoContext(optionsbuilder.Options);    
        }
    }
}
