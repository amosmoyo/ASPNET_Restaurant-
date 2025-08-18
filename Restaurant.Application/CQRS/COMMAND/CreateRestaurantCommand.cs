using MediatR;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.COMMAND
{
    public class CreateRestaurantCommand: IRequest<Restaurants?>
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public List<CreateDishesCommand>? Dishes { get; set; } = new List<CreateDishesCommand>();

        public static CreateRestaurantCommand FromEntity(Restaurants restaurants)
        {
            if (restaurants == null) return null;

            return new CreateRestaurantCommand
            {
                Id = restaurants.Id,
                Name = restaurants.Name,
                Description = restaurants.Description,
                Category = restaurants.Category,
                HasDelivery = restaurants.HasDelivery,
                ContactEmail = restaurants.ContactEmail,
                ContactNumber = restaurants.ContactNumber,
                CreatedAt = restaurants.CreatedAt,
                City = restaurants?.address?.City,
                Street = restaurants?.address?.Street,
                PostalCode = restaurants?.address?.PostalCode,
                Dishes = restaurants?.Dishes?.Select(CreateDishesCommand.FromEntity).ToList() ?? []
            };
        }

    }

    public class CreateDishesCommand: IRequest<Dish?>
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = default!;
        public string Price { get; set; } = default!;

        public static CreateDishesCommand FromEntity(Dish dish)
        {
            if (dish == null) return null;

            return new CreateDishesCommand
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                RestaurantId = dish.RestaurantId,
                Price = dish.Price,
            };
        }
    }
}
