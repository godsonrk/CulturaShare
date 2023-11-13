using System.Text;
using CulturaShare.MongoSidecar.Model.Configuration;
using CulturaShare.MongoSidecar.Model.ConnectorModel;
using CulturaShare.MongoSidecar.Model.Exceptions;
using CulturaShare.MongoSidecar.Services.Base;
using Newtonsoft.Json;

namespace CulturaShare.MongoSidecar.Services
{
    public class DebesiumConnectorService : IDebesiumConnectorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DebesiumConfiguration _debesiumConfiguration;

        public DebesiumConnectorService(IHttpClientFactory httpClientFactory, DebesiumConfiguration debesiumConfiguration)
        {
            _httpClientFactory = httpClientFactory;
            _debesiumConfiguration = debesiumConfiguration;
        }

        public async Task CreateDebesiumConnectors(string[] tables)
        {
            foreach (var table in tables)
            {
                try
                {
                    var connector = new DebeziumConnectorConfig
                    {
                        Name = $"source-{table}-connector",
                        Config = new ConnectorConfig
                        {
                            ConnectorClass = "io.debezium.connector.postgresql.PostgresConnector",
                            DatabaseHostname = "postgres",
                            DatabasePort = "5432",
                            DatabaseUser = "docker",
                            DatabasePassword = "docker",
                            DatabaseDbName = "PostWrite",
                            PluginName = "pgoutput",
                            DatabaseServerName = "source",
                            KeyConverterSchemasEnable = "false",
                            ValueConverterSchemasEnable = "false",
                            ValueConverter = "org.apache.kafka.connect.json.JsonConverter",
                            Transforms = "unwrap",
                            TransformsUnwrapType = "io.debezium.transforms.ExtractNewRecordState",
                            KeyConverter = "org.apache.kafka.connect.json.JsonConverter",
                            TableIncludeList = $"public.{table}",
                            SlotName = $"dbz_sales_transaction_slot_{table}",
                            TombstonesOnDelete = "true"
                        }
                    };

                    string jsonString = JsonConvert.SerializeObject(connector, Formatting.Indented);
                    var httpClient = _httpClientFactory.CreateClient();
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{_debesiumConfiguration.Url}/connectors", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ConnectorCreationException($"Failed creating connector for {table}!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
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
    }
}
