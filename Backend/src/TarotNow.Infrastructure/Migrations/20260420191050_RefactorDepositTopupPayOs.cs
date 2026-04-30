using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDepositTopupPayOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_deposit_orders_transaction_id",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "fx_snapshot",
                table: "deposit_orders");

            migrationBuilder.RenameColumn(
                name: "bonus_diamond",
                table: "deposit_promotions",
                newName: "bonus_gold");

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "deposit_orders",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "base_diamond_amount",
                table: "deposit_orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "bonus_gold_amount",
                table: "deposit_orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "checkout_url",
                table: "deposit_orders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_request_key",
                table: "deposit_orders",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at_utc",
                table: "deposit_orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "failure_reason",
                table: "deposit_orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "package_code",
                table: "deposit_orders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "payos_order_code",
                table: "deposit_orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "payos_payment_link_id",
                table: "deposit_orders",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "qr_code",
                table: "deposit_orders",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "deposit_orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "wallet_granted_at_utc",
                table: "deposit_orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_client_request_key",
                table: "deposit_orders",
                column: "client_request_key",
                unique: true,
                filter: "\"client_request_key\" <> ''");

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_payos_order_code",
                table: "deposit_orders",
                column: "payos_order_code",
                unique: true,
                filter: "\"payos_order_code\" <> 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_deposit_orders_client_request_key",
                table: "deposit_orders");

            migrationBuilder.DropIndex(
                name: "ix_deposit_orders_payos_order_code",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "base_diamond_amount",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "bonus_gold_amount",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "checkout_url",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "client_request_key",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "expires_at_utc",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "failure_reason",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "package_code",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payos_order_code",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payos_payment_link_id",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "qr_code",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "wallet_granted_at_utc",
                table: "deposit_orders");

            migrationBuilder.RenameColumn(
                name: "bonus_gold",
                table: "deposit_promotions",
                newName: "bonus_diamond");

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "deposit_orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fx_snapshot",
                table: "deposit_orders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_transaction_id",
                table: "deposit_orders",
                column: "transaction_id",
                unique: true);
        }
    }
}
