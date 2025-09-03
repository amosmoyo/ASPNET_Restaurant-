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

namespace Restaurant.Application.CQRS.COMMAND.UPDATE
{
    public class UpdateRestaurantCommandHandler(IRestaurantServices restaurantServices, ILogger<UpdateRestaurantCommand> logger, IMapper mapper): IRequestHandler<UpdateRestaurantCommand, Restaurants?>
    {
        public async Task<Restaurants?> Handle(UpdateRestaurantCommand command,  CancellationToken cancellationToken)
        {
            var restaurant = mapper.Map<Restaurants>(command);

            var UpdatedRestaurant = await restaurantServices.UpdateRestaurantv2(restaurant.Id, restaurant);

            if (UpdatedRestaurant == null)
            {
                return null;
            }

            return UpdatedRestaurant;
        }
    }
}
