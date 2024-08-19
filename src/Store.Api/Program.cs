using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Store.Api.Configurations;
using Store.Api.Middleware;
using Store.Application;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppConections(builder.Configuration).addConfigureControllers(builder.Configuration).addApiServices(builder.Configuration);

var app = builder.Build();

// Configures the routing middleware
app.UseRouting();

// Configure CORS
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
);

// Configure auth
app.UseAuthentication();
app.UseAuthorization();


// Add middlewares
app.UseMiddleware<IdentityMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();


// Configures the endpoinds
app.UseCustomEndpoints();

// Configure the controllers
app.MapControllers();

app.Run();
public partial class Program { }