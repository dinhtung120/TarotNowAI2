using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.IntegrationTests.Migrations;

/// <summary>
/// Ngăn migration "mồ côi" (có file .cs nhưng thiếu metadata) khiến EF không apply ở production.
/// </summary>
public sealed class MigrationMetadataTests
{
    /// <summary>
    /// Mọi lớp kế thừa Migration phải có MigrationAttribute để EF discover và chạy được.
    /// </summary>
    [Fact]
    public void AllMigrationClasses_ShouldHaveMigrationAttribute()
    {
        var migrationTypes = typeof(ApplicationDbContext).Assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && typeof(Migration).IsAssignableFrom(type))
            .ToArray();

        var missingMetadata = migrationTypes
            .Where(type => type.GetCustomAttribute<MigrationAttribute>() is null)
            .Select(type => type.FullName ?? type.Name)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.True(
            missingMetadata.Length == 0,
            $"Migration classes thiếu [Migration]: {string.Join(", ", missingMetadata)}");
    }

    /// <summary>
    /// Bảo vệ các migration auth quan trọng luôn được EF nhận diện trong assembly.
    /// </summary>
    [Fact]
    public void AuthSessionMigrations_ShouldBeDiscoverable()
    {
        var migrationIds = typeof(ApplicationDbContext).Assembly
            .GetTypes()
            .Select(type => type.GetCustomAttribute<MigrationAttribute>()?.Id)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains("20260414120000_RefactorAuthSessionRotation", migrationIds);
        Assert.Contains("20260414153000_AddUniqueActiveAuthSessionDeviceIndex", migrationIds);
        Assert.Contains("20260415020000_BackfillLegacyRefreshTokenSessions", migrationIds);
    }
}
