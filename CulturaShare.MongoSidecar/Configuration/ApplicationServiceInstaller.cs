using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Configuration.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulturaShare.MongoSidecar.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IConfigurationRoot configuration, ServiceCollection services)
    {
        services.AddSingleton<IApplication, Application.Application>();
    }
}
