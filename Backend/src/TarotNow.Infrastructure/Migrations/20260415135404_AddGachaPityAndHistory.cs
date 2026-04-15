using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGachaPityAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gacha_history_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pull_operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    pull_count = table.Column<int>(type: "integer", nullable: false),
                    pity_before = table.Column<int>(type: "integer", nullable: false),
                    pity_after = table.Column<int>(type: "integer", nullable: false),
                    was_pity_reset = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gacha_history_entries", x => x.id);
                    table.CheckConstraint("ck_gacha_history_entries_pity_after_non_negative", "\"pity_after\" >= 0");
                    table.CheckConstraint("ck_gacha_history_entries_pity_before_non_negative", "\"pity_before\" >= 0");
                    table.CheckConstraint("ck_gacha_history_entries_pull_count_positive", "\"pull_count\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "user_gacha_pities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pull_count = table.Column<int>(type: "integer", nullable: false),
                    last_pity_reset_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_gacha_pities", x => x.id);
                    table.CheckConstraint("ck_user_gacha_pities_pull_count_non_negative", "\"pull_count\" >= 0");
                });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_history_entries_pull_operation_id",
                table: "gacha_history_entries",
                column: "pull_operation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gacha_history_entries_user_id_created_at_utc",
                table: "gacha_history_entries",
                columns: new[] { "user_id", "created_at_utc" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ix_gacha_history_entries_user_id_pool_id_created_at_utc",
                table: "gacha_history_entries",
                columns: new[] { "user_id", "pool_id", "created_at_utc" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_user_gacha_pities_user_id_pool_id",
                table: "user_gacha_pities",
                columns: new[] { "user_id", "pool_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gacha_history_entries");

            migrationBuilder.DropTable(
                name: "user_gacha_pities");
        }
    }
}
