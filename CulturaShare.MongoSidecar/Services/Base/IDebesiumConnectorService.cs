namespace CulturaShare.MongoSidecar.Services.Base;

public interface IDebesiumConnectorService
{
    Task CreateDebesiumConnectors(string[] tables);
    Task DeleteDebesiumConnectors();
}
