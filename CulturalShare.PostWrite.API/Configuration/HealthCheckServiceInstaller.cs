using CulturalShare.PostWrite.API.Configuration.Base;

namespace CulturalShare.PostWrite.API.Configuration;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
           .AddNpgSql(builder.Configuration.GetConnectionString("PostWriteDB"), name: "PostWriteDB");
    }
}
