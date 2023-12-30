using CulturalShare.PostRead.API.Configuration.Base;
using CulturalShare.PostRead.API.DependencyInjection;
using CulturalShare.PostRead.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.InstallServices(typeof(IServiceInstaller).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGrpcService<PostsReadService>();

app.UseAuthorization();

app.MapControllers();

app.Run();
