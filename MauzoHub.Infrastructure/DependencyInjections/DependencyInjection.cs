using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using MauzoHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MauzoHub.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton(provider => new MongoDbContext(connectionString, databaseName));
        }
    }
}
