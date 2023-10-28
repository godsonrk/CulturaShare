using CulturalShare.Gateway.Middleware.Extension;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PostsReadProto;
using PostsWriteProto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddGrpcClient<Authentication.Authentication.AuthenticationClient>(options =>
{
    options.Address = new Uri("https://localhost:7140");
});

builder.Services.AddGrpcClient<PostsRead.PostsReadClient>(options =>
{
    options.Address = new Uri("https://localhost:7102");
});

builder.Services.AddGrpcClient<PostsWrite.PostsWriteClient>(options =>
{
    options.Address = new Uri("https://localhost:7143");
});

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
 