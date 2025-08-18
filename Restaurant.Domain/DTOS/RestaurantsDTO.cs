using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.DTOS
{
    public class RestaurantsDTO
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

        public List<DishDTO>? Dishes { get; set; } = new List<DishDTO>();

        public static RestaurantsDTO FromEntity(Restaurants restaurants)
        {
            if(restaurants == null) return null;

            return new RestaurantsDTO
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
                Dishes = restaurants?.Dishes?.Select(DishDTO.FromEntity).ToList() ?? []
            };
        }

    }

    public class DishDTO
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = default!;
        public string Price { get; set; } = default!;

        public static DishDTO FromEntity(Dish dish)
        {
            if (dish == null) return null;

            return new DishDTO 
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
