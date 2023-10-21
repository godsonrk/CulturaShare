using CulturalShare.PostRead.Services.DependencyInjection;
using CulturalShare.PostRead.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddPostsReadServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
