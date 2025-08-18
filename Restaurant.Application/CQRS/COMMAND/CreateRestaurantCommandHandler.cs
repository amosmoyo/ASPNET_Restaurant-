using AutoMapper;
using MediatR;
using Restaurant.Application.Services;
using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.COMMAND
{
    public class CreateRestaurantCommandHandler(IRestaurantServices restaurantServices, IMapper mapper) : IRequestHandler<CreateRestaurantCommand, Restaurants>
    {
        public async Task<Restaurants?> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var dishesCount = command.Dishes?.Count ?? 0;

            var restaurantLog = mapper.Map<Restaurants>(command);

            var restaurant = await restaurantServices.CreateRestaurantv2(restaurantLog);

           return restaurant!;
        }
    }
}
