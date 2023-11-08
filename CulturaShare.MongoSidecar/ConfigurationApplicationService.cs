using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CulturaShare.MongoSidecar.DependencyInjection;
using CulturaShare.MongoSidecar.Configuration.Base;

namespace CulturaShare.MongoSidecar;

public class ConfigurationApplicationService
{
    public ServiceProvider SetupServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var services = new ServiceCollection();
        services.InstallServices(configuration, typeof(IServiceInstaller).Assembly);
        return services.BuildServiceProvider();
    }
}
