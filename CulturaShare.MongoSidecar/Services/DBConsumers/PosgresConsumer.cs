using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public class PosgresConsumer : IPostgresConsumer
{
    public async Task Consume<T> (ConsumerConfig kafkaConfig, Func<PostWriteDBContext> CreateDbContext, IMongoCollection<T> mongoCollection,
        params Expression<Func<T, object>>[] includes) where T : class
    {

        var entityType = typeof(T);
        var topic = $"source.public.{entityType.GetTableAttributeValue()}";
        using (var consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build())
        {
            consumer.Subscribe(topic);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

            try
            {
                while (true)
                {
                    using (var context = CreateDbContext())
                    {
                        var consumeResult = consumer.Consume(cts.Token);
                        var id = GetIdFromMessage(consumeResult.Message.Value);

                        var entity = await context.GetEntityByIdAsync(id, includes);

                        var serializerSettings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        };
                        // Convert entity to JSON
                        string json = JsonConvert.SerializeObject(entity, serializerSettings);

                        // Insert JSON document into MongoDB
                        var entityFromJson = JsonConvert.DeserializeObject<T>(json);
                        await mongoCollection.InsertOneAsync(entityFromJson);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                //Ctrl + C was pressed, application is terminating.
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                consumer.Close();
            }
        }
    }

    private int GetIdFromMessage(string message)
    {
        try
        {
            // Parse the JSON string using JObject
            JObject jsonObject = JObject.Parse(message);

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

