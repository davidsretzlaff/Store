using Store.Api.Configurations;
using Store.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppConections(builder.Configuration).addConfigureControllers(builder.Configuration).addApiServices(builder.Configuration);

var app = builder.Build();
app.MapControllers();
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
);
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!");
app.Run();
public partial class Program { }