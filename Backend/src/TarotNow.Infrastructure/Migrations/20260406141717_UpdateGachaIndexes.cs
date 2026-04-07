using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
        public partial class UpdateGachaIndexes : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_gacha_reward_logs_user_id_banner_id_created_at",
                table: "gacha_reward_logs");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_user_id_banner_id_rarity_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "banner_id", "rarity", "created_at" });
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_gacha_reward_logs_user_id_banner_id_rarity_created_at",
                table: "gacha_reward_logs");

            migrationBuilder.CreateIndex(
                name: "ix_gacha_reward_logs_user_id_banner_id_created_at",
                table: "gacha_reward_logs",
                columns: new[] { "user_id", "banner_id", "created_at" },
                descending: new[] { false, false, true });
        }
    }
}
