using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CQRS.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
