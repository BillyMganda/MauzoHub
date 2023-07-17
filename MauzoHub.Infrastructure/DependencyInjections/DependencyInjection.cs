using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using MauzoHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

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

            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }
    }
}
