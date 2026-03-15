using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingSessionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "diamond_balance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "frozen_diamond_balance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "gold_balance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "status",
                table: "users");

            migrationBuilder.DropColumn(
                name: "total_diamonds_purchased",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "users",
                newName: "user_level");

            migrationBuilder.RenameColumn(
                name: "exp",
                table: "users",
                newName: "user_exp");

            migrationBuilder.AddColumn<long>(
                name: "amount_charged",
                table: "reading_sessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "currency_used",
                table: "reading_sessions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "question",
                table: "reading_sessions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_wallet",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    gold_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    diamond_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    frozen_diamond_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    total_diamonds_purchased = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_wallet", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_wallet_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_wallet");

            migrationBuilder.DropColumn(
                name: "amount_charged",
                table: "reading_sessions");

            migrationBuilder.DropColumn(
                name: "currency_used",
                table: "reading_sessions");

            migrationBuilder.DropColumn(
                name: "question",
                table: "reading_sessions");

            migrationBuilder.RenameColumn(
                name: "user_level",
                table: "users",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "user_exp",
                table: "users",
                newName: "exp");

            migrationBuilder.AddColumn<long>(
                name: "diamond_balance",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "frozen_diamond_balance",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "gold_balance",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "total_diamonds_purchased",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
