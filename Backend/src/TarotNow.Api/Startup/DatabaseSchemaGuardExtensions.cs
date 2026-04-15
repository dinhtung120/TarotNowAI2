using System.Data;
using Microsoft.EntityFrameworkCore;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.Startup;

/// <summary>
/// Guard xác thực schema PostgreSQL trước khi app nhận traffic.
/// </summary>
public static class DatabaseSchemaGuardExtensions
{
    private static readonly string[] RequiredTables = ["users", "refresh_tokens", "auth_sessions"];
    private const string MissingTablesQuery = """
        SELECT table_name
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name IN ('users', 'refresh_tokens', 'auth_sessions')
        """;

    /// <summary>
    /// Fail-fast khi còn pending migrations hoặc thiếu bảng quan trọng.
    /// </summary>
    public static async Task EnsureDatabaseSchemaIsCurrentAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue("Database:RequireUpToDateSchema", true))
        {
            return;
        }

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseSchemaGuard");

        await EnsureNoPendingMigrationsAsync(dbContext, logger, cancellationToken);
        await EnsureRequiredTablesExistAsync(dbContext, logger, cancellationToken);
    }

    private static async Task EnsureNoPendingMigrationsAsync(
        ApplicationDbContext dbContext,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToArray();
        if (pendingMigrations.Length == 0)
        {
            return;
        }

        var migrationList = string.Join(", ", pendingMigrations);
        logger.LogCritical("Database schema is outdated. Pending migrations: {PendingMigrations}", migrationList);
        throw new InvalidOperationException($"Database schema is outdated. Pending migrations: {migrationList}");
    }

    private static async Task EnsureRequiredTablesExistAsync(
        ApplicationDbContext dbContext,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var existingTables = await LoadExistingRequiredTablesAsync(dbContext, cancellationToken);
        var missingTables = RequiredTables
            .Where(table => !existingTables.Contains(table, StringComparer.Ordinal))
            .ToArray();
        if (missingTables.Length == 0)
        {
            return;
        }

        var missingList = string.Join(", ", missingTables);
        logger.LogCritical("Database schema is incomplete. Missing required tables: {MissingTables}", missingList);
        throw new InvalidOperationException($"Database schema is incomplete. Missing required tables: {missingList}");
    }

    private static async Task<IReadOnlyCollection<string>> LoadExistingRequiredTablesAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await using var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = MissingTablesQuery;
        var existingTables = new HashSet<string>(StringComparer.Ordinal);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var tableName = reader.GetString(0);
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                existingTables.Add(tableName);
            }
        }

        return existingTables.ToArray();
    }
}
