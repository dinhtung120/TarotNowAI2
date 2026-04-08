using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration seed dữ liệu gói subscription ban đầu.
    public partial class SeedInitialSubscriptionPlans : Migration
    {
        /// <summary>
        /// Áp dụng dữ liệu seed theo hướng nâng cấp.
        /// Luồng xử lý: chèn 2 gói subscription mẫu (weekly/monthly) với entitlements mặc định.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed gói Weekly Premium.
            migrationBuilder.Sql(@"
                INSERT INTO subscription_plans (id, name, description, price_diamond, duration_days, entitlements_json, is_active, display_order, created_at)
                VALUES (
                    'd1b11111-1111-1111-1111-111111111111', 
                    'Weekly Premium', 
                    '7 days of premium perks with daily reading quotas.', 
                    100, 
                    7, 
                    '[
                        {""key"":""free_spread_3_daily"", ""dailyQuota"":3},
                        {""key"":""free_spread_5_daily"", ""dailyQuota"":1},
                        {""key"":""free_ai_stream_daily"", ""dailyQuota"":5}
                    ]', 
                    true, 
                    1, 
                    timezone('utc', now())
                );
            ");

            // Seed gói Monthly Premium.
            migrationBuilder.Sql(@"
                INSERT INTO subscription_plans (id, name, description, price_diamond, duration_days, entitlements_json, is_active, display_order, created_at)
                VALUES (
                    'd1b22222-2222-2222-2222-222222222222', 
                    'Monthly Premium', 
                    '30 days of full premium experience. Best value for serious explorers.', 
                    300, 
                    30, 
                    '[
                        {""key"":""free_spread_3_daily"", ""dailyQuota"":5},
                        {""key"":""free_spread_5_daily"", ""dailyQuota"":3},
                        {""key"":""free_ai_stream_daily"", ""dailyQuota"":10},
                        {""key"":""bonus_exp_multiplier"", ""dailyQuota"":120}
                    ]', 
                    true, 
                    2, 
                    timezone('utc', now())
                );
            ");
        }

        /// <summary>
        /// Hoàn tác dữ liệu seed của migration này khi rollback.
        /// Luồng xử lý: xóa các gói subscription seed theo id cố định.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM subscription_plans WHERE id IN ('d1b11111-1111-1111-1111-111111111111', 'd1b22222-2222-2222-2222-222222222222');");
        }
    }
}
