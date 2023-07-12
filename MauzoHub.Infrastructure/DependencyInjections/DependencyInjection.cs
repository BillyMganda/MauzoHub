using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using MauzoHub.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MauzoHub.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DB Injection
            services.Configure<MauzoHubDatabaseSettings>(options =>
            {
                options.ConnectionString = configuration.GetSection("MauzoHubDatabase:ConnectionString").Value!;
                // Set other properties of MauzoHubDatabaseSettings here
            });

            // Services Injection
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
