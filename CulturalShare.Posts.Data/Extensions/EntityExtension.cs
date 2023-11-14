using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CulturalShare.Posts.Data.Extensions;

public static class EntityExtension
{
    public static string GetTableAttributeValue(this IEntityType type)
    {
        try
        {
            var tableAttribute = type.ClrType.GetCustomAttribute<TableAttribute>();
            return tableAttribute != null ? tableAttribute.Name : type.Name;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
