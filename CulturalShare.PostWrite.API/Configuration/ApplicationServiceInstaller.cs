using CulturalShare.PostWrite.API.Configuration.Base;

namespace CulturalShare.PostWrite.API.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddMvc();
        builder.Services.AddControllers();
        builder.Services.AddGrpc();
    }
}
