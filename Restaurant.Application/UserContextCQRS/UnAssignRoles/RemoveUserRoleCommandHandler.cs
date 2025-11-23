using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserContextCQRS.UnAssignRoles
{
    internal class RemoveUserRoleCommandHandler(ILogger<RemoveUserRoleCommandHandler> logger, UserManager<User> userManager, RoleManager<IdentityRole> roleManager):
        IRequestHandler<RemoveUserRoleCommand>
    {
        public async Task Handle(RemoveUserRoleCommand command, CancellationToken cancellationToken) 
        { 
            var user = await userManager.FindByEmailAsync(command.Email);

            if (user == null) 
            { 
                logger.LogError("User with email {Email} not find", command.Email);

                return;          
            }

            var role = await roleManager.FindByNameAsync(command.RoleName);

            if (role == null) 
            { 
                logger.LogError("User with role {Role} not found", command.RoleName);

                return;
            }


            if (!await userManager.IsInRoleAsync(user, command.RoleName)) 
            { 
                logger.LogError("user {Email} is not in role {Role}", command.Email, command.RoleName);

                return;
            }

            var result = await userManager.RemoveFromRoleAsync(user, command.RoleName);

            if (result.Succeeded)
            {
                logger.LogInformation("Successfully removed role {Role} from user {Email}", command.RoleName, command.Email);
            }
            else
            {
                logger.LogError("Failed to remove role {Role} from user {Email}. Errors: {@Errors}",
                    command.RoleName, command.Email, result.Errors);
            }

        }
    }
}
