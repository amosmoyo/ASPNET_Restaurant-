using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services
{
    public interface IDishServices
    {
        Task<Dish?> CreateDish(Dish dish);

        Task<Dish?> UpdateDish(double price, int dishdId);

        Task<Dish?> FindOneDish(int dishId);

        Task<IEnumerable<Dish?>> FindAllDish(int restaurantId);
    }
}
