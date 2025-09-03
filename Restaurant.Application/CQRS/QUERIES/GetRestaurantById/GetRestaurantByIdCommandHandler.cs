using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.CQRS.QUERIES.GetAllRestaurants;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.QUERIES.GetRestaurantById
{
    public class GetRestaurantByIdCommandHandler(IRestaurantServices restaurantServices, ILogger<GetRestaurantByIdCommandHandler> logger, IMapper mapper) : IRequestHandler<GetRestaurantByIdCommand, Restaurants?>
    {
        public async Task<Restaurants?> Handle(GetRestaurantByIdCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantServices.GetRestaurant(command.Id);

            return restaurant;
        }
    }
}
