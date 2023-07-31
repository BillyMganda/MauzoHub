using MauzoHub.Application.CQRS.Appointments.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Appointments.Handlers
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, GetAppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentsRepository = appointmentsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAppointmentDto> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
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
                var appointmnet = await _appointmentsRepository.GetByIdAsync(request.Id);

                if (appointmnet == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"appointmnet with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("appointmnet with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"appointmnet with id {request.Id} not found");
                }

                appointmnet.AppointmentDateTime = request.AppointmentDateTime;                
                appointmnet.LastModified = DateTime.Now;

                await _appointmentsRepository.UpdateAsync(appointmnet);

                var appointmentDto = new GetAppointmentDto
                {
                    Id = appointmnet.Id,
                    UserId = appointmnet.UserId,
                    ServiceId = appointmnet.ServiceId,                    
                    AppointmentDateTime = appointmnet.AppointmentDateTime,                    
                };

                return appointmentDto;
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
