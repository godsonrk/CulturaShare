using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Configuration.Base;
using CulturaShare.MongoSidecar.Helper;
using CulturaShare.MongoSidecar.Services;
using CulturaShare.MongoSidecar.Services.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulturaShare.MongoSidecar.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IConfigurationRoot configuration, ServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IApplication, Application.Application>();
        services.AddSingleton<IConsumerFactory, ConsumerFactory>();
    }
}
