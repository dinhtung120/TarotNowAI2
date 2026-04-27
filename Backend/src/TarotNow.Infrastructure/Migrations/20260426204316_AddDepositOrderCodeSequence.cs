using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositOrderCodeSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1
                        FROM pg_class c
                        JOIN pg_namespace n ON n.oid = c.relnamespace
                        WHERE c.relkind = 'S'
                          AND c.relname = 'deposit_order_code_seq'
                    ) THEN
                        CREATE SEQUENCE deposit_order_code_seq START WITH 100000000 INCREMENT BY 1;
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql(
                """
                SELECT setval(
                    'deposit_order_code_seq',
                    GREATEST(
                        COALESCE((SELECT MAX(payos_order_code) FROM deposit_orders), 0),
                        99999999
                    ),
                    true
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS deposit_order_code_seq;");
        }
    }
}
