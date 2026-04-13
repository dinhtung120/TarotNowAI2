using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.IntegrationTests.Outbox;

/// <summary>
/// Fixture PostgreSQL container cho integration tests tầng Infrastructure.
/// </summary>
public sealed class InfrastructurePostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("tarotnow_infra_tests")
        .WithUsername("postgres")
        .WithPassword("postgres_test_password")
        .Build();

    /// <summary>
    /// Connection string PostgreSQL của fixture.
    /// </summary>
    public string ConnectionString => _postgreSqlContainer.GetConnectionString();

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        await using var dbContext = CreateDbContext();
        await dbContext.Database.EnsureCreatedAsync();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync().AsTask();
    }

    /// <summary>
    /// Tạo ApplicationDbContext dùng chung connection string của fixture.
    /// </summary>
    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Xóa toàn bộ dữ liệu outbox để cô lập từng test case.
    /// </summary>
    public async Task ResetOutboxAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM outbox_handler_states;");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM outbox_messages;");
    }
}

/// <summary>
/// Collection definition chia sẻ PostgreSQL fixture cho nhóm infra integration tests.
/// </summary>
[CollectionDefinition("InfrastructurePostgres")]
public sealed class InfrastructurePostgresCollection : ICollectionFixture<InfrastructurePostgresFixture>
{
}
