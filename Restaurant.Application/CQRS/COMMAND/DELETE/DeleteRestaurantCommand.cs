using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.CQRS.COMMAND.DELETE
{
    public class DeleteRestaurantCommand: IRequest<int>
    {
        public int Id { get; set; }
    }
}
