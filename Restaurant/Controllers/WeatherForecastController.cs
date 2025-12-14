using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;
using Restaurant.Application.Constants;
using Restaurant.Application.CQRS.COMMAND;
using Restaurant.Application.CQRS.COMMAND.DELETE;
using Restaurant.Application.CQRS.COMMAND.UPDATE;
using Restaurant.Application.CQRS.QUERIES.GetAllRestaurants;
using Restaurant.Application.CQRS.QUERIES.GetRestaurantById;
using Restaurant.Application.DishCQRS.Commands;
using Restaurant.Application.DishCQRS.FindAllCommand;
using Restaurant.Application.DishCQRS.FindOneCommand;
using Restaurant.Application.DishCQRS.UpdateCommand;
using Restaurant.Application.Services;
using Restaurant.Domain.DTOS;
using Restaurant.Infrastucture.Authorization;
using System.Diagnostics;

namespace Restaurant.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
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
    private readonly IServiceProvider _serviceProvider;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRestaurantServices restaurantServices, IMapper mapper, IMediator mediatR, IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.restaurantServices = restaurantServices;
        this.mapper = mapper;
        this._mediator = mediatR;
        this._serviceProvider = serviceProvider;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [AllowAnonymous]
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
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<ActionResult> GetAllRestaurants()
    {
        try
        {
            _logger.LogInformation("Starting to fetch all restaurants...");
            var stopwatch = Stopwatch.StartNew();

            var restaurants = await _mediator.Send(new GetAllRestaurantsCommand());

            var restaurantsCommand = restaurants.Select(GetAllRestaurantsCommand.FromEntity).ToList();

            stopwatch.Stop();
            _logger.LogInformation("Successfully fetched {RestaurantCount} restaurants in {ElapsedMilliseconds}ms",
                restaurantsCommand.Count, stopwatch.ElapsedMilliseconds);

            return Ok(restaurantsCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all restaurants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpGet("find")]
    [Authorize(Policy = PolicyNames.HasNationality)]
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


    [HttpPost("createdish/{restaurantId}")]
    public async Task<ActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        try
        {
            _logger.LogInformation("Starting creation dish");

            command.RestaurantId = restaurantId;

            var validator = _serviceProvider.GetRequiredService<IValidator<CreateDishCommand>>();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var dish = await _mediator.Send(command);

            if(dish == null)
            {
                return NotFound(new
                {
                    Status = "404",
                    Message = $"Restaurant with ID {restaurantId} was not found or dish creation failed."
                });
            }

            //return Ok(new
            //{
            //    Status = "200",
            //    Message = "Dish created successfully",
            //    Data = dish
            //});

            return Ok(dish);

        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occured whiet creating a dish");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpPut("updatedish/{restaurantId}")]
    public async Task<ActionResult> UpdateDish([FromRoute] int restaurantId, [FromBody] UpdateDishCommandV2 command)
    {
        try
        {
            _logger.LogInformation("Starting updating dish");

            command.RestaurantId = restaurantId;

            var validator = _serviceProvider.GetRequiredService<IValidator<UpdateDishCommandV2>>();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var dish = await _mediator.Send(command);

            if (dish == null)
            {
                return NotFound(new
                {
                    Status = "404",
                    Message = $"Restaurant with ID {restaurantId} was not found or dish creation failed."
                });
            }

            //return Ok(new
            //{
            //    Status = "200",
            //    Message = "Dish created successfully",
            //    Data = dish
            //});

            return Ok(dish);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured whiet creating a dish");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }


    [HttpGet("{restaurantId}/dishes")]
    public async Task<ActionResult> findAllDished([FromRoute] int restaurantId)
    {
        try
        {
            _logger.LogInformation("gettting all  dishes");

            FindAllDishesCommand command = new FindAllDishesCommand();

            command.RestaurantId = (int)restaurantId;

            var dishes = await _mediator.Send(command);

            if (dishes == null)
            {
                return NotFound(new
                {
                    Status = "404",
                    Message = $"Dishes with restaurantId {restaurantId} were not found"
                });
            }


            return Ok(new
            {
                Status = "200",
                Message = "Dishes retrieved successfully",
                Data = dishes
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while findinga dish");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }

    [HttpGet("{restaurantId}/dishes/{dishId}")]
    public async Task<ActionResult> findDish([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        try
        {
            _logger.LogInformation("Starting updating dish");
            FindOneDishCommand command = new FindOneDishCommand();

            command.RestaurantId = restaurantId;
            command.Id = dishId;

            var dish = await _mediator.Send(command);

            if (dish == null)
            {
                return NotFound(new
                {
                    Status = "404",
                    Message = $"Restaurant with ID {restaurantId} was not found or dish Id not found"
                });
            }

            //return Ok(new
            //{
            //    Status = "200",
            //    Message = "Dish created successfully",
            //    Data = dish
            //});

            return Ok(dish);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while findinga dish");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while processing your request" });
        }
    }
}
