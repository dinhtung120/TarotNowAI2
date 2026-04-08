using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration căn chỉnh schema theo kiến trúc Mongo mới và bổ sung cột bảo mật.
    public partial class AlignSchemaWithMongoAndSecurity : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: drop bảng không còn dùng, thêm cột bảo mật và tạo các bảng tài chính/rút tiền mới.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reading_sessions");

            migrationBuilder.DropTable(
                name: "user_collections");

            migrationBuilder.AddColumn<string>(
                name: "mfa_backup_codes_hash_json",
                table: "users",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "mfa_enabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "mfa_secret_encrypted",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "otp_code",
                table: "email_otps",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.CreateTable(
                name: "chat_finance_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_ref = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reader_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    total_frozen = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_finance_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "withdrawal_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    business_date_utc = table.Column<DateOnly>(type: "date", nullable: false),
                    amount_diamond = table.Column<long>(type: "bigint", nullable: false),
                    amount_vnd = table.Column<long>(type: "bigint", nullable: false),
                    fee_vnd = table.Column<long>(type: "bigint", nullable: false),
                    net_amount_vnd = table.Column<long>(type: "bigint", nullable: false),
                    bank_name = table.Column<string>(type: "text", nullable: false),
                    bank_account_name = table.Column<string>(type: "text", nullable: false),
                    bank_account_number = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: true),
                    admin_note = table.Column<string>(type: "text", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_withdrawal_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_question_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    finance_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_ref = table.Column<string>(type: "text", nullable: false),
                    payer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    amount_diamond = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    proposal_message_ref = table.Column<string>(type: "text", nullable: true),
                    offer_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    accepted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reader_response_due_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    auto_release_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    auto_refund_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    released_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refunded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dispute_window_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dispute_window_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    idempotency_key = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_question_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_question_items_chat_finance_sessions_finance_session_id",
                        column: x => x.finance_session_id,
                        principalTable: "chat_finance_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_wallet_transactions_idempotency_key",
                table: "wallet_transactions",
                column: "idempotency_key",
                unique: true,
                filter: "idempotency_key IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_chat_question_items_finance_session_id",
                table: "chat_question_items",
                column: "finance_session_id");
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa đối tượng tạo mới và phục hồi trạng thái schema trước migration.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_question_items");

            migrationBuilder.DropTable(
                name: "withdrawal_requests");

            migrationBuilder.DropTable(
                name: "chat_finance_sessions");

            migrationBuilder.DropIndex(
                name: "ix_wallet_transactions_idempotency_key",
                table: "wallet_transactions");

            migrationBuilder.DropColumn(
                name: "mfa_backup_codes_hash_json",
                table: "users");

            migrationBuilder.DropColumn(
                name: "mfa_enabled",
                table: "users");

            migrationBuilder.DropColumn(
                name: "mfa_secret_encrypted",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "otp_code",
                table: "email_otps",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "reading_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_charged = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    cards_drawn = table.Column<string>(type: "jsonb", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    currency_used = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    spread_type = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_collections",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_id = table.Column<int>(type: "integer", nullable: false),
                    copies = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    exp_gained = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    last_drawn_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_collections", x => new { x.user_id, x.card_id });
                });

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_user_id",
                table: "reading_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_user_id_created_at",
                table: "reading_sessions",
                columns: new[] { "user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_user_collections_user_id",
                table: "user_collections",
                column: "user_id");
        }
    }
}
