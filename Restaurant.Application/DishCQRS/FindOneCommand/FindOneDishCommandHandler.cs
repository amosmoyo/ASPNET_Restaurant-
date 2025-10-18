using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.FindOneCommand
{
    public class FindOneDishCommandHandler(ILogger<FindOneDishCommandHandler> logger, IDishServices dishServices, IRestaurantServices restaurantServices) : IRequestHandler<FindOneDishCommand, Dish?>
    {
        public async Task<Dish?> Handle(FindOneDishCommand findOneDishCommand, CancellationToken cancellationToken)
        {

            var restaurant = await restaurantServices.GetRestaurant(findOneDishCommand.RestaurantId);

            if (restaurant == null)
            {
                return null;
            }

            return await dishServices.FindOneDish(findOneDishCommand.Id);
        }
    }
}
