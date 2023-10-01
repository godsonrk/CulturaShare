using Cultural.Auth.Domain.AuthDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cultural.Auth.Domain.AuthDB.Context;

public class AuthDBContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
    {
        try
        {
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}


