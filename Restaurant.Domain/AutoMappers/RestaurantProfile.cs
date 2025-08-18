using AutoMapper;
using Restaurant.Domain.DTOS;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.AutoMappers
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            //Map from DTO to Restaurants

            CreateMap<RestaurantsDTO, Restaurants>()
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode,
                }))
                .ForMember(dest => dest.Dishes, opt => opt.Ignore());


            CreateMap<DishDTO, Dish>()
                .ForMember(dest => dest.RestaurantId, opt => opt.Ignore()); ;

            // Map from Entity to DTO (if needed)
            //CreateMap<Restaurants, RestaurantsDTO>()
            //    .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.address.City))
            //    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.address.Street))
            //    .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.address.PostalCode))
            //    .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes));

            CreateMap<Restaurants, RestaurantsDTO>()
             .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.address != null ? src.address.City : null))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.address != null ? src.address.Street : null))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.address != null ? src.address.PostalCode : null))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src =>
                src.Dishes != null ?
                src.Dishes.Select(d => new DishDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    RestaurantId = d.RestaurantId
                }).ToList() :
                new List<DishDTO>()))
            .AfterMap((src, dest) =>
            {
                // Ensure CreatedAt is properly mapped
                dest.CreatedAt = src.CreatedAt;
            });





        }
    }
}
