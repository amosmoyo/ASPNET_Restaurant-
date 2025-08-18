using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Services;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.QUERIES.GetAllRestaurants
{
    public class GetAllRestaurantsCommandHandler (IRestaurantServices restaurantServices, ILogger<GetAllRestaurantsCommandHandler> logger, IMapper mapper): IRequestHandler<GetAllRestaurantsCommand, IEnumerable<Restaurants>>
    {
        public async Task<IEnumerable<Restaurants>> Handle(GetAllRestaurantsCommand command, CancellationToken cancellationToken)
        {

            var restaurants = await restaurantServices.GetAllRestaurants();



            return restaurants;
        }
    }


}
