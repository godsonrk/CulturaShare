using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using CulturalShare.PostWrite.Domain.Context.Services;
using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Helper;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Services.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CulturaShare.MongoSidecar.Application
{
    public class Application : DbService<PostWriteDBContext>, IApplication
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly IDebesiumConnectorService _debesiumConnectorService;
        private readonly IConsumerFactory _consumerFactory;
        public Application(DbContextOptions<PostWriteDBContext> dbContextOptions, KafkaConfiguration kafkaConfiguration, 
            IDebesiumConnectorService debesiumConnectorService, IConsumerFactory consumerFactory) : base(dbContextOptions)
        {
            _kafkaConfiguration = kafkaConfiguration;
            _debesiumConnectorService = debesiumConnectorService;
            _consumerFactory = consumerFactory;
        }

        public async Task RunAsync()
        {
            try
            {
                using (var dbContext = CreateDbContext())
                {
                    var tableTypes = GetTableTypesFromDbContext(dbContext);
                    await CreateDebesiumConnectors(tableTypes);
                    await RunKafkaConsumers(tableTypes);
                }
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

        private async Task CreateDebesiumConnectors(Type[] tables)
        {
            var tableNames = new List<string>();
            foreach (var table in tables)
                tableNames.Add(table.GetTableAttributeValue());

            await _debesiumConnectorService.CreateDebesiumConnectors(tableNames.ToArray());
        }

        private async Task RunKafkaConsumers(Type[] tables)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfiguration.Url,
                GroupId = _kafkaConfiguration.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var cunsumers = tables.Select(x => _consumerFactory.GetPosgresConsumer().Consume(config, x, CreateDbContext));
            await Task.WhenAll(cunsumers);
        }

        private Type[] GetTableTypesFromDbContext(PostWriteDBContext dbContext)
        {
            var tables = dbContext.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToArray();

            return tables.Select(x => x.PropertyType).ToArray();
        }
    }
}
