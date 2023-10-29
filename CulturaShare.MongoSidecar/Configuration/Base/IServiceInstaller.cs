using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulturaShare.MongoSidecar.Configuration.Base;

public interface IServiceInstaller
{
    void Install(IConfigurationRoot configuration, ServiceCollection services);
}
