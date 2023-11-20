using CulturalShare.MongoSidecar.Model.Configuration;
using CulturalShare.Posts.Data.Configuration;
using CulturaShare.MongoSidecar.Configuration.Base;
using CulturaShare.MongoSidecar.Model.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Configuration;
using System.Text.RegularExpressions;

namespace CulturaShare.MongoSidecar.Configuration;

public class ConfigurationServiceInstaller : IServiceInstaller
{
    public void Install(IConfigurationRoot configuration, ServiceCollection services)
    {
        var kafkaConfig = configuration
            .GetSection("KafkaConfiguration")
            .Get<KafkaConfiguration>();
        services.AddSingleton(kafkaConfig);

        var debesiumConfig = configuration
            .GetSection("DebesiumConfiguration")
            .Get<DebesiumConfiguration>();
        services.AddSingleton(debesiumConfig);

        var mongoConfig = configuration
            .GetSection("MongoConfiguration")
            .Get<MongoConfiguration>();
        services.AddSingleton(mongoConfig);

        var postgresConfig = ParsePostgresConnectionString(configuration.GetConnectionString("PostgresDB"));
        services.AddSingleton(postgresConfig);
    }

    public static PostgresConfiguration ParsePostgresConnectionString(string connectionString)
    {
        var configuration = new PostgresConfiguration();

        try
        {
            // Example connection string: "Host=myhost;Port=5432;Database=mydb;Username=myuser;Password=mypassword;"
            var regex = new Regex(@"(?<key>\w+)=(?<value>[^;]+)");
            var matches = regex.Matches(connectionString);

            foreach (Match match in matches)
            {
                var key = match.Groups["key"].Value.ToLower();
                var value = match.Groups["value"].Value;

                switch (key)
                {
                    case "host":
                        configuration.Host = value;
                        break;
                    case "port":
                        configuration.Port = int.Parse(value);
                        break;
                    case "database":
                        configuration.Database = value;
                        break;
                    case "username":
                        configuration.Username = value;
                        break;
                    case "password":
                        configuration.Password = value;
                        break;
                        // Add more cases for other parameters if needed
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception or log the error
            Console.WriteLine($"Error parsing connection string: {ex.Message}");
        }

        return configuration;
    }
}
