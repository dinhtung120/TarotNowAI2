using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorInventoryTicketsAndFreeDrawBySpread : Migration
    {
        private const string FreeDrawTicket3Id = "5f76748d-e6ae-4be9-b675-2d7008309991";
        private const string FreeDrawTicket5Id = "5f76748d-e6ae-4be9-b675-2d7008309992";
        private const string FreeDrawTicket10Id = "5f76748d-e6ae-4be9-b675-2d7008309993";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_free_draw_credits_user_id",
                table: "free_draw_credits");

            migrationBuilder.AddColumn<int>(
                name: "spread_card_count",
                table: "free_draw_credits",
                type: "integer",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.Sql("""
                UPDATE free_draw_credits
                SET spread_card_count = 3
                WHERE spread_card_count NOT IN (3, 5, 10);
                """);

            migrationBuilder.CreateIndex(
                name: "ix_free_draw_credits_user_id",
                table: "free_draw_credits",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_free_draw_credits_user_id_spread_card_count",
                table: "free_draw_credits",
                columns: new[] { "user_id", "spread_card_count" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_free_draw_credits_spread_card_count_valid",
                table: "free_draw_credits",
                sql: "\"spread_card_count\" IN (3,5,10)");

            migrationBuilder.Sql($"""
                INSERT INTO item_definitions
                (
                    id,
                    code,
                    type,
                    enhancement_type,
                    rarity,
                    is_consumable,
                    is_permanent,
                    effect_value,
                    success_rate_percent,
                    name_vi,
                    name_en,
                    name_zh,
                    description_vi,
                    description_en,
                    description_zh,
                    icon_url,
                    is_active,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES
                (
                    '{FreeDrawTicket3Id}',
                    'free_draw_ticket_3',
                    'reading_booster',
                    'free_draw',
                    'uncommon',
                    TRUE,
                    FALSE,
                    1,
                    100,
                    'Vé Xem Bài Miễn Phí 3 Lá',
                    'Free Reading Ticket (3 Cards)',
                    '免费占卜券（3张）',
                    'Cộng 1 lượt xem bài miễn phí cho trải 3 lá.',
                    'Grant 1 free reading for the 3-card spread.',
                    '增加1次3张牌免费占卜次数。',
                    'https://media.tarotnow.xyz/icon/free_draw_ticket_50_20260416_165452.avif',
                    TRUE,
                    NOW() AT TIME ZONE 'UTC',
                    NOW() AT TIME ZONE 'UTC'
                )
                ON CONFLICT (code) DO UPDATE SET
                    type = EXCLUDED.type,
                    enhancement_type = EXCLUDED.enhancement_type,
                    rarity = EXCLUDED.rarity,
                    is_consumable = EXCLUDED.is_consumable,
                    is_permanent = EXCLUDED.is_permanent,
                    effect_value = EXCLUDED.effect_value,
                    success_rate_percent = EXCLUDED.success_rate_percent,
                    name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    name_zh = EXCLUDED.name_zh,
                    description_vi = EXCLUDED.description_vi,
                    description_en = EXCLUDED.description_en,
                    description_zh = EXCLUDED.description_zh,
                    icon_url = EXCLUDED.icon_url,
                    is_active = TRUE,
                    updated_at_utc = NOW() AT TIME ZONE 'UTC';
                """);

            migrationBuilder.Sql($"""
                INSERT INTO item_definitions
                (
                    id,
                    code,
                    type,
                    enhancement_type,
                    rarity,
                    is_consumable,
                    is_permanent,
                    effect_value,
                    success_rate_percent,
                    name_vi,
                    name_en,
                    name_zh,
                    description_vi,
                    description_en,
                    description_zh,
                    icon_url,
                    is_active,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES
                (
                    '{FreeDrawTicket5Id}',
                    'free_draw_ticket_5',
                    'reading_booster',
                    'free_draw',
                    'rare',
                    TRUE,
                    FALSE,
                    1,
                    100,
                    'Vé Xem Bài Miễn Phí 5 Lá',
                    'Free Reading Ticket (5 Cards)',
                    '免费占卜券（5张）',
                    'Cộng 1 lượt xem bài miễn phí cho trải 5 lá.',
                    'Grant 1 free reading for the 5-card spread.',
                    '增加1次5张牌免费占卜次数。',
                    'https://media.tarotnow.xyz/icon/free_draw_ticket_50_20260416_165452.avif',
                    TRUE,
                    NOW() AT TIME ZONE 'UTC',
                    NOW() AT TIME ZONE 'UTC'
                )
                ON CONFLICT (code) DO UPDATE SET
                    type = EXCLUDED.type,
                    enhancement_type = EXCLUDED.enhancement_type,
                    rarity = EXCLUDED.rarity,
                    is_consumable = EXCLUDED.is_consumable,
                    is_permanent = EXCLUDED.is_permanent,
                    effect_value = EXCLUDED.effect_value,
                    success_rate_percent = EXCLUDED.success_rate_percent,
                    name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    name_zh = EXCLUDED.name_zh,
                    description_vi = EXCLUDED.description_vi,
                    description_en = EXCLUDED.description_en,
                    description_zh = EXCLUDED.description_zh,
                    icon_url = EXCLUDED.icon_url,
                    is_active = TRUE,
                    updated_at_utc = NOW() AT TIME ZONE 'UTC';
                """);

            migrationBuilder.Sql($"""
                INSERT INTO item_definitions
                (
                    id,
                    code,
                    type,
                    enhancement_type,
                    rarity,
                    is_consumable,
                    is_permanent,
                    effect_value,
                    success_rate_percent,
                    name_vi,
                    name_en,
                    name_zh,
                    description_vi,
                    description_en,
                    description_zh,
                    icon_url,
                    is_active,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES
                (
                    '{FreeDrawTicket10Id}',
                    'free_draw_ticket_10',
                    'reading_booster',
                    'free_draw',
                    'epic',
                    TRUE,
                    FALSE,
                    1,
                    100,
                    'Vé Xem Bài Miễn Phí 10 Lá',
                    'Free Reading Ticket (10 Cards)',
                    '免费占卜券（10张）',
                    'Cộng 1 lượt xem bài miễn phí cho trải 10 lá.',
                    'Grant 1 free reading for the 10-card spread.',
                    '增加1次10张牌免费占卜次数。',
                    'https://media.tarotnow.xyz/icon/free_draw_ticket_50_20260416_165452.avif',
                    TRUE,
                    NOW() AT TIME ZONE 'UTC',
                    NOW() AT TIME ZONE 'UTC'
                )
                ON CONFLICT (code) DO UPDATE SET
                    type = EXCLUDED.type,
                    enhancement_type = EXCLUDED.enhancement_type,
                    rarity = EXCLUDED.rarity,
                    is_consumable = EXCLUDED.is_consumable,
                    is_permanent = EXCLUDED.is_permanent,
                    effect_value = EXCLUDED.effect_value,
                    success_rate_percent = EXCLUDED.success_rate_percent,
                    name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    name_zh = EXCLUDED.name_zh,
                    description_vi = EXCLUDED.description_vi,
                    description_en = EXCLUDED.description_en,
                    description_zh = EXCLUDED.description_zh,
                    icon_url = EXCLUDED.icon_url,
                    is_active = TRUE,
                    updated_at_utc = NOW() AT TIME ZONE 'UTC';
                """);

            migrationBuilder.Sql("""
                WITH old_ticket AS (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket'
                ),
                new_ticket AS (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_3'
                )
                UPDATE user_items target
                SET
                    quantity = target.quantity + source.quantity,
                    acquired_at_utc = LEAST(target.acquired_at_utc, source.acquired_at_utc),
                    updated_at_utc = NOW() AT TIME ZONE 'UTC'
                FROM user_items source
                WHERE target.user_id = source.user_id
                  AND target.item_definition_id = (SELECT id FROM new_ticket)
                  AND source.item_definition_id = (SELECT id FROM old_ticket);
                """);

            migrationBuilder.Sql("""
                WITH old_ticket AS (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket'
                ),
                new_ticket AS (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_3'
                )
                DELETE FROM user_items source
                USING user_items target
                WHERE target.user_id = source.user_id
                  AND target.item_definition_id = (SELECT id FROM new_ticket)
                  AND source.item_definition_id = (SELECT id FROM old_ticket);
                """);

            migrationBuilder.Sql("""
                UPDATE user_items
                SET
                    item_definition_id = (SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_3'),
                    updated_at_utc = NOW() AT TIME ZONE 'UTC'
                WHERE item_definition_id = (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket'
                );
                """);

            migrationBuilder.Sql("""
                UPDATE gacha_pool_reward_rates
                SET item_definition_id = (SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_3')
                WHERE item_definition_id = (
                    SELECT id FROM item_definitions WHERE code = 'free_draw_ticket'
                );
                """);

            migrationBuilder.Sql("""
                UPDATE gacha_pool_reward_rates
                SET item_definition_id = (SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_5')
                WHERE item_definition_id = (
                    SELECT id FROM item_definitions WHERE code = 'level_upgrader'
                );
                """);

            migrationBuilder.Sql("""
                UPDATE gacha_pool_reward_rates
                SET item_definition_id = (SELECT id FROM item_definitions WHERE code = 'exp_booster')
                WHERE item_definition_id = (
                    SELECT id FROM item_definitions WHERE code = 'daily_fortune_scroll'
                );
                """);

            migrationBuilder.Sql("""
                UPDATE gacha_pool_reward_rates
                SET item_definition_id = (SELECT id FROM item_definitions WHERE code = 'free_draw_ticket_10')
                WHERE item_definition_id = (
                    SELECT id FROM item_definitions WHERE code = 'mystery_card_pack'
                );
                """);

            migrationBuilder.Sql("""
                UPDATE gacha_pool_reward_rates rates
                SET
                    icon_url = defs.icon_url,
                    name_vi = defs.name_vi,
                    name_en = defs.name_en,
                    name_zh = defs.name_zh
                FROM item_definitions defs
                WHERE rates.item_definition_id = defs.id
                  AND rates.reward_kind = 'item';
                """);

            migrationBuilder.Sql("""
                UPDATE item_definitions
                SET
                    effect_value = 1,
                    success_rate_percent = 100,
                    description_vi = 'Tăng ngẫu nhiên từ 1 đến 100 EXP cho một lá bài.',
                    description_en = 'Randomly increase 1 to 100 EXP for one card.',
                    description_zh = '为一张卡牌随机增加1到100经验。',
                    updated_at_utc = NOW() AT TIME ZONE 'UTC'
                WHERE code = 'exp_booster';
                """);

            migrationBuilder.Sql("""
                UPDATE item_definitions
                SET
                    effect_value = 1,
                    success_rate_percent = 100,
                    description_vi = 'Tăng ngẫu nhiên 1% đến 10% ATK cho một lá bài.',
                    description_en = 'Randomly increase 1% to 10% ATK for one card.',
                    description_zh = '为一张卡牌随机增加1%到10%攻击。',
                    updated_at_utc = NOW() AT TIME ZONE 'UTC'
                WHERE code = 'power_booster';
                """);

            migrationBuilder.Sql("""
                UPDATE item_definitions
                SET
                    effect_value = 1,
                    success_rate_percent = 100,
                    description_vi = 'Tăng ngẫu nhiên 1% đến 10% DEF cho một lá bài.',
                    description_en = 'Randomly increase 1% to 10% DEF for one card.',
                    description_zh = '为一张卡牌随机增加1%到10%防御。',
                    updated_at_utc = NOW() AT TIME ZONE 'UTC'
                WHERE code = 'defense_booster';
                """);

            migrationBuilder.Sql("""
                DELETE FROM user_items
                WHERE item_definition_id IN (
                    SELECT id
                    FROM item_definitions
                    WHERE code IN ('level_upgrader', 'daily_fortune_scroll', 'mystery_card_pack')
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM item_definitions
                WHERE code IN ('level_upgrader', 'daily_fortune_scroll', 'mystery_card_pack', 'free_draw_ticket');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_free_draw_credits_user_id",
                table: "free_draw_credits");

            migrationBuilder.DropIndex(
                name: "ix_free_draw_credits_user_id_spread_card_count",
                table: "free_draw_credits");

            migrationBuilder.DropCheckConstraint(
                name: "ck_free_draw_credits_spread_card_count_valid",
                table: "free_draw_credits");

            migrationBuilder.DropColumn(
                name: "spread_card_count",
                table: "free_draw_credits");

            migrationBuilder.Sql("""
                DELETE FROM user_items
                WHERE item_definition_id IN (
                    SELECT id
                    FROM item_definitions
                    WHERE code IN ('free_draw_ticket_3', 'free_draw_ticket_5', 'free_draw_ticket_10')
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM item_definitions
                WHERE code IN ('free_draw_ticket_3', 'free_draw_ticket_5', 'free_draw_ticket_10');
                """);

            migrationBuilder.CreateIndex(
                name: "ix_free_draw_credits_user_id",
                table: "free_draw_credits",
                column: "user_id",
                unique: true);
        }
    }
}
