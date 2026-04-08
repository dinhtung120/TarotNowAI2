using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence;

public partial class ApplicationDbContext
{
    /// <summary>
    /// Áp dụng toàn bộ cấu hình entity từ assembly và loại trừ entity chỉ dùng Mongo.
    /// Luồng xử lý: load IEntityTypeConfiguration tự động rồi ignore ReadingSession/UserCollection khỏi EF relational.
    /// </summary>
    private static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<ReadingSession>();
        modelBuilder.Ignore<UserCollection>();
        // Hai entity này lưu ở Mongo nên không map vào PostgreSQL.
    }

    /// <summary>
    /// Áp bộ quy tắc đặt tên snake_case cho table/column/key/foreign key/index.
    /// Luồng xử lý: duyệt toàn bộ entity type rồi áp từng quy tắc naming tương ứng.
    /// </summary>
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

    /// <summary>
    /// Áp quy tắc tên bảng snake_case cho entity không phải owned type.
    /// Luồng xử lý: bỏ qua owned entity, lấy table name hiện tại và đổi sang snake_case.
    /// </summary>
    private static void ApplyTableNameConvention(IMutableEntityType entity)
    {
        if (entity.IsOwned())
        {
            // Owned type thường dùng tên cột đã kiểm soát riêng, không đổi tên bảng.
            return;
        }

        var tableName = entity.GetTableName();
        if (tableName != null)
        {
            entity.SetTableName(ToSnakeCase(tableName));
        }
    }

    /// <summary>
    /// Áp quy tắc tên cột snake_case cho các property.
    /// Luồng xử lý: riêng PK của owned entity đặt cứng "id", các cột còn lại đổi sang snake_case khi chưa custom name.
    /// </summary>
    private static void ApplyPropertyNameConvention(IMutableEntityType entity)
    {
        var table = StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema());
        foreach (var property in entity.GetProperties())
        {
            if (entity.IsOwned() && property.IsPrimaryKey())
            {
                property.SetColumnName("id");
                // Chuẩn hóa PK owned type để tránh tên cột dài/khó đọc.
                continue;
            }

            var existingName = property.GetColumnName(table);
            if (string.IsNullOrEmpty(existingName) || existingName == property.Name)
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }

    /// <summary>
    /// Áp quy tắc tên key snake_case.
    /// Luồng xử lý: duyệt toàn bộ key của entity và đổi tên khi có giá trị hiện tại.
    /// </summary>
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

    /// <summary>
    /// Áp quy tắc tên foreign key constraint snake_case.
    /// Luồng xử lý: duyệt các foreign key của entity và đổi constraint name nếu có.
    /// </summary>
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

    /// <summary>
    /// Áp quy tắc tên index snake_case.
    /// Luồng xử lý: duyệt index của entity và cập nhật database name theo chuẩn.
    /// </summary>
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

    /// <summary>
    /// Chuyển chuỗi PascalCase/camelCase sang snake_case.
    /// Luồng xử lý: dùng regex chèn dấu "_" giữa lower/digit và upper, sau đó lower-case toàn bộ.
    /// </summary>
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            // Edge case: giữ nguyên input rỗng để tránh phát sinh giá trị null ngoài ý muốn.
            return input;
        }

        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
