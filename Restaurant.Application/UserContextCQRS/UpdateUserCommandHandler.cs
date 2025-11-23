using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Application.HttpContext;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserContextCQRS
{
    public class UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IUserContext userContext, IUserStore<User> user): IRequestHandler<UpdateUserCommand>
    {
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var loginedUser = userContext.GetCurrentUserContext();

            if (loginedUser == null) 
            {
                logger.LogWarning("No loginuser found");
                return;
            }

            logger.LogInformation("My log in userId {userId}, {@user}",loginedUser?.Id, loginedUser);

            var dbUser = await user.FindByIdAsync(loginedUser?.Id, cancellationToken);

            if (dbUser == null) 
            {
                logger.LogWarning("User with Id {userId} notfound", loginedUser?.Id);
            }

            dbUser.DateOfBirth = request.DateOfBirth;

            dbUser.Nationality = request.Nationality;

            var result = await user.UpdateAsync(dbUser, cancellationToken);

            if (!result.Succeeded)
            {
                logger.LogError("Error updating user information");
            }
            else
            {
                logger.LogInformation("Updated the user information successfully");
            }
        }
    }
}
