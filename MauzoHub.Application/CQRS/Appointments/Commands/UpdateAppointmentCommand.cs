using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Appointments.Commands
{
    public class UpdateAppointmentCommand : IRequest<GetAppointmentDto>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime AppointmentDateTime { get; set; }        
    }
}
