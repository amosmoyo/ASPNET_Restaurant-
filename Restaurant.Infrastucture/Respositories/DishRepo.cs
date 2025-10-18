using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Respositories
{
    internal class DishRepo(RestaurantsDbContext dbContext, IMapper mapper) : IDishRepo
    {


        public async Task<Dish?> CreateDishAsync(Dish dish)
        {
            if (dish == null)
            {
                return null;
            }


            //var strategy = dbContext.Database.CreateExecutionStrategy();

            Dish? createdDish = null;

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await dbContext.Dishes.AddAsync(dish);

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                createdDish = dish;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw;
            }

            return createdDish;
        }

        public async Task<Dish?> UpdateDishAsync(double price, int dishId)
        {
            Dish? updatedDish = null;

            var dish = await dbContext.Dishes.FirstOrDefaultAsync(dbContext => dbContext.Id == dishId);

            if (dish != null)
            {
                dish.Price = price.ToString("F2");

                dbContext.Update(dish);

                await dbContext.SaveChangesAsync(true);

                updatedDish = dish;
            }

            return updatedDish;

        }

        public async Task<Dish?> FindOneDishAsync(int dishId)
        {
            var dish = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

            if (dish == null)
            { 
                return null;
            }

            return dish;
        }


        public async Task<IEnumerable<Dish?>> FindAllDishAsync(int restaurantId)
        {
            return  await dbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();
        }

    }
}
