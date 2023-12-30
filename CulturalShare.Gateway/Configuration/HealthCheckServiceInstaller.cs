using CulturalShare.Gateway.Configuration.Base;

namespace CulturalShare.Gateway.Configuration;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
    }
}
