using MauzoHub.Application.DependencyInjections;
using MauzoHub.Infrastructure.DependencyInjections;
using MauzoHub.Prentation.Middlewares;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// MINE
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header Using Bearer Token",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.ConfigureInfrastructureServices(configuration);
builder.Services.ConfigureApplicationServices(configuration);

builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// MINE
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
