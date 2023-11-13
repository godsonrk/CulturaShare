using CulturalShare.Posts.Data.Entities.NpSqlEntities;
using CulturalShare.PostWrite.API.Configuration.Base;
using CulturalShare.PostWrite.API.DependencyInjection;
using CulturalShare.PostWrite.API.Services;
using CulturalShare.PostWrite.Domain.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.InstallServices(typeof(IServiceInstaller).Assembly);

builder.Services.AddHealthChecks()
           .AddNpgSql(builder.Configuration.GetConnectionString("Postgres"), name: "PostgresDB");

var app = builder.Build();

// Seed database.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContextDealerPortal = scope.ServiceProvider.GetRequiredService<PostWriteDBContext>();
        for (int i = 0; i < 10000; i++)
        {
            dbContextDealerPortal.Posts.Add(new PostEntity()
            {
                Caption = "test",
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "AAA",
                Likes = 0,
                OwnerId = 1,
                Comments = new List<CommentEntity>()
                {
                    new()
                    {
                        OwnerId = 1,
                        Text = "test",
                        Timestamp = DateTime.UtcNow,
                        Username = "test",
                    }
                }
            });

            dbContextDealerPortal.SaveChanges();

            Thread.Sleep(1000);
        }        
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw ex;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGrpcService<PostsWriteService>();

app.UseAuthorization();

app.MapHealthChecks("/_health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
