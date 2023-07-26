using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CQRS.BusinessCategories.Queries;
using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CQRS.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BusinessCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
        {
            var categoryDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(UpdateBusinessCategoryCommand command)
        {
            var category = await _mediator.Send(command);

            return Ok(category);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var command = new DeleteBusinessCategoryCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
