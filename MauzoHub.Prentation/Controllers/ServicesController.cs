using MauzoHub.Application.CQRS.Products.Commands;
using MauzoHub.Application.CQRS.Products.Queries;
using MauzoHub.Application.CQRS.Services.Commands;
using MauzoHub.Application.CQRS.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromForm] CreateServiceCommand command)
        {
            var data = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetServiceById), new { id = data.Id }, data);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var query = new GetServiceByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllServices()
        {
            var query = new GetAllServicesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateService(UpdateServiceCommand command)
        {
            var data = await _mediator.Send(command);

            return Ok(data);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var command = new DeleteServiceCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
