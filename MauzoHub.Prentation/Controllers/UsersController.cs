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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        
            try
            {
                var userDto = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
