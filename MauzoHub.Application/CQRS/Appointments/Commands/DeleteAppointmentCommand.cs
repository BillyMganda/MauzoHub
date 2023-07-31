using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Appointments.Commands
{
    public class DeleteAppointmentCommand : IRequest<Unit>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
