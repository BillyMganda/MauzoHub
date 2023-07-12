using MauzoHub.Application.CQRS.Users.Commands;
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
                return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
            }
            catch (Exception)            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
            }
        }
    }
}
