using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Appointments.Queries
{
    public class GetAllAppointmentsQuery : IRequest<IEnumerable<GetAppointmentDto>>
    {
    }
}
