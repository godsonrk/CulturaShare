using Confluent.Kafka;
using CulturalShare.PostWrite.Domain.Context;
using CulturalShare.PostWrite.Domain.Context.Services;
using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Services.Base;
using CulturaShare.MongoSidecar.Services.DBConsumers.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CulturaShare.MongoSidecar.Application
{
    public class Application : DbService<PostWriteDBContext>, IApplication
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly IEnumerable<IDBConsumer> _consumers;
        private readonly IDebesiumConnectorService _debesiumConnectorService;

        public Application(DbContextOptions<PostWriteDBContext> dbContextOptions, KafkaConfiguration kafkaConfiguration, IEnumerable<IDBConsumer> consumers, IDebesiumConnectorService debesiumConnectorService) : base(dbContextOptions)
        {
            _kafkaConfiguration = kafkaConfiguration;
            _consumers = consumers;
            _debesiumConnectorService = debesiumConnectorService;
        }

        public async Task RunAsync()
        {
            try
            {
                await CreateDebesiumConnectors();
                await RunKafkaConsumers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                await _debesiumConnectorService.DeleteDebesiumConnectors();
            }
        }

        private async Task CreateDebesiumConnectors()
        {
            using (var dbContext = CreateDbContext())
            {
                var tableNames = GetTableNamesFromDbContext(dbContext);
                await _debesiumConnectorService.CreateDebesiumConnectors(tableNames);
            }
        }

        private async Task RunKafkaConsumers()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfiguration.Url,
                GroupId = _kafkaConfiguration.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumersTasks = _consumers.Select(x => x.Consume(config)).ToList();
            await Task.WhenAll(consumersTasks);
        }

        private string[] GetTableNamesFromDbContext(PostWriteDBContext dbContext)
        {
            var tables = dbContext.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToArray();

            var tableNames = new List<string>();
            foreach (var table in tables)
            {
                var entityType = table.PropertyType.GenericTypeArguments[0];
                var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();

                string tableName = tableAttribute != null ? tableAttribute.Name : table.Name;
                tableNames.Add(tableName);
            }

            return tableNames.ToArray();
        }
    }
}
