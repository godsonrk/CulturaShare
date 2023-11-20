using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
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
                var id = GetIdFromMessage(consumeResult.Message.Value);

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
            catch (Exception ex)
            {
                // Handle the exception as needed
                var errorMessage = ex.Message;
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

        private int GetIdFromMessage(string message)
        {
            try
            {
                // Parse the JSON string using JObject
                var jsonObject = JObject.Parse(message);

                // Get the value of the "Id" field
                var idValue = (int)jsonObject["Id"];

                return idValue;
            }
            catch
            {
                return 0;
            }
        }
    }
}
