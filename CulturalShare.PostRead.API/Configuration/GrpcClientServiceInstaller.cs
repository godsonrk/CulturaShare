using CulturalShare.PostRead.API.Configuration.Base;

namespace CulturalShare.PostRead.API.Configuration;

public class GrpcClientServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
    }
}
