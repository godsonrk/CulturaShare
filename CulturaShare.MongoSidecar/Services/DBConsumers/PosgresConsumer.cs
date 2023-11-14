using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public class PosgresConsumer : IPostgresConsumer
{
    public async Task Consume(ConsumerConfig kafkaConfig, IEntityType table, Func<PostWriteDBContext> CreateDbContext)
    {
        var topic = $"source.public.{table.GetTableAttributeValue()}";
        using (var consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build())
        {
            consumer.Subscribe(topic);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume(cts.Token);
                    var id = GetIdFromMessage(consumeResult.Message.Value);
                    await Console.Out.WriteLineAsync($"Id = {id}");
                }
            }
            catch (OperationCanceledException)
            {
                // Ctrl+C was pressed, application is terminating.
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
            int idValue = (int)jsonObject["Id"];

            return idValue;
        }
        catch
        {
            return 0;
        }
    }
}

