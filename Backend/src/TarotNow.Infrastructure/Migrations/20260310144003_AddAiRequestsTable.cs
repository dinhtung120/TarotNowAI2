using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAiRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ai_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reading_session_ref = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    followup_sequence = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "requested"),
                    first_token_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completion_marker_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    finish_reason = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    retry_count = table.Column<short>(type: "smallint", nullable: false),
                    prompt_version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    policy_version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    trace_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    charge_gold = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    charge_diamond = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    requested_locale = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    returned_locale = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    fallback_reason = table.Column<string>(type: "text", nullable: true),
                    idempotency_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_ai_requests_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_ai_requests_idempotency",
                table: "ai_requests",
                column: "idempotency_key",
                unique: true,
                filter: "idempotency_key IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_ai_requests_reading",
                table: "ai_requests",
                column: "reading_session_ref");

            migrationBuilder.CreateIndex(
                name: "idx_ai_requests_status",
                table: "ai_requests",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_ai_requests_user_id",
                table: "ai_requests",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_requests");
        }
    }
}
