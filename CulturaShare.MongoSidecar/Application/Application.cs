using Confluent.Kafka;
using CulturalShare.Posts.Data.Extensions;
using CulturalShare.PostWrite.Domain.Context;
using CulturalShare.PostWrite.Domain.Context.Services;
using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Helper;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Services;
using CulturaShare.MongoSidecar.Services.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CulturaShare.MongoSidecar.Application
{
    public class Application : DbService<PostWriteDBContext>, IApplication
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConsumerFactory _consumerFactory;
        private readonly DebesiumConfiguration _debesiumConfiguration;
        public Application(DbContextOptions<PostWriteDBContext> dbContextOptions, KafkaConfiguration kafkaConfiguration, IHttpClientFactory httpClientFactory, IConsumerFactory consumerFactory, DebesiumConfiguration debesiumConfiguration) : base(dbContextOptions)
        {
            _kafkaConfiguration = kafkaConfiguration;
            _httpClientFactory = httpClientFactory;
            _consumerFactory = consumerFactory;
            _debesiumConfiguration = debesiumConfiguration;
        }

        public async Task RunAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var tableTypes = dbContext.Model.GetEntityTypes();
                var tableNames = tableTypes.Select(x => x.GetTableAttributeValue()).ToArray();

                await using (var debesiumConnectorService = new DebesiumConnectorService(_httpClientFactory, _debesiumConfiguration))
                {
                    await debesiumConnectorService.CreateDebesiumConnectors(tableNames);

                    await RunKafkaConsumers(tableTypes.ToArray());
                }
            }
        }
         
        private async Task RunKafkaConsumers(IEntityType[] tables)
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
    }
}
