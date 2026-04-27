using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TarotNow.Infrastructure.Migrations;

namespace TarotNow.Infrastructure.UnitTests.Migrations;

public sealed class AddDepositOrderCodeSequenceMigrationTests
{
    [Fact]
    public void Up_ShouldUseIdempotentSqlAndSafeSetval()
    {
        var migrationBuilder = new MigrationBuilder("Npgsql");
        var migration = new TestableAddDepositOrderCodeSequence();

        migration.RunUp(migrationBuilder);

        var sqlOperations = migrationBuilder.Operations.OfType<SqlOperation>().ToList();
        Assert.Contains(sqlOperations, operation =>
            operation.Sql.Contains("IF NOT EXISTS", StringComparison.OrdinalIgnoreCase)
            && operation.Sql.Contains("deposit_order_code_seq", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(sqlOperations, operation =>
            operation.Sql.Contains("setval", StringComparison.OrdinalIgnoreCase)
            && operation.Sql.Contains("payos_order_code", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Down_ShouldDropSequenceWithIfExistsGuard()
    {
        var migrationBuilder = new MigrationBuilder("Npgsql");
        var migration = new TestableAddDepositOrderCodeSequence();

        migration.RunDown(migrationBuilder);

        var sql = Assert.Single(migrationBuilder.Operations.OfType<SqlOperation>());
        Assert.Contains("DROP SEQUENCE IF EXISTS deposit_order_code_seq", sql.Sql, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class TestableAddDepositOrderCodeSequence : AddDepositOrderCodeSequence
    {
        public void RunUp(MigrationBuilder migrationBuilder) => Up(migrationBuilder);

        public void RunDown(MigrationBuilder migrationBuilder) => Down(migrationBuilder);
    }
}
