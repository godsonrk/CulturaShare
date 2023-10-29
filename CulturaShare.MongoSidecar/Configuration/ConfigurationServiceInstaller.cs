using CulturaShare.MongoSidecar.Configuration.Base;
using CulturaShare.MongoSidecar.Model.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulturaShare.MongoSidecar.Configuration;

public class ConfigurationServiceInstaller : IServiceInstaller
{
    public void Install(IConfigurationRoot configuration, ServiceCollection services)
    {
        var kafkaConfig = configuration
                    .GetSection("KafkaConfiguration")
                    .Get<KafkaConfiguration>();
        services.AddSingleton(kafkaConfig);
    }
}
