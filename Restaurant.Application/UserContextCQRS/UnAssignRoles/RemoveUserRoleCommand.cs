using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserContextCQRS.UnAssignRoles
{
    public class RemoveUserRoleCommand: IRequest
    {
        public string Email { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}
