using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
        public partial class AddSubscriptionEntitlement : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entitlement_mapping_rules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    target_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ratio = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entitlement_mapping_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subscription_plans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    price_diamond = table.Column<long>(type: "bigint", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    entitlements_json = table.Column<string>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription_plans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_subscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    plan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price_paid_diamond = table.Column<long>(type: "bigint", nullable: false),
                    idempotency_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_subscriptions_subscription_plans_plan_id",
                        column: x => x.plan_id,
                        principalTable: "subscription_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscription_entitlement_buckets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entitlement_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    daily_quota = table.Column<int>(type: "integer", nullable: false),
                    used_today = table.Column<int>(type: "integer", nullable: false),
                    business_date = table.Column<DateOnly>(type: "date", nullable: false),
                    subscription_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription_entitlement_buckets", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscription_entitlement_buckets_user_subscriptions_user_su~",
                        column: x => x.user_subscription_id,
                        principalTable: "user_subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entitlement_consumes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bucket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entitlement_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    consumed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reference_source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    reference_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    idempotency_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entitlement_consumes", x => x.id);
                    table.ForeignKey(
                        name: "fk_entitlement_consumes_subscription_entitlement_buckets_bucke~",
                        column: x => x.bucket_id,
                        principalTable: "subscription_entitlement_buckets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_entitlement_consumes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_entitlement_consumes_bucket_id",
                table: "entitlement_consumes",
                column: "bucket_id");

            migrationBuilder.CreateIndex(
                name: "ix_entitlement_consumes_idempotency_key",
                table: "entitlement_consumes",
                column: "idempotency_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_entitlement_consumes_user_time",
                table: "entitlement_consumes",
                columns: new[] { "user_id", "consumed_at" });

            migrationBuilder.CreateIndex(
                name: "ix_mapping_rules_source_target",
                table: "entitlement_mapping_rules",
                columns: new[] { "source_key", "target_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_buckets_business_date",
                table: "subscription_entitlement_buckets",
                column: "business_date");

            migrationBuilder.CreateIndex(
                name: "ix_buckets_subscription_id",
                table: "subscription_entitlement_buckets",
                column: "user_subscription_id");

            migrationBuilder.CreateIndex(
                name: "ix_buckets_user_key_date",
                table: "subscription_entitlement_buckets",
                columns: new[] { "user_id", "entitlement_key", "business_date", "subscription_end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_subscription_plans_is_active",
                table: "subscription_plans",
                column: "is_active",
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_idempotency_key",
                table: "user_subscriptions",
                column: "idempotency_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_plan_id",
                table: "user_subscriptions",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_status_end_date",
                table: "user_subscriptions",
                columns: new[] { "status", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_user_id_status",
                table: "user_subscriptions",
                columns: new[] { "user_id", "status" });
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entitlement_consumes");

            migrationBuilder.DropTable(
                name: "entitlement_mapping_rules");

            migrationBuilder.DropTable(
                name: "subscription_entitlement_buckets");

            migrationBuilder.DropTable(
                name: "user_subscriptions");

            migrationBuilder.DropTable(
                name: "subscription_plans");
        }
    }
}
