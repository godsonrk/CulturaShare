using CulturalShare.Posts.Data.Entities.NpSqlEntities;
using Microsoft.EntityFrameworkCore;

namespace CulturalShare.PostWrite.Domain.Context;

public class PostWriteDBContext : DbContext
{
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<PostEntity> Posts { get; set; }

    public PostWriteDBContext(DbContextOptions<PostWriteDBContext> options) : base(options)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostEntity>().ToTable("posts");
        modelBuilder.Entity<CommentEntity>().ToTable("comments");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }
}
