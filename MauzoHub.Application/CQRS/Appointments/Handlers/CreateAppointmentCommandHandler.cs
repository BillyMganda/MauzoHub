using MauzoHub.Application.CQRS.Appointments.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Appointments.Handlers
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, GetAppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentsRepository, IHttpContextAccessor contextAccessor)
        {
            _appointmentsRepository = appointmentsRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<GetAppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var appointment = new Domain.Entities.Appointments
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                    UserId = request.UserId,
                    ServiceId = request.ServiceId,
                    AppointmentDateTime = request.AppointmentDateTime,                    
                };

                await _appointmentsRepository.AddAsync(appointment);

                //TODO: use this 'appointment' as a massage in azure service bus

                var apointmentDto = new GetAppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.UserId,
                    ServiceId = appointment.ServiceId,
                    AppointmentDateTime = appointment.AppointmentDateTime,                    
                };

                return apointmentDto;
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
