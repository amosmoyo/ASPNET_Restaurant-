using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.CQRS.QUERIES.GetAllRestaurants;
using Restaurant.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.COMMAND.DELETE
{
    public class DeleteRestaurantCommandHandler(IRestaurantServices restaurantServices, ILogger<DeleteRestaurantCommandHandler> logger, IMapper mapper): IRequestHandler<DeleteRestaurantCommand, int>
    {
        public async Task<int> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurantId = await restaurantServices.DeleteRestaurant(command.Id);

            return restaurantId;
        }
    }
}
