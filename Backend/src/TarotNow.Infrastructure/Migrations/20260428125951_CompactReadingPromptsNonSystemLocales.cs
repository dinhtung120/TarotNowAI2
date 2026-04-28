using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompactReadingPromptsNonSystemLocales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR REPLACE FUNCTION __tarotnow_reading_prompt_pick_text(node jsonb, default_locale text)
                RETURNS text
                LANGUAGE sql
                IMMUTABLE
                AS $$
                SELECT
                    CASE
                        WHEN node IS NULL OR jsonb_typeof(node) = 'null' THEN NULL
                        WHEN jsonb_typeof(node) = 'string' THEN NULLIF(BTRIM(node #>> '{}'), '')
                        WHEN jsonb_typeof(node) = 'object' THEN COALESCE(
                            NULLIF(BTRIM(node ->> LOWER(COALESCE(default_locale, ''))), ''),
                            NULLIF(BTRIM(node ->> 'vi'), ''),
                            NULLIF(BTRIM(node ->> 'en'), ''),
                            NULLIF(BTRIM(node ->> 'zh'), ''),
                            (
                                SELECT NULLIF(BTRIM(item.value), '')
                                FROM jsonb_each_text(node) AS item
                                WHERE NULLIF(BTRIM(item.value), '') IS NOT NULL
                                LIMIT 1
                            )
                        )
                        ELSE NULL
                    END;
                $$;
                """);

            migrationBuilder.Sql(
                """
                WITH source AS (
                    SELECT
                        sc.key,
                        sc.value::jsonb AS doc,
                        COALESCE(NULLIF(BTRIM(sc.value::jsonb ->> 'defaultLocale'), ''), 'vi') AS default_locale
                    FROM system_configs AS sc
                    WHERE sc.key = 'ai.reading.prompts'
                      AND sc.value_kind = 'json'
                      AND sc.value IS NOT NULL
                ),
                compacted AS (
                    SELECT
                        key,
                        jsonb_set(
                            jsonb_set(
                                jsonb_set(
                                    jsonb_set(
                                        jsonb_set(
                                            jsonb_set(
                                                jsonb_set(
                                                    jsonb_set(
                                                        jsonb_set(
                                                            jsonb_set(
                                                                doc,
                                                                '{initial,default}',
                                                                to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{initial,default}', default_locale), 'My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}')),
                                                                true
                                                            ),
                                                            '{initial,daily_1}',
                                                            to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{initial,daily_1}', default_locale), 'My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}')),
                                                            true
                                                        ),
                                                        '{initial,spread_3}',
                                                        to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{initial,spread_3}', default_locale), 'My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}')),
                                                        true
                                                    ),
                                                    '{initial,spread_5}',
                                                    to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{initial,spread_5}', default_locale), 'My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}')),
                                                    true
                                                ),
                                                '{initial,spread_10}',
                                                to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{initial,spread_10}', default_locale), 'My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}')),
                                                true
                                            ),
                                            '{followup,default}',
                                            to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{followup,default}', default_locale), 'Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}')),
                                            true
                                        ),
                                        '{context,defaultQuestion}',
                                        to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{context,defaultQuestion}', default_locale), 'A general reading about my current life.')),
                                        true
                                    ),
                                    '{context,orientation,upright}',
                                    to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{context,orientation,upright}', default_locale), 'Upright')),
                                    true
                                ),
                                '{context,orientation,reversed}',
                                to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{context,orientation,reversed}', default_locale), 'Reversed')),
                                true
                            ),
                            '{context,unknownCardLabel}',
                            to_jsonb(COALESCE(__tarotnow_reading_prompt_pick_text(doc #> '{context,unknownCardLabel}', default_locale), 'Unknown Card')),
                            true
                        ) AS compact_doc
                    FROM source
                )
                UPDATE system_configs AS sc
                SET value = compacted.compact_doc::text,
                    updated_at = NOW()
                FROM compacted
                WHERE sc.key = compacted.key
                  AND sc.value::jsonb IS DISTINCT FROM compacted.compact_doc;
                """);

            migrationBuilder.Sql("DROP FUNCTION IF EXISTS __tarotnow_reading_prompt_pick_text(jsonb, text);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Irreversible data normalization migration.
        }
    }
}
