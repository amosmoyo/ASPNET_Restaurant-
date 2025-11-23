using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserContextCQRS.AssignRoles
{
    public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        : AssignUserRoleCommand
    {
        public async Task Handle(AssignUserRoleCommandHandler command, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(command.Email);

            if (user == null) 
            {
                logger.LogError("User with email {Email} not found.", command.Email);
                return;
            }

            var role = await roleManager.FindByNameAsync(command.RoleName);

            if (role == null) 
            {
                logger.LogInformation("Role {Role} not found.", command.RoleName);

                return;
            }

            var result = await userManager.AddToRoleAsync(user, role.Name!);

            if (result.Succeeded)
            {
                logger.LogInformation("Successfully assigned role {Role} to user {Email}", command.RoleName, command.Email);
                return;
            }
            {
                logger.LogError("Failed to assign role {Role} to user {Email}. Errors: {@Errors}",
                    command.RoleName, command.Email, result.Errors);
                return;
            }


        }
    }
}
