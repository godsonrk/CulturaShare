using CulturalShare.Auth.API.Configuration.Base;

namespace CulturalShare.Auth.API.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }
}
