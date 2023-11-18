using Confluent.Kafka;
using CulturalShare.PostRead.Domain.Context;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CulturaShare.MongoSidecar.Helper;

public interface IConsumerFactory
{
    Task CreateConsumerForEntityType(IEntityType type, ConsumerConfig kafkaConfig, Func<PostWriteDBContext> createDbContext, MongoDbContext mongoDbContext);
}
