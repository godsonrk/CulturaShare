using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CulturalShare.Posts.Data.Extensions;

public static class EntityExtension
{
    public static string GetTableAttributeValue(this Type type)
    {
        var entityType = type.GenericTypeArguments[0];
        var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();

        return tableAttribute != null ? tableAttribute.Name : type.Name;
    }
}
