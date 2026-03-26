using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence;

public partial class ApplicationDbContext
{
    private static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<ReadingSession>();
        modelBuilder.Ignore<UserCollection>();
    }

    private static void ApplySnakeCaseConventions(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            ApplyTableNameConvention(entity);
            ApplyPropertyNameConvention(entity);
            ApplyKeyNameConvention(entity);
            ApplyForeignKeyConvention(entity);
            ApplyIndexNameConvention(entity);
        }
    }

    private static void ApplyTableNameConvention(IMutableEntityType entity)
    {
        if (entity.IsOwned())
        {
            return;
        }

        var tableName = entity.GetTableName();
        if (tableName != null)
        {
            entity.SetTableName(ToSnakeCase(tableName));
        }
    }

    private static void ApplyPropertyNameConvention(IMutableEntityType entity)
    {
        var table = StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema());
        foreach (var property in entity.GetProperties())
        {
            if (entity.IsOwned() && property.IsPrimaryKey())
            {
                property.SetColumnName("id");
                continue;
            }

            var existingName = property.GetColumnName(table);
            if (string.IsNullOrEmpty(existingName) || existingName == property.Name)
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }

    private static void ApplyKeyNameConvention(IMutableEntityType entity)
    {
        foreach (var key in entity.GetKeys())
        {
            var keyName = key.GetName();
            if (keyName != null)
            {
                key.SetName(ToSnakeCase(keyName));
            }
        }
    }

    private static void ApplyForeignKeyConvention(IMutableEntityType entity)
    {
        foreach (var foreignKey in entity.GetForeignKeys())
        {
            var constraintName = foreignKey.GetConstraintName();
            if (constraintName != null)
            {
                foreignKey.SetConstraintName(ToSnakeCase(constraintName));
            }
        }
    }

    private static void ApplyIndexNameConvention(IMutableEntityType entity)
    {
        foreach (var index in entity.GetIndexes())
        {
            var databaseName = index.GetDatabaseName();
            if (databaseName != null)
            {
                index.SetDatabaseName(ToSnakeCase(databaseName));
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
