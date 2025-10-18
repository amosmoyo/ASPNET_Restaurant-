using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.Commands
{
    public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger, IDishServices dishServices, IRestaurantServices restaurantServices, IMapper  mapper) : IRequestHandler<CreateDishCommand, Dish>
    {

        public async Task<Dish?> Handle(CreateDishCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantServices.GetRestaurant(command.RestaurantId);

            if (restaurant == null) 
            { 
                return null;
            }

            var mappedDish = mapper.Map<Dish>(command);

            var dish = await dishServices.CreateDish(mappedDish);

            if(dish == null) { return null; }

            return dish;
        }
    }
}
