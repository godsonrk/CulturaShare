using Confluent.Kafka;
using CulturalShare.PostRead.Domain.Context;
using CulturalShare.Posts.Data.Entities.NpSqlEntities;
using CulturalShare.PostWrite.Domain.Context;
using CulturaShare.MongoSidecar.Services.DBConsumers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CulturaShare.MongoSidecar.Helper;

public class ConsumerFactory : IConsumerFactory
{
    public Task CreateConsumerForEntityType(IEntityType type, ConsumerConfig kafkaConfig, Func<PostWriteDBContext> createDbContext, MongoDbContext mongoDbContext ) =>
    type switch
    {
        var t when t.ClrType == typeof(PostEntity) => 
        new PosgresConsumer().Consume(kafkaConfig, createDbContext, mongoDbContext.GetCollection<PostEntity>(), x => x.Comments),
        var t when t.ClrType == typeof(CommentEntity) => 
        new PosgresConsumer().Consume(kafkaConfig, createDbContext, mongoDbContext.GetCollection<CommentEntity>(), x => x.Post),
        _ => throw new NotSupportedException($"Type {type.Name} is not supported."),
    };
}
