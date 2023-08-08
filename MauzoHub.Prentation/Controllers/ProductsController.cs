using MauzoHub.Application.CQRS.Businesses.Commands;
using MauzoHub.Application.CQRS.Businesses.Queries;
using MauzoHub.Application.CQRS.Carts.Commands;
using MauzoHub.Application.CQRS.Products.Commands;
using MauzoHub.Application.CQRS.Products.Queries;
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
        public async Task<IActionResult> CreateProduct([FromForm] AddProductCommand command)
        {
            var productDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct(UpdateProductsCommand command)
        {
            var product = await _mediator.Send(command);

            return Ok(product);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var command = new DeleteProductCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
