using MauzoHub.Application.CQRS.Appointments.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Appointments.Handlers
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<GetAppointmentDto>>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentsRepository = appointmentsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetAppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var appointments = await _appointmentsRepository.GetAllAsync();

                var appointmentsDto = appointments.Select(a => new GetAppointmentDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    ServiceId = a.ServiceId,
                    AppointmentDateTime = a.AppointmentDateTime,                   
                });

                return appointmentsDto;
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "500",
                    ErrorMessage = ex.Message,
                    IPAddress = remoteIpAddress.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };
                Log.Error(ex, "An error occurred while processing the command: {@ErrorLog}", errorLog);

                throw;
            }
        }
    }
}
