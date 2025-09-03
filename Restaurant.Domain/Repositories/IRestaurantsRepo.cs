using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IRestaurantsRepo
    {
        Task<IEnumerable<Restaurants>> GetAllAsync();

        Task<Restaurants?> GetOneAsync(int Id);

        Task<Restaurants?> CreateAsync(RestaurantsDTO restaurants);

        Task<Restaurants?> CreateAsyncv2(Restaurants restaurants);

        Task<Restaurants?> UpdateRestaurantAsync(int Id, RestaurantsDTO restaurantsDTO);

        Task<Restaurants?> UpdateRestaurantAsyncv2(int Id, Restaurants restaurants);

        Task<int> DeleteRestaurantAsyncv2(int Id);
    }
}
