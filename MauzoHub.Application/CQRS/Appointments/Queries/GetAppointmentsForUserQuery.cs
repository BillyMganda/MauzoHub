using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Appointments.Queries
{
    public class GetAppointmentsForUserQuery : IRequest<IEnumerable<GetAppointmentDto>>
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
