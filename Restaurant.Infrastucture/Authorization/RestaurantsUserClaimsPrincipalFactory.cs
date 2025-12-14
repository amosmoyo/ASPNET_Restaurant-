using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Authorization
{
    public class RestaurantsUserClaimsPrincipalFactory: UserClaimsPrincipalFactory<User,IdentityRole>
    {
        public RestaurantsUserClaimsPrincipalFactory(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) :
            base(userManager, roleManager, options)
        { }


        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {

            var identity = await GenerateClaimsAsync(user);

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                identity.AddClaim(new Claim(PolicyAttributes.Nationality, user.Nationality));
            }

            if (user.DateOfBirth.HasValue)
            {
                identity.AddClaim(new Claim(PolicyAttributes.DateOfBirth, user?.DateOfBirth.Value.ToString("yyyy-MM-dd")!));
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
