using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
        public partial class AddGachaTables : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gacha_banners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description_vi = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description_en = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    cost_diamond = table.Column<long>(type: "bigint", nullable: false),
                    odds_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pity_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    hard_pity_count = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banner_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    odds_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    spent_diamond = table.Column<long>(type: "bigint", nullable: false),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    pity_count_at_spin = table.Column<int>(type: "integer", nullable: false),
                    was_pity_triggered = table.Column<bool>(type: "boolean", nullable: false),
                    rng_seed = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    idempotency_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    reward_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    weight_basis_points = table.Column<int>(type: "integer", nullable: false),
                    display_name_vi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    display_name_en = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    display_icon = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
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
                name: "ix_gacha_reward_logs_user_id_banner_id_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "banner_id", "created_at" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_user_id_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "created_at" },
                descending: new[] { false, true });
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gacha_banner_items");

            migrationBuilder.DropTable(
                name: "gacha_reward_logs");

            migrationBuilder.DropTable(
                name: "gacha_banners");
        }
    }
}
