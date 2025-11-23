using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.HttpContext
{
    public interface IUserContext
    {
        CurrentUserContext? GetCurrentUserContext();
    }
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public CurrentUserContext? GetCurrentUserContext()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            if (user == null) return null;

            if(user.Identity == null) return null;

            if (!user.Identity.IsAuthenticated) return null;

            var Id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var Email = user.FindFirstValue(ClaimTypes.Email);

            var Role = user.FindAll(ClaimTypes.Role).Select(r => r.Value);

            return new CurrentUserContext(Id ?? string.Empty, Email ?? string.Empty, Role);
        }
    }
}
