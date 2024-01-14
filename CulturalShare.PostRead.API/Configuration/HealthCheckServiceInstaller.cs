using CulturalShare.PostRead.API.Configuration.Base;

namespace CulturalShare.PostRead.API.Configuration;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
           .AddNpgSql(builder.Configuration.GetConnectionString("Postgres"), name: "PostgresDB");
    }
}
