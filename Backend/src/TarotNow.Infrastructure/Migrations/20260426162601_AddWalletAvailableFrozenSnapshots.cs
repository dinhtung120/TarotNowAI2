using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletAvailableFrozenSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE wallet_transactions
                    ADD COLUMN IF NOT EXISTS available_balance_before BIGINT;

                ALTER TABLE wallet_transactions
                    ADD COLUMN IF NOT EXISTS available_balance_after BIGINT;

                ALTER TABLE wallet_transactions
                    ADD COLUMN IF NOT EXISTS frozen_balance_before BIGINT;

                ALTER TABLE wallet_transactions
                    ADD COLUMN IF NOT EXISTS frozen_balance_after BIGINT;

                UPDATE wallet_transactions
                SET available_balance_before = COALESCE(available_balance_before, balance_before, 0),
                    available_balance_after = COALESCE(available_balance_after, balance_after, 0),
                    frozen_balance_before = COALESCE(frozen_balance_before, 0),
                    frozen_balance_after = COALESCE(frozen_balance_after, 0);

                ALTER TABLE wallet_transactions
                    ALTER COLUMN available_balance_before TYPE BIGINT USING available_balance_before::BIGINT,
                    ALTER COLUMN available_balance_before SET DEFAULT 0,
                    ALTER COLUMN available_balance_before SET NOT NULL,
                    ALTER COLUMN available_balance_after TYPE BIGINT USING available_balance_after::BIGINT,
                    ALTER COLUMN available_balance_after SET DEFAULT 0,
                    ALTER COLUMN available_balance_after SET NOT NULL,
                    ALTER COLUMN frozen_balance_before TYPE BIGINT USING frozen_balance_before::BIGINT,
                    ALTER COLUMN frozen_balance_before SET DEFAULT 0,
                    ALTER COLUMN frozen_balance_before SET NOT NULL,
                    ALTER COLUMN frozen_balance_after TYPE BIGINT USING frozen_balance_after::BIGINT,
                    ALTER COLUMN frozen_balance_after SET DEFAULT 0,
                    ALTER COLUMN frozen_balance_after SET NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE wallet_transactions DROP COLUMN IF EXISTS available_balance_after;
                ALTER TABLE wallet_transactions DROP COLUMN IF EXISTS available_balance_before;
                ALTER TABLE wallet_transactions DROP COLUMN IF EXISTS frozen_balance_after;
                ALTER TABLE wallet_transactions DROP COLUMN IF EXISTS frozen_balance_before;
                """);
        }
    }
}
