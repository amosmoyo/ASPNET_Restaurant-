using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Services
{
    public interface IRestaurantServices
    {
        Task<IEnumerable<Restaurants>> GetAllRestaurants();

        Task<Restaurants?> GetRestaurant(int Id);

        Task<Restaurants?> CreateRestaurant(RestaurantsDTO restaurantDto);

        Task<Restaurants?> CreateRestaurantv2(Restaurants restaurant);

        Task<Restaurants?> UpdateRestaurant(int Id, RestaurantsDTO restaurantsDTO);

        Task<Restaurants?> UpdateRestaurantv2(int Id, Restaurants restaurants);

        Task<int> DeleteRestaurant(int Id);
    }
}