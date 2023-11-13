using Confluent.Kafka;
using CulturalShare.PostWrite.Domain.Context;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public interface IPostgresConsumer
{
    Task Consume(ConsumerConfig kafkaConfig, Type table, Func<PostWriteDBContext> CreateDbContext);
}
