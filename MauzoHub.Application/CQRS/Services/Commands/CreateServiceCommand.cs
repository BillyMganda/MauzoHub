using MauzoHub.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MauzoHub.Application.CQRS.Services.Commands
{
    public class CreateServiceCommand : IRequest<GetServiceDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public Guid BusinessId { get; set; }
    }
}
