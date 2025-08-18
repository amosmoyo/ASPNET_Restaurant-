using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services;

public class RestaurantServices(IRestaurantsRepo RestaurantsRepo) : IRestaurantServices
{
    public async Task<IEnumerable<Restaurants>> GetAllRestaurants()
    {
        var restaurants = await RestaurantsRepo.GetAllAsync();

        return restaurants;
    }

    public async Task<Restaurants?> GetRestaurant(int Id)
    {
        var restaurants = await RestaurantsRepo.GetOneAsync(Id);

        return restaurants;
    }

    public async Task<Restaurants?> CreateRestaurant(RestaurantsDTO restaurantDto)
    {
        var restaurants = await RestaurantsRepo.CreateAsync(restaurantDto);

        return restaurants;
    }

    public async Task<Restaurants?> CreateRestaurantv2(Restaurants restaurant)
    {
        var restaurants = await RestaurantsRepo.CreateAsyncv2(restaurant);

        return restaurants;
    }

    public async Task<Restaurants?> UpdateRestaurant(int Id, RestaurantsDTO restaurantsDTO)
    {
        var restaurant = await RestaurantsRepo.UpdateRestaurantAsync(Id, restaurantsDTO);

        return restaurant;
    }
}

