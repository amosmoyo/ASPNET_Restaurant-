using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Seeders
{
    internal class RestaurantSeader(RestaurantsDbContext dbContext): IRestaurantSeeder
    {

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetPreconfiguredRestaurants();
                    await dbContext.Restaurants.AddRangeAsync(restaurants);
                    await dbContext.SaveChangesAsync();
                }
            }
        }


        private IEnumerable<Restaurants> GetPreconfiguredRestaurants()
        {
            var restaurants = new List<Restaurants>
                                {
                                    new Restaurants
                                    {
                                        Name = "Pizza Palace",
                                        Description = "Best pizza in town",
                                        Category = "Italian",
                                        HasDelivery = true,
                                        ContactEmail = "contact@pizzapalace.com",
                                        ContactNumber = "+123456789",
                                        address = new Address
                                        {
                                            City = "Nairobi",
                                            Street = "Kenyatta Avenue",
                                            PostalCode = "00100"
                                        },
                                        Dishes = new List<Dish>
                                        {
                                            new Dish { Name = "Margherita", Description = "Classic cheese and tomato", Price = "8.99" },
                                            new Dish { Name = "Pepperoni", Description = "Pepperoni and cheese", Price = "9.99" }
                                        },
                                        CreatedAt = DateTime.UtcNow
                                    },
                                    new Restaurants
                                    {
                                        Name = "Sushi World",
                                        Description = "Fresh sushi and sashimi",
                                        Category = "Japanese",
                                        HasDelivery = false,
                                        ContactEmail = "info@sushiworld.com",
                                        ContactNumber = "+987654321",
                                        address = new Address
                                        {
                                            City = "Mombasa",
                                            Street = "Moi Avenue",
                                            PostalCode = "80100"
                                        },
                                        Dishes = new List<Dish>
                                        {
                                            new Dish { Name = "Salmon Nigiri", Description = "Fresh salmon over rice", Price = "12.99" },
                                            new Dish { Name = "California Roll", Description = "Crab, avocado, and cucumber", Price = "10.99" }
                                        },
                                        CreatedAt = DateTime.UtcNow
                                    },
                                    new Restaurants
                                    {
                                        Name = "Spice Garden",
                                        Description = "Authentic Indian cuisine",
                                        Category = "Indian",
                                        HasDelivery = true,
                                        ContactEmail = "contact@spicegarden.co.ke",
                                        ContactNumber = "+254712345678",
                                        address = new Address
                                        {
                                            City = "Kisumu",
                                            Street = "Oginga Odinga Street",
                                            PostalCode = "40100"
                                        },
                                        Dishes = new List<Dish>
                                        {
                                            new Dish { Name = "Butter Chicken", Description = "Creamy tomato-based chicken curry", Price = "11.99" },
                                            new Dish { Name = "Paneer Tikka", Description = "Grilled cottage cheese cubes", Price = "9.49" }
                                        },
                                        CreatedAt = DateTime.UtcNow
                                    }
                                };

            return restaurants;
        }

    }
}
