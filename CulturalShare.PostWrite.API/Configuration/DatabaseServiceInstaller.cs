using CulturalShare.PostWrite.API.Configuration.Base;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace CulturalShare.PostWrite.API.Configuration;

public class DatabaseServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        var docker = builder.Configuration["DOTNET_RUNNING_IN_CONTAINER"];

        if (docker != null && docker.ToLower() == "true")
        {
            var connectionString = builder.Configuration.GetConnectionString("PostgresDBDocker");

            Console.WriteLine(connectionString);
            builder.Services.AddDbContext<PostWriteDBContext>(options => options.UseNpgsql(connectionString));
        }
        else
        {
            Console.WriteLine(builder.Configuration.GetConnectionString("PostWriteDB"));
            builder.Services.AddDbContext<PostWriteDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostWriteDB")));
        }   
    }
}
