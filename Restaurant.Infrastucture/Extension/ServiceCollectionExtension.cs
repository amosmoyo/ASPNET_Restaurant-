using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.AutoMappers;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastucture.Persistence;
using Restaurant.Infrastucture.Respositories;
using Restaurant.Infrastucture.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            string? _dbConn = configuration["ConnectionString:dbConn"];

            services.AddDbContext<RestaurantsDbContext>(options =>

                options.UseSqlServer(_dbConn)
                .EnableSensitiveDataLogging(false)  // disable sensitive data logging
                .EnableDetailedErrors()
            );

            services.AddScoped<IRestaurantSeeder, RestaurantSeader>();
            services.AddScoped<IRestaurantsRepo, RestaurantsRepo>();
            services.AddAutoMapper(typeof(RestaurantProfile).Assembly);
        }

    }
}
