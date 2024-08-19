using Store.Api.Configurations;
using Store.Api.Middleware;
using Store.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppConections(builder.Configuration).addConfigureControllers(builder.Configuration).addApiServices(builder.Configuration);

var app = builder.Build();

app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<IdentityMiddleware>();
app.MapGet("/", () => "Hello World!").AllowAnonymous();
app.MapControllers();
app.Run();
public partial class Program { }