using Store.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppConections(builder.Configuration).AddUseCases().AddAndConfigureControllers(builder.Configuration); 

var app = builder.Build();
app.MapControllers();
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
);
app.UseAuthentication();
app.UseAuthorization();
app.Run();
public partial class Program { }