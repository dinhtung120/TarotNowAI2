using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration cập nhật cấu trúc index gacha_reward_logs để tối ưu truy vấn theo rarity.
    public partial class UpdateGachaIndexes : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: thay index user+banner+created_at bằng index user+banner+rarity+created_at.
        /// </summary>
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

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: khôi phục index cũ user+banner+created_at.
        /// </summary>
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
