using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWeeklyWithdrawalFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests");

            migrationBuilder.RenameColumn(
                name: "business_date_utc",
                table: "withdrawal_requests",
                newName: "business_week_start_utc");

            migrationBuilder.AlterColumn<string>(
                name: "admin_note",
                table: "withdrawal_requests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "process_idempotency_key",
                table: "withdrawal_requests",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "request_idempotency_key",
                table: "withdrawal_requests",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_note",
                table: "withdrawal_requests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_withdrawal_one_per_week",
                table: "withdrawal_requests",
                columns: new[] { "user_id", "business_week_start_utc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_withdrawal_process_idempotency_key",
                table: "withdrawal_requests",
                column: "process_idempotency_key",
                unique: true,
                filter: "process_idempotency_key IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_withdrawal_one_per_week",
                table: "withdrawal_requests");

            migrationBuilder.DropIndex(
                name: "ix_withdrawal_process_idempotency_key",
                table: "withdrawal_requests");

            migrationBuilder.DropColumn(
                name: "process_idempotency_key",
                table: "withdrawal_requests");

            migrationBuilder.DropColumn(
                name: "request_idempotency_key",
                table: "withdrawal_requests");

            migrationBuilder.DropColumn(
                name: "user_note",
                table: "withdrawal_requests");

            migrationBuilder.RenameColumn(
                name: "business_week_start_utc",
                table: "withdrawal_requests",
                newName: "business_date_utc");

            migrationBuilder.AlterColumn<string>(
                name: "admin_note",
                table: "withdrawal_requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests",
                columns: new[] { "user_id", "business_date_utc" },
                unique: true,
                filter: "status in ('pending','approved')");
        }
    }
}
