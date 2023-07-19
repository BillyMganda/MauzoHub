using MauzoHub.Application.CQRS.Oauth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OauthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand command)
        {
            var Token = await _mediator.Send(command);
            return Ok(Token);  
        }
    }
}
