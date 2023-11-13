using CulturaShare.MongoSidecar.Services.DBConsumers;

namespace CulturaShare.MongoSidecar.Helper;

public interface IConsumerFactory
{
    IPostgresConsumer GetPosgresConsumer();
}
