using Microsoft.EntityFrameworkCore;
using TarotNow.Infrastructure.IntegrationTests.Outbox;
using TarotNow.Infrastructure.Persistence.Repositories;

namespace TarotNow.Infrastructure.IntegrationTests.Reconciliation;

[Collection("InfrastructurePostgres")]
public sealed class AdminRepositoryIntegrationTests
{
    private readonly InfrastructurePostgresFixture _fixture;

    public AdminRepositoryIntegrationTests(InfrastructurePostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetLedgerMismatchesAsync_ShouldRunAgainstCurrentLedgerViewShape()
    {
        await using var dbContext = _fixture.CreateDbContext();
        await dbContext.Database.ExecuteSqlRawAsync("DROP VIEW IF EXISTS v_user_ledger_balance;");
        await dbContext.Database.ExecuteSqlRawAsync(
            """
            CREATE VIEW v_user_ledger_balance AS
            SELECT user_id,
                   currency,
                   balance_after AS ledger_balance,
                   created_at AS last_tx_at,
                   id AS last_tx_id
            FROM (
                SELECT *, ROW_NUMBER() OVER (PARTITION BY user_id, currency ORDER BY created_at DESC) AS rn
                FROM wallet_transactions
            ) t
            WHERE rn = 1;
            """);

        var repository = new AdminRepository(dbContext);
        var mismatches = await repository.GetLedgerMismatchesAsync(CancellationToken.None);

        Assert.NotNull(mismatches);
    }
}
