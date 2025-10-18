using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IDishRepo
    {
        Task<Dish?> CreateDishAsync(Dish dish);

        Task<Dish?> UpdateDishAsync(double price, int dishId);

        Task<Dish?> FindOneDishAsync(int dishId);

        Task<IEnumerable<Dish?>> FindAllDishAsync(int restaurantId);
    }
}
