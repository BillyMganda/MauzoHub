using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MauzoHub.Application.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<IMediator, Mediator>();
        }
    }
}
