using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Appointments.Queries
{
    public class GetAppointmentByIdQuery : IRequest<GetAppointmentDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
