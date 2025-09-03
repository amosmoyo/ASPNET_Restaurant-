using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.COMMAND.UPDATE
{
    public class UpdateRestaurantCommand: IRequest<Restaurants?>
    {
        public int Id { get; set; }
        public string? Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string? Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public List<UpdateDishCommand>? Dishes { get; set; } = new List<UpdateDishCommand>();

        public static UpdateRestaurantCommand FromEntity(Restaurants restaurants)
        {
            if (restaurants == null) return null;

            return new UpdateRestaurantCommand
            {
                Name = restaurants.Name,
                Description = restaurants.Description,
                Category = restaurants.Category,
                HasDelivery = restaurants.HasDelivery,
                ContactEmail = restaurants.ContactEmail,
                ContactNumber = restaurants.ContactNumber,
                City = restaurants?.address?.City,
                Street = restaurants?.address?.Street,
                PostalCode = restaurants?.address?.PostalCode,
                Dishes = restaurants?.Dishes?.Select(UpdateDishCommand.FromEntity).ToList() ?? []
            };
        }
    }

    public class UpdateDishCommand : IRequest<Dish?>
    {
        public string? Name { get; set; }
        public string Description { get; set; } = default!;
        public string Price { get; set; } = default!;

        public static UpdateDishCommand FromEntity(Dish dish)
        {
            if (dish == null) return null;

            return new UpdateDishCommand
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price
            };
        }
    }
}
