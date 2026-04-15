using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorGachaToPoolRewards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gacha_banner_items");

            migrationBuilder.DropTable(
                name: "gacha_reward_logs");

            migrationBuilder.DropTable(
                name: "gacha_banners");

            migrationBuilder.CreateTable(
                name: "gacha_pools",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    pool_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_zh = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description_vi = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    description_en = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    description_zh = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    cost_currency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    cost_amount = table.Column<long>(type: "bigint", nullable: false),
                    odds_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    pity_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    hard_pity_count = table.Column<int>(type: "integer", nullable: false),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_pools", x => x.id);
                    table.CheckConstraint("ck_gacha_pools_cost_amount_positive", "\"cost_amount\" > 0");
                    table.CheckConstraint("ck_gacha_pools_hard_pity_non_negative", "\"hard_pity_count\" >= 0");
                    table.CheckConstraint("ck_gacha_pools_hard_pity_when_enabled", "(\"pity_enabled\" = false AND \"hard_pity_count\" = 0) OR (\"pity_enabled\" = true AND \"hard_pity_count\" > 0)");
                });

            migrationBuilder.CreateTable(
                name: "gacha_pull_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    idempotency_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    pull_count = table.Column<int>(type: "integer", nullable: false),
                    current_pity_count = table.Column<int>(type: "integer", nullable: false),
                    hard_pity_threshold = table.Column<int>(type: "integer", nullable: false),
                    was_pity_triggered = table.Column<bool>(type: "boolean", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_pull_operations", x => x.id);
                    table.CheckConstraint("ck_gacha_pull_operations_current_pity_non_negative", "\"current_pity_count\" >= 0");
                    table.CheckConstraint("ck_gacha_pull_operations_hard_pity_non_negative", "\"hard_pity_threshold\" >= 0");
                    table.CheckConstraint("ck_gacha_pull_operations_pull_count_positive", "\"pull_count\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "gacha_pull_reward_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pull_operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    reward_rate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reward_kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    item_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    item_definition_id = table.Column<Guid>(type: "uuid", nullable: true),
                    currency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    amount = table.Column<long>(type: "bigint", nullable: true),
                    quantity_granted = table.Column<int>(type: "integer", nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_zh = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_hard_pity_reward = table.Column<bool>(type: "boolean", nullable: false),
                    pity_count_at_reward = table.Column<int>(type: "integer", nullable: false),
                    rng_seed = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_pull_reward_logs", x => x.id);
                    table.CheckConstraint("ck_gacha_pull_reward_logs_kind_currency", "\"reward_kind\" <> 'currency' OR (\"item_definition_id\" IS NULL AND \"item_code\" IS NULL AND \"currency\" IS NOT NULL AND \"amount\" > 0)");
                    table.CheckConstraint("ck_gacha_pull_reward_logs_kind_item", "\"reward_kind\" <> 'item' OR (\"item_definition_id\" IS NOT NULL AND \"item_code\" IS NOT NULL AND \"currency\" IS NULL AND \"amount\" IS NULL)");
                    table.CheckConstraint("ck_gacha_pull_reward_logs_pity_non_negative", "\"pity_count_at_reward\" >= 0");
                    table.CheckConstraint("ck_gacha_pull_reward_logs_quantity_positive", "\"quantity_granted\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "gacha_pool_reward_rates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reward_kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    probability_basis_points = table.Column<int>(type: "integer", nullable: false),
                    item_definition_id = table.Column<Guid>(type: "uuid", nullable: true),
                    currency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    amount = table.Column<long>(type: "bigint", nullable: true),
                    quantity_granted = table.Column<int>(type: "integer", nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_zh = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_pool_reward_rates", x => x.id);
                    table.CheckConstraint("ck_gacha_pool_reward_rates_kind_currency", "\"reward_kind\" <> 'currency' OR (\"item_definition_id\" IS NULL AND \"currency\" IS NOT NULL AND \"amount\" > 0)");
                    table.CheckConstraint("ck_gacha_pool_reward_rates_kind_item", "\"reward_kind\" <> 'item' OR (\"item_definition_id\" IS NOT NULL AND \"currency\" IS NULL AND \"amount\" IS NULL)");
                    table.CheckConstraint("ck_gacha_pool_reward_rates_probability_positive", "\"probability_basis_points\" > 0");
                    table.CheckConstraint("ck_gacha_pool_reward_rates_quantity_positive", "\"quantity_granted\" > 0");
                    table.ForeignKey(
                        name: "fk_gacha_pool_reward_rates_gacha_pools_pool_id",
                        column: x => x.pool_id,
                        principalTable: "gacha_pools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pool_reward_rates_pool_id_is_active",
                table: "gacha_pool_reward_rates",
                columns: new[] { "pool_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pool_reward_rates_pool_id_rarity",
                table: "gacha_pool_reward_rates",
                columns: new[] { "pool_id", "rarity" });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pools_code",
                table: "gacha_pools",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pools_effective_from_effective_to",
                table: "gacha_pools",
                columns: new[] { "effective_from", "effective_to" });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pools_is_active",
                table: "gacha_pools",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pull_operations_user_id_idempotency_key",
                table: "gacha_pull_operations",
                columns: new[] { "user_id", "idempotency_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pull_operations_user_id_pool_id_created_at_utc",
                table: "gacha_pull_operations",
                columns: new[] { "user_id", "pool_id", "created_at_utc" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pull_reward_logs_pull_operation_id",
                table: "gacha_pull_reward_logs",
                column: "pull_operation_id");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pull_reward_logs_user_id_pool_id_created_at_utc",
                table: "gacha_pull_reward_logs",
                columns: new[] { "user_id", "pool_id", "created_at_utc" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_pull_reward_logs_user_id_pool_id_rarity_created_at_utc",
                table: "gacha_pull_reward_logs",
                columns: new[] { "user_id", "pool_id", "rarity", "created_at_utc" },
                descending: new[] { false, false, false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gacha_pool_reward_rates");

            migrationBuilder.DropTable(
                name: "gacha_pull_operations");

            migrationBuilder.DropTable(
                name: "gacha_pull_reward_logs");

            migrationBuilder.DropTable(
                name: "gacha_pools");

            migrationBuilder.CreateTable(
                name: "gacha_banners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    cost_diamond = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description_en = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description_vi = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    hard_pity_count = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    odds_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    pity_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_banners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gacha_reward_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    banner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banner_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    idempotency_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    odds_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    pity_count_at_spin = table.Column<int>(type: "integer", nullable: false),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    rng_seed = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    spent_diamond = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    was_pity_triggered = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_reward_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gacha_banner_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    banner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_icon = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    display_name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    display_name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    weight_basis_points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_banner_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_gacha_banner_items_gacha_banners_banner_id",
                        column: x => x.banner_id,
                        principalTable: "gacha_banners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_banner_items_banner_id",
                table: "gacha_banner_items",
                column: "banner_id");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_banners_code",
                table: "gacha_banners",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gacha_banners_is_active",
                table: "gacha_banners",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_idempotency_key",
                table: "gacha_reward_logs",
                column: "idempotency_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_user_id_banner_id_rarity_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "banner_id", "rarity", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_user_id_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "created_at" },
                descending: new[] { false, true });
        }
    }
}
