using AutoMapper;
using Restaurant.Application.DishCQRS.Commands;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DTOs
{
    public class DishesProfiler: Profile
    {
        public DishesProfiler() 
        {
            CreateMap<CreateDishCommand, Dish>();
        }
    }
}
