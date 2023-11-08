using Confluent.Kafka;

namespace CulturaShare.MongoSidecar.Services.DBConsumers.Base;

public interface IDBConsumer
{
    Task Consume(ConsumerConfig config);
}
