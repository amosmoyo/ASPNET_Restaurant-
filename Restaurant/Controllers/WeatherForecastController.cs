using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;
using Restaurant.Application.CQRS.COMMAND;
using Restaurant.Application.CQRS.COMMAND.DELETE;
using Restaurant.Application.CQRS.COMMAND.UPDATE;
using Restaurant.Application.CQRS.QUERIES.GetAllRestaurants;
using Restaurant.Application.CQRS.QUERIES.GetRestaurantById;
using Restaurant.Application.Services;
using Restaurant.Domain.DTOS;
using System.Diagnostics;

namespace Restaurant.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    IRestaurantServices restaurantServices;
    IMapper mapper;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRestaurantServices restaurantServices, IMapper mapper, IMediator mediatR)
    {
        _logger = logger;
        this.restaurantServices = restaurantServices;
        this.mapper = mapper;
        this._mediator = mediatR;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllRestaurants()
    {
        try
        {
            _logger.LogInformation("Starting to fetch restaurants...");
            var stopwatch = Stopwatch.StartNew();

            var restaurants = await _mediator.Send(new GetAllRestaurantsCommand());

            var restaurantsCommand = restaurants.Select(GetAllRestaurantsCommand.FromEntity).ToList();

            stopwatch.Stop();
            _logger.LogInformation("Successfully fetched {RestaurantCount} restaurants in {ElapsedMilliseconds}ms",
                restaurants.Count(), stopwatch.ElapsedMilliseconds);


            // Return the complete list
            return Ok(restaurantsCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching restaurants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpGet("find")]
    public async Task<ActionResult> GetRestaurant([FromQuery] int Id)
    {
        try
        {
            _logger.LogInformation("Starting to fetch restaurants...");

            var stopwatch = Stopwatch.StartNew();

            // var restaurant = await restaurantServices.GetRestaurant(Id);
            var restaurant = await _mediator.Send(new GetRestaurantByIdCommand{ Id = Id });

          

            stopwatch.Stop();

            _logger.LogInformation("Successfully fetched {RestaurantCount} restaurants in {ElapsedMilliseconds}ms",
                restaurant, stopwatch.ElapsedMilliseconds);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Return the complete list
            return Ok(restaurant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching restaurants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult> Create([FromBody] CreateRestaurantCommand command)
    {
        var restaurant = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, mapper.Map<RestaurantsDTO>(restaurant));
    }


    [HttpPut]
    public async Task<ActionResult> UpdateRestaurants([FromQuery] int Id, [FromBody] UpdateRestaurantCommand command)
    {
        try
        {
            _logger.LogInformation("Starting to fetch restaurants...");
            var stopwatch = Stopwatch.StartNew();

            command.Id = Id;

            var UpdatedRestaurant = await _mediator.Send(command);

            if (UpdatedRestaurant == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetRestaurant), new { id = UpdatedRestaurant.Id }, mapper.Map<RestaurantsDTO>(UpdatedRestaurant));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching restaurants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRestaurants([FromQuery] int Id)
    {
        try
        {
            _logger.LogInformation("Starting to delete operations...");

            var stopwatch = Stopwatch.StartNew();

            DeleteRestaurantCommand command = new DeleteRestaurantCommand{Id = Id };

            var restaurantId = await _mediator.Send(command);

            if (restaurantId == 0)
            {
                return NotFound();
            }

            return Ok(new { Message = "Restaurant deleted successfully", Id = restaurantId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching restaurants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }
}
