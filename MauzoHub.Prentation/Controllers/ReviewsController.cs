using MauzoHub.Application.CQRS.Reviews.Commands;
using MauzoHub.Application.CQRS.Reviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            var reviewDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReviewById), new { id = reviewDto.Id }, reviewDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var query = new GetReviewByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateReview(DeactivateReviewCommand command)
        {
            var reviewDto = await _mediator.Send(command);

            return Ok(reviewDto);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> ActivateReview(ActivateReviewCommand command)
        {
            var reviewDto = await _mediator.Send(command);

            return Ok(reviewDto);
        }
    }
}
