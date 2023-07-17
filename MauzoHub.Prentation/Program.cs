using MauzoHub.Application.DependencyInjections;
using MauzoHub.Infrastructure.DependencyInjections;
using MauzoHub.Prentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MINE
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.ConfigureInfrastructureServices(configuration);
builder.Services.ConfigureApplicationServices(configuration);

builder.Services.AddHttpContextAccessor();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// MINE
app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
