using Confluent.Kafka;

namespace CulturaShare.MongoSidecar.Services.Base;

public interface IDBConsumer
{
    Task Consume(ConsumerConfig config);
}
