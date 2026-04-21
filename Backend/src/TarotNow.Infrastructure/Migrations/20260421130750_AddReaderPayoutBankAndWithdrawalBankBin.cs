using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReaderPayoutBankAndWithdrawalBankBin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bank_bin",
                table: "withdrawal_requests",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payout_bank_account_holder",
                table: "users",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payout_bank_account_number",
                table: "users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payout_bank_bin",
                table: "users",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payout_bank_name",
                table: "users",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bank_bin",
                table: "withdrawal_requests");

            migrationBuilder.DropColumn(
                name: "payout_bank_account_holder",
                table: "users");

            migrationBuilder.DropColumn(
                name: "payout_bank_account_number",
                table: "users");

            migrationBuilder.DropColumn(
                name: "payout_bank_bin",
                table: "users");

            migrationBuilder.DropColumn(
                name: "payout_bank_name",
                table: "users");
        }
    }
}
