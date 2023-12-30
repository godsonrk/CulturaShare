using CulturalShare.Gateway.Configuration.Base;

namespace CulturalShare.Gateway.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
    }
}
