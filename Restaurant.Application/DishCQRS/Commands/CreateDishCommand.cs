using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.Commands
{
    public class CreateDishCommand: IRequest<Dish?>
    {
        public int RestaurantId { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = default!;
        public string Price { get; set; } = default!;
    }
}
