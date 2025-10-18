using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services
{
    public class DishServices(IDishRepo dishRepo) : IDishServices
    {
        public async Task<Dish?> CreateDish(Dish dish)
        {
            return await dishRepo.CreateDishAsync(dish);
        }

        public async Task<Dish?> UpdateDish(double price, int dishdId)
        {
            return await dishRepo.UpdateDishAsync(price, dishdId);
        }

        public async Task<Dish?> FindOneDish(int dishId)
        {
            return await dishRepo.FindOneDishAsync(dishId);
        }

        public async Task<IEnumerable<Dish?>> FindAllDish(int restaurantId)
        {
            return await dishRepo.FindAllDishAsync(restaurantId);
        }
    }
}
