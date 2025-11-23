using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Constants;
using Restaurant.Application.Services;
using Restaurant.Application.UserContextCQRS;
using Restaurant.Application.UserContextCQRS.AssignRoles;
using Restaurant.Application.UserContextCQRS.UnAssignRoles;

namespace Restaurant.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [HttpPatch]
        [Route("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> updateUser([FromBody] UpdateUserCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }


        [HttpPost]
        [Route("assign/role")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> assignrole([FromBody] AssignUserRoleCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("assign/role")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deleterole([FromBody] RemoveUserRoleCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
