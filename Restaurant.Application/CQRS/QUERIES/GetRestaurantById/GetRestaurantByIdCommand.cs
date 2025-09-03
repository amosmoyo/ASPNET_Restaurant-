using MediatR;
using Restaurant.Application.CQRS.QUERIES.GetAllRestaurants;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.QUERIES.GetRestaurantById
{

    public class GetRestaurantByIdCommand: IRequest<Restaurants?>
    {
        public int Id { get; set; }
        //public string Name { get; set; } = default!;
        //public string Description { get; set; } = default!;
        //public string Category { get; set; } = default!;
        //public bool HasDelivery { get; set; }
        //public string? ContactEmail { get; set; }
        //public string? ContactNumber { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public string? City { get; set; }
        //public string? Street { get; set; }
        //public string? PostalCode { get; set; }

        //public List<GetAllDishedCommand>? Dishes { get; set; } = new List<GetAllDishedCommand>();

        //public static GetRestaurantByIdCommand FromEntity(Restaurants restaurants)
        //{
        //    if (restaurants == null) return null;

        //    return new GetRestaurantByIdCommand
        //    {
        //        Id = restaurants.Id,
        //        Name = restaurants.Name,
        //        Description = restaurants.Description,
        //        Category = restaurants.Category,
        //        HasDelivery = restaurants.HasDelivery,
        //        ContactEmail = restaurants.ContactEmail,
        //        ContactNumber = restaurants.ContactNumber,
        //        CreatedAt = restaurants.CreatedAt,
        //        City = restaurants?.address?.City,
        //        Street = restaurants?.address?.Street,
        //        PostalCode = restaurants?.address?.PostalCode,
        //        Dishes = restaurants?.Dishes?.Select(GetAllDishedCommand.FromEntity).ToList() ?? []
        //    };
        //}
    }


}
