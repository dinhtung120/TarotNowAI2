using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260416083000_UpdateGachaItemIcons")]
    public partial class UpdateGachaItemIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật đường dẫn ảnh cho các vật phẩm Gacha đã được tạo
            UpdateIcon(migrationBuilder, "exp_booster", "https://media.tarotnow.xyz/icon/exp_booster_50_20260416_165452.avif");
            UpdateIcon(migrationBuilder, "power_booster", "https://media.tarotnow.xyz/icon/power_booster_50_20260416_165453.avif");
            UpdateIcon(migrationBuilder, "defense_booster", "https://media.tarotnow.xyz/icon/defense_booster_50_20260416_165452.avif");
            UpdateIcon(migrationBuilder, "level_upgrader", "https://media.tarotnow.xyz/icon/level_upgrader_50_20260416_165453.avif");
            UpdateIcon(migrationBuilder, "free_draw_ticket", "https://media.tarotnow.xyz/icon/free_draw_ticket_50_20260416_165452.avif");
            UpdateIcon(migrationBuilder, "daily_fortune_scroll", "https://media.tarotnow.xyz/icon/daily_fortune_scroll_50_20260416_165452.avif");
            UpdateIcon(migrationBuilder, "mystery_card_pack", "https://media.tarotnow.xyz/icon/mystery_card_pack_50_20260416_165453.avif");
            UpdateIcon(migrationBuilder, "rare_title_lucky_star", "https://media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif");

            // Cập nhật cho các phần thưởng tiền tệ trong pool reward rates
            migrationBuilder.Sql("UPDATE gacha_pool_reward_rates SET icon_url = 'https://media.tarotnow.xyz/icon/gold_50_20260416_165452.avif' WHERE currency = 'gold' AND reward_kind = 'currency';");
            migrationBuilder.Sql("UPDATE gacha_pool_reward_rates SET icon_url = 'https://media.tarotnow.xyz/icon/diamond_50_20260416_165452.avif' WHERE currency = 'diamond' AND reward_kind = 'currency';");
        }

        private static void UpdateIcon(MigrationBuilder mb, string code, string url)
        {
            mb.Sql($"UPDATE item_definitions SET icon_url = '{url}' WHERE code = '{code}';");
            // Đồng thời cập nhật luôn trong bảng reward rates nếu item này đã được link
            mb.Sql($"UPDATE gacha_pool_reward_rates SET icon_url = '{url}' WHERE item_definition_id IN (SELECT id FROM item_definitions WHERE code = '{code}');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE item_definitions SET icon_url = NULL WHERE code IN ('exp_booster', 'power_booster', 'defense_booster', 'level_upgrader', 'free_draw_ticket', 'daily_fortune_scroll', 'mystery_card_pack', 'rare_title_lucky_star');");
            migrationBuilder.Sql("UPDATE gacha_pool_reward_rates SET icon_url = NULL WHERE currency IN ('gold', 'diamond') OR item_definition_id IN (SELECT id FROM item_definitions WHERE code IN ('exp_booster', 'power_booster', 'defense_booster', 'level_upgrader', 'free_draw_ticket', 'daily_fortune_scroll', 'mystery_card_pack', 'rare_title_lucky_star'));");
        }
    }
}
