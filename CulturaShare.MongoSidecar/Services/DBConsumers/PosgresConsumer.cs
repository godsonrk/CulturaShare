using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CulturaShare.MongoSidecar.Services.DBConsumers;

public class PosgresConsumer : IPostgresConsumer
{
    public async Task Consume<T> (ConsumerConfig kafkaConfig, Func<PostWriteDBContext> CreateDbContext, params Expression<Func<T, object>>[] includes) where T : class
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

    public async Task Consume(ConsumerConfig kafkaConfig, IEntityType entityType, Func<PostWriteDBContext> CreateDbContext)
    {
        using (var context = CreateDbContext())
        {
            var methods = context.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var a = methods.Where(x => x.Name.Contains("get"));
            // Get the GetEntities method using reflection
            MethodInfo method = methods
                .FirstOrDefault(m => m.Name == "get_People" && m.GetParameters().Length == 0 && m.ReturnType.IsGenericType);
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

