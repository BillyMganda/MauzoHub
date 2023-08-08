using MauzoHub.Application.CQRS.Checkouts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CheckoutsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ProceedToCheckout([FromBody] ProceedToCheckoutCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
