using Confluent.Kafka;
using CulturalShare.MongoSidecar.Model;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace CulturaShare.MongoSidecar.Services.DBConsumers
{
    public class PosgresConsumer : IPostgresConsumer
    {
        public async Task Consume<T>(ConsumerConfig kafkaConfig, Func<PostWriteDBContext> createDbContext, IMongoCollection<T> mongoCollection,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            var entityType = typeof(T);
            var topic = $"source.public.{entityType.GetTableAttributeValue()}";

            using (var consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build())
            {
                consumer.Subscribe(topic);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

                try
                {
                    while (true)
                    {
                        using (var context = createDbContext())
                        {
                            await ProcessMessage(mongoCollection, includes, consumer, cts, context);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl + C was pressed, application is terminating.
                }
                catch (Exception ex)
                {
                    // Handle specific exceptions or log the error
                    throw;
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        private async Task ProcessMessage<T>(
            IMongoCollection<T> mongoCollection,
            Expression<Func<T, object>>[] includes,
            IConsumer<Ignore, string> consumer,
            CancellationTokenSource cts,
            PostWriteDBContext context) where T : class
        {
            try
            {
                var consumeResult = consumer.Consume(cts.Token);
                var model = GetModelFromMessage(consumeResult.Message.Value);

                if(model.After != null)
                {
                    await CreateOrUpdateEntity(model.After.Id, mongoCollection, includes, context);
                }
                else if(model.Before != null)
                {
                    await DeleteEntity(model.Before.Id, mongoCollection);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                var errorMessage = ex.Message;
            }
        }

        private async Task DeleteEntity<T>(int id, IMongoCollection<T> mongoCollection) where T : class
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await mongoCollection.DeleteOneAsync(filter);
        }

        private async Task CreateOrUpdateEntity<T>(int id, IMongoCollection<T> mongoCollection, Expression<Func<T, object>>[] includes, PostWriteDBContext context) where T: class
        {
            // Check if document with the given ID already exists
            var existingDocument = await mongoCollection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
            var entity = await context.GetEntityByIdAsync(id, includes);
            var mongoEntity = GetMongoEntity(entity);
            if (existingDocument != null)
            {
                var updateResult = await mongoCollection.ReplaceOneAsync(
                    Builders<T>.Filter.Eq("_id", id),
                    mongoEntity,
                    new ReplaceOptions { IsUpsert = false });
            }
            else
            {
                await mongoCollection.InsertOneAsync(mongoEntity);
            }
        }

        private T GetMongoEntity<T>(T entity)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            var json = JsonConvert.SerializeObject(entity, serializerSettings);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private ChangeEvent GetModelFromMessage(string message)
        {
            try
            {
                var changeEvent = JsonConvert.DeserializeObject<ChangeEvent>(message);
                return changeEvent;
            }
            catch
            {
                return new ChangeEvent();
            }
        }
    }
}
