using Confluent.Kafka;
using CulturaShare.MongoSidecar.Application.Base;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Services.Base;

namespace CulturaShare.MongoSidecar.Application;

public class Application : IApplication
{
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IEnumerable<IDBConsumer> _consumers;
    public Application(KafkaConfiguration kafkaConfiguration, IEnumerable<IDBConsumer> consumers)
    {
        _kafkaConfiguration = kafkaConfiguration;
        _consumers = consumers;
    }

    public async Task RunAsync()
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
}
