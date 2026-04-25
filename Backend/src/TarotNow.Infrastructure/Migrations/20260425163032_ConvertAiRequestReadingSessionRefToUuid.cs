using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertAiRequestReadingSessionRefToUuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = current_schema()
                          AND table_name = 'ai_requests'
                          AND column_name = 'reading_session_ref'
                          AND data_type <> 'uuid'
                    ) THEN
                        ALTER TABLE ai_requests
                            ALTER COLUMN reading_session_ref TYPE uuid
                            USING NULLIF(BTRIM(reading_session_ref), '')::uuid;
                    END IF;
                END
                $$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = current_schema()
                          AND table_name = 'ai_requests'
                          AND column_name = 'reading_session_ref'
                          AND data_type = 'uuid'
                    ) THEN
                        ALTER TABLE ai_requests
                            ALTER COLUMN reading_session_ref TYPE character varying(36)
                            USING reading_session_ref::text;
                    END IF;
                END
                $$;
                """);
        }
    }
}
