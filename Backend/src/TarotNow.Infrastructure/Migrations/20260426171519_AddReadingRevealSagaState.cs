using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingRevealSagaState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reading_reveal_saga_states",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    revealed_cards_json = table.Column<string>(type: "text", nullable: true),
                    charge_debited = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    charge_currency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    charge_change_type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    charge_amount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    charge_reference_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    collection_applied = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    exp_granted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    session_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    revealed_event_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    refund_compensated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    last_attempt_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    next_repair_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_error = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_reveal_saga_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reading_reveal_saga_states_status_next_repair",
                table: "reading_reveal_saga_states",
                columns: new[] { "status", "next_repair_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ux_reading_reveal_saga_states_session_id",
                table: "reading_reveal_saga_states",
                column: "session_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reading_reveal_saga_states");
        }
    }
}
