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
            migrationBuilder.AddColumn<long>(
                name: "available_balance_after",
                table: "wallet_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "available_balance_before",
                table: "wallet_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "frozen_balance_after",
                table: "wallet_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "frozen_balance_before",
                table: "wallet_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(
                """
                UPDATE wallet_transactions
                SET available_balance_before = balance_before,
                    available_balance_after = balance_after,
                    frozen_balance_before = 0,
                    frozen_balance_after = 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available_balance_after",
                table: "wallet_transactions");

            migrationBuilder.DropColumn(
                name: "available_balance_before",
                table: "wallet_transactions");

            migrationBuilder.DropColumn(
                name: "frozen_balance_after",
                table: "wallet_transactions");

            migrationBuilder.DropColumn(
                name: "frozen_balance_before",
                table: "wallet_transactions");
        }
    }
}
