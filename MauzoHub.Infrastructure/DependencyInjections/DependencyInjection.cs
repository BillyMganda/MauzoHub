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
                options.RefreshTokensCollectionName = configuration.GetSection("MauzoHubDatabase:RefreshTokensCollectionName").Value!;
                options.BusinessCategoriesCollectionName = configuration.GetSection("MauzoHubDatabase:BusinessCategoriesCollectionName").Value!;
                options.BusinessCollectionName = configuration.GetSection("MauzoHubDatabase:BusinessCollectionName").Value!;
                options.ProductsCollectionName = configuration.GetSection("MauzoHubDatabase:ProductsCollectionName").Value!;
                options.ServicesCollectionName = configuration.GetSection("MauzoHubDatabase:ServicesCollectionName").Value!;
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
            services.AddScoped<IOauthRepository, OauthRepository>();
            services.AddScoped<IBusinessCategoryRepository, BusinessCategoryRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IServicesRepository, ServicesRepository>();

            // JWT
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o => {
                var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // on production make it true
                    ValidateAudience = false, // on production make it true
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });            

        }
    }
}
