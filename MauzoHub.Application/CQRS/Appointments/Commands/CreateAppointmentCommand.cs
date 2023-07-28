using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Appointments.Commands
{
    public class CreateAppointmentCommand : IRequest<GetAppointmentDto>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ServiceId { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        [Required]
        public TimeOnly AppointmentTime { get; set; }
    }
}
