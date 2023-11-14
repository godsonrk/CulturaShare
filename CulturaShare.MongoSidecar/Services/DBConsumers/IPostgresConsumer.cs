using Confluent.Kafka;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public interface IPostgresConsumer
{
    Task Consume(ConsumerConfig kafkaConfig, IEntityType table, Func<PostWriteDBContext> CreateDbContext);
}
