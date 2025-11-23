using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserContextCQRS
{
    public class UpdateUserCommand: IRequest
    {
        public DateOnly? DateOfBirth {  get; set; }

        public string Nationality { get; set; } = string.Empty;
    }
}
