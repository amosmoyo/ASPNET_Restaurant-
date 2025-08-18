using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Application.Services;
using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using System.Diagnostics;

namespace Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController(IRestaurantServices restaurantServices, ILogger<RestaurantController> logger, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllRestaurants()
        {
            try
            {
                logger.LogInformation("Starting to fetch restaurants...");
                var stopwatch = Stopwatch.StartNew();

                var restaurants = await restaurantServices.GetAllRestaurants();

                var restaurantsDTO = restaurants.Select(RestaurantsDTO.FromEntity).ToList();

                stopwatch.Stop();
                logger.LogInformation("Successfully fetched {RestaurantCount} restaurants in {ElapsedMilliseconds}ms",
                    restaurants.Count(), stopwatch.ElapsedMilliseconds);


                // Return the complete list
                return Ok(restaurantsDTO);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Error occurred while fetching restaurants");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("find")]
        public async Task<ActionResult> GetRestaurant([FromQuery]int Id)
        {
            try
            {
                logger.LogInformation("Starting to fetch restaurants...");
                var stopwatch = Stopwatch.StartNew();

                var restaurant = await restaurantServices.GetRestaurant(Id);

                var restaurantsDTO = RestaurantsDTO.FromEntity(restaurant);

                stopwatch.Stop();
                logger.LogInformation("Successfully fetched {RestaurantCount} restaurants in {ElapsedMilliseconds}ms",
                    restaurant, stopwatch.ElapsedMilliseconds);

                if(restaurantsDTO == null)
                {
                    return NotFound();
                }

                // Return the complete list
                return Ok(restaurantsDTO);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching restaurants");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while processing your request" });
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> Create(RestaurantsDTO restaurantDto)
        {
            var dishesCount = restaurantDto.Dishes?.Count ?? 0;

            var restaurant = await restaurantServices.CreateRestaurant(restaurantDto);

            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, mapper.Map<RestaurantsDTO>(restaurant));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRestaurants([FromQuery] int Id, [FromBody] RestaurantsDTO restaurantUpdateDTO)
        {
            try
            {
                logger.LogInformation("Starting to fetch restaurants...");
                var stopwatch = Stopwatch.StartNew();

                var restaurant = await restaurantServices.GetRestaurant(Id);

                if (restaurant == null) 
                { 
                    return NotFound();
                }

                var UpdatedRestaurant = await restaurantServices.UpdateRestaurant(Id, restaurantUpdateDTO);

                if(UpdatedRestaurant == null)
                {
                    return NotFound();
                }

                return CreatedAtAction(nameof(GetRestaurant), new { id = UpdatedRestaurant.Id }, mapper.Map<RestaurantsDTO>(UpdatedRestaurant));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching restaurants");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while processing your request" });
            }
        }
    }
}
