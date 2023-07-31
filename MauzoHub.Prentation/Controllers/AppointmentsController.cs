using MauzoHub.Application.CQRS.Appointments.Commands;
using MauzoHub.Application.CQRS.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var appointmentDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentDto.Id }, appointmentDto);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var query = new GetAppointmentByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var query = new GetAllAppointmentsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // TODO: check this endpoint
        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetAllAppointmentsForUser(Guid userid)
        {
            var query = new GetAppointmentsForUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentCommand command)
        {
            var appointmentDto = await _mediator.Send(command);

            return Ok(appointmentDto);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var command = new DeleteAppointmentCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
