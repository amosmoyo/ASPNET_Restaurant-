using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Restaurant.Infrastucture.Persistence
{
    internal class RestaurantsDbContextFactory : IDesignTimeDbContextFactory<RestaurantsDbContext>
    {
        public RestaurantsDbContext CreateDbContext(string[] args)
        {
            // Get the base path of the API project (where appsettings.json resides)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Restaurant");

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            string? _dbConn = configuration["ConnectionString:dbConn"];
            var connectionString = configuration.GetConnectionString("dbConn");

            var optionsBuilder = new DbContextOptionsBuilder<RestaurantsDbContext>();
            optionsBuilder.UseSqlServer(_dbConn);

            return new RestaurantsDbContext(optionsBuilder.Options);
        }
    }
}
