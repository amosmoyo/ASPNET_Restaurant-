using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.UpdateCommand
{
    public class UpdateDishCommandHandler(ILogger<UpdateDishCommandHandler> logger, IDishServices dishServices, IRestaurantServices restaurantServices): IRequestHandler<UpdateDishCommandV2, Dish>
    {
        public async Task<Dish?> Handle(UpdateDishCommandV2 updateCommand, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantServices.GetRestaurant(updateCommand.RestaurantId);

            if (restaurant == null) 
            {
                return null;
            }

            Dish dish = await dishServices.UpdateDish(double.Parse(updateCommand.Price), updateCommand.Id);

            return dish;
        }
    }
}
