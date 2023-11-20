using Confluent.Kafka;
using CulturalShare.PostWrite.Domain.Context;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public interface IPostgresConsumer
{
    Task Consume<T>(ConsumerConfig kafkaConfig, Func<PostWriteDBContext> createDbContext, IMongoCollection<T> mongoCollection,
           params Expression<Func<T, object>>[] includes) where T : class;
}
