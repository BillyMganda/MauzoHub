using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace MauzoHub.Application.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {            
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<IMediator, Mediator>();

            // Serilog
            string connectionString = configuration.GetSection("MauzoHubDatabase:ConnectionString").Value!;
            string databaseName = configuration.GetSection("MauzoHubDatabase:DatabaseName").Value!;
            string collectionName = configuration.GetSection("MauzoHubDatabase:LogsCollectionName").Value!;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.MongoDB(connectionString +"/"+ databaseName, collectionName)
                .CreateLogger();            
        }
    }
}
