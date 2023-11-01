using CulturalShare.Posts.Data.Entities.NpSqlEntities;
using CulturalShare.PostWrite.API.Configuration.Base;
using CulturalShare.PostWrite.API.DependencyInjection;
using CulturalShare.PostWrite.API.Services;
using CulturalShare.PostWrite.Domain.Context;

var builder = WebApplication.CreateBuilder(args);
builder.InstallServices(typeof(IServiceInstaller).Assembly);
var app = builder.Build();

// Seed database.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContextDealerPortal = scope.ServiceProvider.GetRequiredService<PostWriteDBContext>();
        if (!dbContextDealerPortal.Posts.Any())
        {
            dbContextDealerPortal.Posts.Add(new PostEntity()
            {
                Caption = "test",
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "AAA",
                Likes = 0,
                Owner_Id = 1,
            });

            dbContextDealerPortal.SaveChanges();
        }
    }
    catch (Exception ex)
    {
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

app.MapControllers();

app.Run();
