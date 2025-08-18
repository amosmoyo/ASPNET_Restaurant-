using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastucture.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Respositories
{
    internal class RestaurantsRepo(RestaurantsDbContext dbContext, IMapper mapper): IRestaurantsRepo
    {

        public async Task<IEnumerable<Restaurants>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.Include(r => r.Dishes).ToListAsync();

            return restaurants;
        }

        public async Task<Restaurants?> GetOneAsync(int Id)
        {
            var restaurants = await dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == Id);

            return restaurants;
        }

        public async Task<Restaurants?> CreateAsync(RestaurantsDTO restaurantDto)
        {
            //var restaurant = mapper.Map<Restaurants>(restaurantDto);

            //// Handle Dishes if they exist in DTO
            //if (restaurantDto.Dishes != null && restaurantDto.Dishes.Any())
            //{
            //    // Map dishes and set the correct RestaurantId
            //    var dishes = mapper.Map<List<Dish>>(restaurantDto.Dishes);
            //    foreach (var dish in dishes)
            //    {
            //        dish.RestaurantId = restaurant.Id; // Ensure this is set after SaveChanges if Id is generated
            //                                           // OR if Id is known beforehand:
            //        dish.Restaurant = restaurant; // This sets up the relationship properly
            //    }
            //    restaurant.Dishes = dishes;
            //}
            //dbContext.Restaurants.Add(restaurant);

            //await dbContext.SaveChangesAsync();

            //return restaurant;

            var strategy = dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                if (restaurantDto == null)
                {
                    return null;
                }

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                try
                {
                    var restaurant = mapper.Map<Restaurants>(restaurantDto);

                    restaurant.Id = 0;

                    dbContext.Restaurants.Add(restaurant);

                    await dbContext.SaveChangesAsync();

                    if (restaurantDto.Dishes.Any() && restaurantDto.Dishes != null)
                    {
                        var dishes = mapper.Map<List<Dish>>(restaurantDto.Dishes);

                        foreach (var dish in dishes)
                        {
                            dish.RestaurantId = restaurant.Id;
                            dish.Id = 0;
                        }

                        await dbContext.Dishes.AddRangeAsync(dishes);

                        await dbContext.SaveChangesAsync();

                    }

                    await transaction.CommitAsync();

                    return restaurant;
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();

                    return null;
                }
            });
        }

        public async Task<Restaurants?> CreateAsyncv2(Restaurants restaurant)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                if (restaurant == null)
                {
                    return null;
                }

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                try
                {
                    restaurant.Id = 0;

                    dbContext.Restaurants.Add(restaurant);

                    await dbContext.SaveChangesAsync();

                    if (restaurant.Dishes != null && restaurant.Dishes.Any())
                    {
                        foreach (var dish in restaurant.Dishes)
                        {
                            dish.RestaurantId = restaurant.Id;
                            dish.Id = 0;
                        }

                        await dbContext.Dishes.AddRangeAsync(restaurant.Dishes);

                        await dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    return restaurant;
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();

                    return null;
                }
            });
        }


        public async Task<Restaurants?> UpdateRestaurantAsync(int Id, RestaurantsDTO restaurantsDTO)
        {
            var existingRestaurant = await dbContext.Restaurants
                .Include(r => r.address)
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(r =>  r.Id == Id);

            if (existingRestaurant == null) 
            {
                return null;
            }

            //Map update update fro DTO to existing entity
            mapper.Map(restaurantsDTO, existingRestaurant);

            //Handle address update
            existingRestaurant.address ??= new Address();
            existingRestaurant.address.City = restaurantsDTO.City;
            existingRestaurant.address.PostalCode = restaurantsDTO.PostalCode;
            existingRestaurant.address.Street = restaurantsDTO.Street;


            //Handle dishes
            if (restaurantsDTO.Dishes.Any() && restaurantsDTO.Dishes != null) 
            {
                existingRestaurant.Dishes = mapper.Map<List<Dish>>(restaurantsDTO.Dishes);
            }


            try
            {
                await dbContext.SaveChangesAsync();

                return existingRestaurant;
            }
            catch (Exception ex) 
            {
                return null;
            }

        }
    }
}
