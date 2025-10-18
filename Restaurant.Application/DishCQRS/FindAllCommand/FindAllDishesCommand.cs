using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.FindAllCommand
{
    public class FindAllDishesCommand: IRequest<IEnumerable<Dish?>>
    {
        public int RestaurantId { get; set; }
    }
}
