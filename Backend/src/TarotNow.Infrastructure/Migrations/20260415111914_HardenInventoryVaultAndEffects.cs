using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HardenInventoryVaultAndEffects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "free_draw_credits",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    available_count = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_free_draw_credits", x => x.id);
                    table.CheckConstraint("ck_free_draw_credits_available_count", "\"available_count\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "inventory_luck_effects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    luck_value = table.Column<int>(type: "integer", nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    source_item_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory_luck_effects", x => x.id);
                    table.CheckConstraint("ck_inventory_luck_effects_luck_value", "\"luck_value\" > 0");
                });

            migrationBuilder.AddCheckConstraint(
                name: "ck_user_items_quantity_non_negative",
                table: "user_items",
                sql: "\"quantity\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_item_definitions_consumable_permanent_xor",
                table: "item_definitions",
                sql: "\"is_consumable\" <> \"is_permanent\"");

            migrationBuilder.AddCheckConstraint(
                name: "ck_item_definitions_effect_value_positive",
                table: "item_definitions",
                sql: "\"effect_value\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_item_definitions_success_rate_percent",
                table: "item_definitions",
                sql: "\"success_rate_percent\" >= 0 AND \"success_rate_percent\" <= 100");

            migrationBuilder.CreateIndex(
                name: "ix_free_draw_credits_user_id",
                table: "free_draw_credits",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_inventory_luck_effects_expires_at_utc",
                table: "inventory_luck_effects",
                column: "expires_at_utc");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_luck_effects_user_id",
                table: "inventory_luck_effects",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "free_draw_credits");

            migrationBuilder.DropTable(
                name: "inventory_luck_effects");

            migrationBuilder.DropCheckConstraint(
                name: "ck_user_items_quantity_non_negative",
                table: "user_items");

            migrationBuilder.DropCheckConstraint(
                name: "ck_item_definitions_consumable_permanent_xor",
                table: "item_definitions");

            migrationBuilder.DropCheckConstraint(
                name: "ck_item_definitions_effect_value_positive",
                table: "item_definitions");

            migrationBuilder.DropCheckConstraint(
                name: "ck_item_definitions_success_rate_percent",
                table: "item_definitions");
        }
    }
}
