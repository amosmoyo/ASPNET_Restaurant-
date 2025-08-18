using AutoMapper;
using Restaurant.Application.CQRS.COMMAND;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DTOs
{
    public class RestaurantsProfilers: Profile
    {
        public RestaurantsProfilers() 
        {
            //CQRS
            CreateMap<CreateRestaurantCommand, Restaurants>()
            .ForMember(dest => dest.address, opt => opt.MapFrom(src => new Address
            {
                City = src.City,
                Street = src.Street,
                PostalCode = src.PostalCode,
            }))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes));

            CreateMap<CreateDishesCommand, Dish>();
        }
    }
}
