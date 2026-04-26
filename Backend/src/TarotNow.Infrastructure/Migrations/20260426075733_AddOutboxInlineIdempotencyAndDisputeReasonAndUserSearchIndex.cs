using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxInlineIdempotencyAndDisputeReasonAndUserSearchIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");

            migrationBuilder.AddColumn<string>(
                name: "dispute_reason",
                table: "chat_question_items",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "outbox_inline_handler_states",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    handler_name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    processed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_inline_handler_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_question_items_status_dispute_window_end",
                table: "chat_question_items",
                columns: new[] { "status", "dispute_window_end" });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_inline_handler_states_processed_at_utc",
                table: "outbox_inline_handler_states",
                column: "processed_at_utc");

            migrationBuilder.CreateIndex(
                name: "ux_outbox_inline_handler_states_event_handler",
                table: "outbox_inline_handler_states",
                columns: new[] { "event_key", "handler_name" },
                unique: true);

            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS ix_users_username_trgm ON users USING gin (username gin_trgm_ops);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_inline_handler_states");

            migrationBuilder.DropIndex(
                name: "ix_chat_question_items_status_dispute_window_end",
                table: "chat_question_items");

            migrationBuilder.DropColumn(
                name: "dispute_reason",
                table: "chat_question_items");

            migrationBuilder.Sql("DROP INDEX IF EXISTS ix_users_username_trgm;");
        }
    }
}
