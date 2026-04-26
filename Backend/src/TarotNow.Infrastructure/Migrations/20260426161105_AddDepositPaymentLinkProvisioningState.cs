using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositPaymentLinkProvisioningState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "qr_code",
                table: "deposit_orders",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "payos_payment_link_id",
                table: "deposit_orders",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "checkout_url",
                table: "deposit_orders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "payment_link_attempt_count",
                table: "deposit_orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "payment_link_failure_reason",
                table: "deposit_orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_link_last_attempt_at_utc",
                table: "deposit_orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_link_provisioned_at_utc",
                table: "deposit_orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_link_status",
                table: "deposit_orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "provisioning");

            migrationBuilder.Sql(
                """
                UPDATE deposit_orders
                SET payment_link_status = CASE
                        WHEN payos_payment_link_id IS NOT NULL
                         AND checkout_url IS NOT NULL
                         AND qr_code IS NOT NULL
                        THEN 'ready'
                        ELSE 'provisioning'
                    END,
                    payment_link_attempt_count = CASE
                        WHEN payos_payment_link_id IS NOT NULL
                         AND checkout_url IS NOT NULL
                         AND qr_code IS NOT NULL
                        THEN 1
                        ELSE 0
                    END,
                    payment_link_provisioned_at_utc = CASE
                        WHEN payos_payment_link_id IS NOT NULL
                         AND checkout_url IS NOT NULL
                         AND qr_code IS NOT NULL
                        THEN COALESCE(created_at, NOW())
                        ELSE NULL
                    END,
                    payment_link_last_attempt_at_utc = CASE
                        WHEN payos_payment_link_id IS NOT NULL
                         AND checkout_url IS NOT NULL
                         AND qr_code IS NOT NULL
                        THEN COALESCE(created_at, NOW())
                        ELSE NULL
                    END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_link_attempt_count",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payment_link_failure_reason",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payment_link_last_attempt_at_utc",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payment_link_provisioned_at_utc",
                table: "deposit_orders");

            migrationBuilder.DropColumn(
                name: "payment_link_status",
                table: "deposit_orders");

            migrationBuilder.AlterColumn<string>(
                name: "qr_code",
                table: "deposit_orders",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payos_payment_link_id",
                table: "deposit_orders",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "checkout_url",
                table: "deposit_orders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
