using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.FindOneCommand
{
    public class FindOneDishCommand: IRequest<Dish?>
    {
        public int RestaurantId { get; set; }

        public int Id { get; set; }
    }
}
