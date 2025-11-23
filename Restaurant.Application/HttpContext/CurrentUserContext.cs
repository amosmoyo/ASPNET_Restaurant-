using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.HttpContext
{
    public record CurrentUserContext(string Id, string Email, IEnumerable<string> Roles)
    {
        public bool IsInRole(string Role) => Roles.Contains(Role);
    }
}
