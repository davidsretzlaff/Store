using Store.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppConections(builder.Configuration).AddUseCases().AddAndConfigureControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
public partial class Program { }