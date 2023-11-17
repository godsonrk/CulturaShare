using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CulturalShare.Posts.Data.Extensions;

public static class DbExtension
{
    public static List<T> GetEntities<T>(this DbContext context) where T : class
    {
        return context.Set<T>().ToList();
    }

    public static async Task<List<T>> GetEntitiesAsync<T>(this DbContext context, params Expression<Func<T, object>>[] includes) where T : class
    {
        IQueryable<T> query = context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public static T GetEntityById<T>(this DbContext context, int id) where T : class
    {
        return context.Set<T>().Find(id);
    }

    public static async Task<T> GetEntityByIdAsync<T>(this DbContext context, int id, params Expression<Func<T, object>>[] includes) where T : class
    {
        IQueryable<T> query = context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }
}
