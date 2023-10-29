using Microsoft.EntityFrameworkCore;

namespace CulturaShare.MongoSidecar.Database;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }
}
