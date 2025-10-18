using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.FindAllCommand
{
    public class FindAllDishesCommandHandler(ILogger<FindAllDishesCommandHandler> logger, IDishServices dishServices): IRequestHandler<FindAllDishesCommand, IEnumerable<Dish?>>
    {
        public async Task<IEnumerable<Dish?>> Handle(FindAllDishesCommand command, CancellationToken cancellationToken)
        {
            var dishes = await dishServices.FindAllDish(command.RestaurantId);

            if (dishes == null || !dishes.Any()) 
            {
                return Enumerable.Empty<Dish?>();
            }

            return dishes;
        }
    }
}
