using MauzoHub.Application.CQRS.Appointments.Queries;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Appointments.Handlers
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, GetAppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentsRepository = appointmentsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            if (request.Id == Guid.Empty)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "400",
                    ErrorMessage = "Bad request",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("An error occurred while processing the command, Invalid request Id: {@ErrorLog}", errorLog);
                throw new BadRequestException("Invalid request Id");
            }

            try
            {
                var appointment = await _appointmentsRepository.GetByIdAsync(request.Id);

                if (appointment == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"appointment with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("appointment with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"appointment with id {request.Id} not found");
                }

                var appaintmentDto = new GetAppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.UserId,
                    ServiceId = appointment.ServiceId,
                    AppointmentDateTime = appointment.AppointmentDateTime,                    
                };

                return appaintmentDto;
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
