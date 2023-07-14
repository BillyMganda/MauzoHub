using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using MauzoHub.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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
                options.DatabaseName = configuration.GetSection("MauzoHubDatabase:DatabaseName").Value!;
                options.UsersCollectionName = configuration.GetSection("MauzoHubDatabase:UsersCollectionName").Value!;
                // Set other properties of MauzoHubDatabaseSettings here
            });

            // Redis Injection
            services.AddStackExchangeRedisCache(options =>
            {
                var redisSettings = configuration.GetSection("RedisSettings");
                var connectionString = configuration.GetSection("RedisSettings:ConnectionString").Value!;

                options.Configuration = connectionString;
            });
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisSettings = configuration.GetSection("RedisSettings");
                var connectionString = configuration.GetSection("RedisSettings:ConnectionString").Value!;

                return ConnectionMultiplexer.Connect(connectionString);
            });

            // Services Injection
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRedisCacheProvider, RedisCacheProvider>();
        }
    }
}
