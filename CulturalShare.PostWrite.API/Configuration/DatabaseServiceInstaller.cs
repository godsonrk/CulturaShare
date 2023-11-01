using CulturalShare.PostWrite.API.Configuration.Base;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace CulturalShare.PostWrite.API.Configuration;

public class DatabaseServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PostWriteDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB")));
    }
}
