using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using MauzoHub.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MauzoHub.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<MongoDbContext>();
            services.AddSingleton(provider => provider.GetRequiredService<MongoDbContext>().Users);

            //services.Configure<MauzoHubDatabaseSettings>(configuration.GetSection("MauzoHubDatabase"));
        }
    }
}
