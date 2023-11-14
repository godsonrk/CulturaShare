namespace CulturaShare.MongoSidecar.Services.Base;

public interface IDebesiumConnectorService : IAsyncDisposable
{
    Task CreateDebesiumConnectors(string[] tables);
}
