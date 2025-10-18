using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.CQRS.COMMAND;
using Restaurant.Application.CQRS.COMMAND.UPDATE;
using Restaurant.Application.DishCQRS.Commands;
using Restaurant.Application.DTOs;
using Restaurant.Application.Services;
using Restaurant.Domain.AutoMappers;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Extension;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IRestaurantServices, RestaurantServices>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantCommand).Assembly));
        services.AddAutoMapper(typeof(RestaurantsProfilers).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateDishCommand>();
        //services.AddValidatorsFromAssemblyContaining<UpdateDishCommand>();
        services.AddScoped<IDishServices, DishServices>();
        services.AddAutoMapper(typeof(DishesProfiler).Assembly);
    }
}

