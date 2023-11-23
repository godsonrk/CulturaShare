using System.Text;
using CulturalShare.MongoSidecar.Model.Configuration;
using CulturalShare.Posts.Data.Extensions;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Model.ConnectorModel;
using CulturaShare.MongoSidecar.Model.Exceptions;
using CulturaShare.MongoSidecar.Services.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

namespace CulturaShare.MongoSidecar.Services;

public class DebesiumConnectorService : IDebesiumConnectorService, IAsyncDisposable
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DebesiumConfiguration _debesiumConfiguration;
    private readonly PostgresConfiguration _postgresConfiguration;

    public DebesiumConnectorService(IHttpClientFactory httpClientFactory, DebesiumConfiguration debesiumConfiguration, PostgresConfiguration postgresConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _debesiumConfiguration = debesiumConfiguration;
        _postgresConfiguration = postgresConfiguration;
    }
    public async Task CreateDebesiumConnectors(IEnumerable<IEntityType> tablesEntityTypes)
    {
        foreach (var tableEntity in tablesEntityTypes)
        {
            try
            {
                var tableName = tableEntity.ClrType.GetTableAttributeValue();
                var connector = new DebeziumConnectorConfig
                {
                    Name = $"source-{tableName}-connector",
                    Config = new ConnectorConfig
                    {
                        ConnectorClass = "io.debezium.connector.postgresql.PostgresConnector",
                        DatabaseHostname = "postgres", // TODO Change when run in docker
                        DatabasePort = _postgresConfiguration.Port.ToString(),
                        DatabaseUser = _postgresConfiguration.Username,
                        DatabasePassword = _postgresConfiguration.Password,
                        DatabaseDbName = _postgresConfiguration.Database,
                        PluginName = "pgoutput",
                        DatabaseServerName = "source",
                        KeyConverterSchemasEnable = "false",
                        ValueConverterSchemasEnable = "false",
                        ValueConverter = "org.apache.kafka.connect.json.JsonConverter",
                        KeyConverter = "org.apache.kafka.connect.json.JsonConverter",
                        TableIncludeList = $"public.{tableName}",
                        SlotName = $"dbz_sales_transaction_slot_{tableName}",
                        TombstonesOnDelete = "true",
                        ColumnBlackList = GetEntityBlacklistedColumns(tableEntity.ClrType)
                    }
                };

                string jsonString = JsonConvert.SerializeObject(connector, Formatting.Indented);
                var httpClient = _httpClientFactory.CreateClient();
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{_debesiumConfiguration.Url}/connectors", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ConnectorCreationException($"Failed creating connector for {tableName}!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private string GetEntityBlacklistedColumns(Type tableType)
    {
        var tableName = tableType.GetTableAttributeValue();
        var tableProperties = tableType.GetProperties().Select(x => x.Name).Where(x => x != "Id").Select(x => $"public.{tableName}.{x}");
        var blackListString = string.Join(",", tableProperties);
        return blackListString.Substring(0, blackListString.Length);
    }

    public async Task DeleteDebesiumConnectors()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_debesiumConfiguration.Url}/connectors");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get connectors. Status code: {response.StatusCode}");
            }

            string json = await response.Content.ReadAsStringAsync();
            string[] connectors = JsonConvert.DeserializeObject<string[]>(json);

            foreach (var connector in connectors)
            {
                var responseDelete = await httpClient.DeleteAsync($"{_debesiumConfiguration.Url}/connectors/{connector}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new ConnectorCreationException($"Failed to delete {connector}!");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async ValueTask DisposeAsync()
    {
       await DeleteDebesiumConnectors();
    }
}
