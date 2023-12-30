using CulturalShare.Gateway.Configuration.Base;
using CulturalShare.Gateway.DependencyInjection;
using CulturalShare.Gateway.Middleware.Extension;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.InstallServices(typeof(IServiceInstaller).Assembly);

var app = builder.Build();

app.UseExceptionsHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecureHeaders();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/_health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
 