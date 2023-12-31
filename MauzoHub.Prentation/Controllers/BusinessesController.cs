﻿using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CQRS.BusinessCategories.Queries;
using MauzoHub.Application.CQRS.Businesses.Commands;
using MauzoHub.Application.CQRS.Businesses.Queries;
using MauzoHub.Application.CQRS.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BusinessesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] AddBusinessCommand command)
        {
            var businessDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBusinessById), new { id = businessDto.Id }, businessDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetBusinessById(Guid id)
        {
            var query = new GetBusinessByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllBusiness()
        {
            var query = new GetAllBusinessesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBusiness(UpdateBusinessCommand command)
        {
            var business = await _mediator.Send(command);

            return Ok(business);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var command = new DeleteBusinessCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
