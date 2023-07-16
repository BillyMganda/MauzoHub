using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CQRS.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var query = new GetUserByEmailQuery { Email = email };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {            
            var user = await _mediator.Send(command);

            return Ok(user);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("disable")]
        public async Task<IActionResult> DisableUser(DisableUserCommand command)
        {            
            var user = await _mediator.Send(command);

            return Ok(user);
        }
    }
}
