using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260424103000_MigrateLegacySystemConfigKeys")]
    public partial class MigrateLegacySystemConfigKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Map legacy keys sang key chuẩn (ưu tiên giữ key chuẩn nếu đã được admin chỉnh).
            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "reading_cost_spread_3_gold",
                targetKey: "pricing.spread_3.gold",
                targetDefaultValue: "50",
                targetDescription: "Giá Gold cho trải bài 3 lá.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "reading_cost_spread_5_gold",
                targetKey: "pricing.spread_5.gold",
                targetDefaultValue: "100",
                targetDescription: "Giá Gold cho trải bài 5 lá.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "reading_cost_spread_10_gold",
                targetKey: "pricing.spread_10.gold",
                targetDefaultValue: "500",
                targetDescription: "Giá Gold cho trải bài 10 lá.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "ai_daily_quota_free",
                targetKey: "ai.daily_quota",
                targetDefaultValue: "3",
                targetDescription: "Hạn mức request AI theo ngày.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "ai_in_flight_cap",
                targetKey: "ai.in_flight_cap",
                targetDefaultValue: "3",
                targetDescription: "Số request AI đồng thời tối đa mỗi user.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "daily_checkin_gold",
                targetKey: "checkin.daily_gold",
                targetDefaultValue: "5",
                targetDescription: "Gold thưởng check-in hằng ngày.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "streak_freeze_window_hours",
                targetKey: "streak.freeze_window_hours",
                targetDefaultValue: "24",
                targetDescription: "Cửa sổ mua streak freeze theo giờ.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "gacha_cost_diamond",
                targetKey: "gacha.cost_diamond",
                targetDefaultValue: "5",
                targetDescription: "Chi phí Diamond mặc định cho một lượt quay gacha.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "min_withdrawal_diamond",
                targetKey: "withdrawal.min_diamond",
                targetDefaultValue: "500",
                targetDescription: "Ngưỡng Diamond tối thiểu cho một lệnh rút.");

            UpsertFromLegacyPercentToRate(
                migrationBuilder,
                legacyKey: "platform_fee_percent",
                targetKey: "withdrawal.fee_rate",
                targetDescription: "Tỷ lệ phí rút (0-1).");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "chat_reader_response_hours",
                targetKey: "escrow.reader_response_due_hours",
                targetDefaultValue: "24",
                targetDescription: "Deadline reader trả lời sau khi nhận câu hỏi.");

            UpsertFromLegacy(
                migrationBuilder,
                legacyKey: "chat_auto_release_hours",
                targetKey: "escrow.auto_refund_hours",
                targetDefaultValue: "24",
                targetDescription: "Deadline tự động refund theo giờ.");

            // 2) Xóa toàn bộ key legacy/dead để chốt 1 bộ key chuẩn duy nhất.
            migrationBuilder.Sql(
                """
                DELETE FROM system_configs
                WHERE key IN (
                    'ai_daily_quota_free',
                    'ai_error_timeout_seconds',
                    'ai_in_flight_cap',
                    'ai_max_retries',
                    'ai_max_retry_per_request',
                    'ai_timeout_before_token_seconds',
                    'ai_timeout_seconds',
                    'chat_auto_release_hours',
                    'chat_min_question_diamond',
                    'chat_reader_response_hours',
                    'daily_checkin_gold',
                    'friend_chain_daily_cap',
                    'friend_chain_daily_limit',
                    'friend_chain_reward_gold',
                    'gacha_cost_diamond',
                    'min_withdrawal_diamond',
                    'offer_timeout_hours',
                    'platform_fee_percent',
                    'reading_cost_daily_1_gold',
                    'reading_cost_spread_10_gold',
                    'reading_cost_spread_3_gold',
                    'reading_cost_spread_5_gold',
                    'referral_first_deposit_gold',
                    'referral_reward_gold',
                    'register_bonus_gold',
                    'share_reward_gold',
                    'streak_freeze_enabled',
                    'streak_freeze_window_hours'
                );
                """);
        }

        private static void UpsertFromLegacy(
            MigrationBuilder migrationBuilder,
            string legacyKey,
            string targetKey,
            string targetDefaultValue,
            string targetDescription)
        {
            migrationBuilder.Sql(
                $"""
                INSERT INTO system_configs (key, value, value_kind, description, updated_by, updated_at)
                SELECT '{targetKey}', legacy.value, 'scalar', '{targetDescription}', legacy.updated_by, NOW()
                FROM system_configs AS legacy
                WHERE legacy.key = '{legacyKey}'
                  AND NOT EXISTS (
                      SELECT 1
                      FROM system_configs AS target
                      WHERE target.key = '{targetKey}'
                  );

                UPDATE system_configs AS target
                SET
                    value = legacy.value,
                    value_kind = 'scalar',
                    updated_by = COALESCE(target.updated_by, legacy.updated_by),
                    updated_at = NOW()
                FROM system_configs AS legacy
                WHERE legacy.key = '{legacyKey}'
                  AND target.key = '{targetKey}'
                  AND target.updated_by IS NULL
                  AND target.value = '{targetDefaultValue}';
                """);
        }

        private static void UpsertFromLegacyPercentToRate(
            MigrationBuilder migrationBuilder,
            string legacyKey,
            string targetKey,
            string targetDescription)
        {
            migrationBuilder.Sql(
                $"""
                INSERT INTO system_configs (key, value, value_kind, description, updated_by, updated_at)
                SELECT
                    '{targetKey}',
                    LEAST(GREATEST(legacy.value::numeric / 100.0, 0), 1)::text,
                    'scalar',
                    '{targetDescription}',
                    legacy.updated_by,
                    NOW()
                FROM system_configs AS legacy
                WHERE legacy.key = '{legacyKey}'
                  AND legacy.value ~ '^[0-9]+(\\.[0-9]+)?$'
                  AND NOT EXISTS (
                      SELECT 1
                      FROM system_configs AS target
                      WHERE target.key = '{targetKey}'
                  );

                UPDATE system_configs AS target
                SET
                    value = LEAST(GREATEST(legacy.value::numeric / 100.0, 0), 1)::text,
                    value_kind = 'scalar',
                    updated_by = COALESCE(target.updated_by, legacy.updated_by),
                    updated_at = NOW()
                FROM system_configs AS legacy
                WHERE legacy.key = '{legacyKey}'
                  AND legacy.value ~ '^[0-9]+(\\.[0-9]+)?$'
                  AND target.key = '{targetKey}'
                  AND target.updated_by IS NULL
                  AND target.value IN ('0.10', '0.1');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Irreversible data cleanup migration.
        }
    }
}
