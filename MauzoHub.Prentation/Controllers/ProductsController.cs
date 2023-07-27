using MauzoHub.Application.CQRS.Businesses.Commands;
using MauzoHub.Application.CQRS.Businesses.Queries;
using MauzoHub.Application.CQRS.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductCommand command)
        {
            var productDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var query = new GetBusinessByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
