using CulturalShare.Auth.API.Configuration.Base;
using CulturalShare.Auth.API.DependencyInjection;
using CulturalShare.Auth.Services;

var builder = WebApplication.CreateBuilder(args);
builder.InstallServices(typeof(IServiceInstaller).Assembly);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGrpcService<AuthenticationService>();

app.UseAuthorization();

app.MapControllers();

app.Run();
