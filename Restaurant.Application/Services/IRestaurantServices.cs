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
    }
}