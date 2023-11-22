using Microsoft.EntityFrameworkCore.Metadata;

namespace CulturaShare.MongoSidecar.Services.Base;

public interface IDebesiumConnectorService : IAsyncDisposable
{
    Task CreateDebesiumConnectors(IEnumerable<IEntityType> tables);
}
