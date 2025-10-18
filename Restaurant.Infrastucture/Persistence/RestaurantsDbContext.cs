using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Persistence
{
    internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : DbContext(options) // This is called a primary constructor
    {
        //public RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options): base(options) { }
        internal DbSet<Restaurants> Restaurants { get; set; }

        internal DbSet<Dish> Dishes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source =DESKTOP-V1U1B6U\\MSSQLSERVER01; Initial Catalog =Elma; User ID =amosmoyo; Password =password@123456; MultipleActiveResultSets =True; Min Pool Size =5; Max Pool Size =5000; Connect Timeout =180; Application Name =Restaurants;TrustServerCertificate=True;");
        //    }

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurants>()
                .OwnsOne(r => r.address);

            //modelBuilder.Entity<Restaurants>()
            //            .HasMany(r => r.Dishes)
            //            .WithOne(r => r.Restaurants)
            //            .HasForeignKey(d => d.RestaurantId);

            // Restaurant-Address relationship (one-to-one)
            //modelBuilder.Entity<Restaurants>()
            //    .HasOne(r => r.address)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            // Restaurant-Dishes relationship (one-to-many)

            //This defines a directional relationship
            //modelBuilder.Entity<Restaurants>()
            //    .HasMany(r => r.Dishes)
            //    .WithOne(d => d.Restaurants)
            //    .HasForeignKey(d => d.RestaurantId)
            //    .OnDelete(DeleteBehavior.Cascade);


            //This defines a unidirectional relationship — Restaurant knows its dishes, but a Dish doesn’t know its Restaurant.
            modelBuilder.Entity<Restaurants>()
            .HasMany(r => r.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
