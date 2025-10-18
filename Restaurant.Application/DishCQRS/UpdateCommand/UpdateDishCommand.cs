using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.UpdateCommand
{
    public class UpdateDishCommandV2: IRequest<Dish?>
    {
        public int RestaurantId { get; set; }
        public string Price { get; set; } = default!;
        public int Id { get; set; }
    }
}
