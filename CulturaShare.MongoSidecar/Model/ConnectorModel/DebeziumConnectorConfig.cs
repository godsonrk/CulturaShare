using Newtonsoft.Json;

namespace CulturaShare.MongoSidecar.Model.ConnectorModel;

public class DebeziumConnectorConfig
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("config")]
    public ConnectorConfig Config { get; set; }
}

public class ConnectorConfig
{
    [JsonProperty("connector.class")]
    public string ConnectorClass { get; set; }

    [JsonProperty("database.hostname")]
    public string DatabaseHostname { get; set; }

    [JsonProperty("database.port")]
    public string DatabasePort { get; set; }

    [JsonProperty("database.user")]
    public string DatabaseUser { get; set; }

    [JsonProperty("database.password")]
    public string DatabasePassword { get; set; }

    [JsonProperty("database.dbname")]
    public string DatabaseDbName { get; set; }

    [JsonProperty("plugin.name")]
    public string PluginName { get; set; }

    [JsonProperty("database.server.name")]
    public string DatabaseServerName { get; set; }

    [JsonProperty("key.converter.schemas.enable")]
    public string KeyConverterSchemasEnable { get; set; }

    [JsonProperty("value.converter.schemas.enable")]
    public string ValueConverterSchemasEnable { get; set; }

    [JsonProperty("value.converter")]
    public string ValueConverter { get; set; }

    [JsonProperty("key.converter")]
    public string KeyConverter { get; set; }

    [JsonProperty("table.include.list")]
    public string TableIncludeList { get; set; }

    [JsonProperty("slot.name")]
    public string SlotName { get; set; }

    [JsonProperty("tombstones.on.delete")]
    public string TombstonesOnDelete { get; set; }

    [JsonProperty("column.blacklist")]
    public string ColumnBlackList { get; set; }
}
