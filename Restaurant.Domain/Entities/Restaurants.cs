using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{


    public class Restaurants
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }

        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public Address? address { get; set;}

        public List<Dish>? Dishes { get; set; } = new List<Dish>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }

    public class Dish
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = default!;
        public string Price { get; set; } = default!;

        // 👇 Add this navigation property back
        //public Restaurants? Restaurants { get; set; }
    }
    

}
