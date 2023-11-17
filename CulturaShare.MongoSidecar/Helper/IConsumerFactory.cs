using Confluent.Kafka;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CulturaShare.MongoSidecar.Helper;

public interface IConsumerFactory
{
    Task CreateConsumerForEntityType(IEntityType type, ConsumerConfig kafkaConfig, Func<PostWriteDBContext> CreateDbContext);
}
