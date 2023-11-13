using CulturaShare.MongoSidecar.Services.DBConsumers;

namespace CulturaShare.MongoSidecar.Helper;

public class ConsumerFactory : IConsumerFactory
{
    public IPostgresConsumer GetPosgresConsumer()
    {
        return new PosgresConsumer();
    }
}
