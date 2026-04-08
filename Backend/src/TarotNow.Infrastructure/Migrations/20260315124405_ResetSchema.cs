using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration khởi tạo lại schema nền cho các bảng cốt lõi của hệ thống.
    public partial class ResetSchema : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: tạo mới bảng, khóa, index và ràng buộc cần thiết cho phiên bản schema này.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deposit_promotions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    min_amount_vnd = table.Column<long>(type: "bigint", nullable: false),
                    bonus_diamond = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deposit_promotions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    spread_type = table.Column<string>(type: "text", nullable: false),
                    question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    cards_drawn = table.Column<string>(type: "jsonb", nullable: true),
                    currency_used = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    amount_charged = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    copies = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    exp_gained = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    last_drawn_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_collections", x => new { x.user_id, x.card_id });
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    user_exp = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    gold_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    diamond_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    frozen_diamond_balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    total_diamonds_purchased = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    reader_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    balance_before = table.Column<long>(type: "bigint", nullable: false),
                    balance_after = table.Column<long>(type: "bigint", nullable: false),
                    reference_source = table.Column<string>(type: "text", nullable: true),
                    reference_id = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    idempotency_key = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallet_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reading_session_ref = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_vnd = table.Column<long>(type: "bigint", nullable: false),
                    diamond_amount = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fx_snapshot = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deposit_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_deposit_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "email_otps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    otp_code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_otps", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_otps_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_consents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    consented_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    user_agent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_consents", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_consents_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_status",
                table: "deposit_orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_transaction_id",
                table: "deposit_orders",
                column: "transaction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deposit_orders_user_id",
                table: "deposit_orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_deposit_promotions_min_amount_vnd",
                table: "deposit_promotions",
                column: "min_amount_vnd");

            migrationBuilder.CreateIndex(
                name: "ix_email_otps_user_id_type_is_used_expires_at",
                table: "email_otps",
                columns: new[] { "user_id", "type", "is_used", "expires_at" });

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_user_id",
                table: "reading_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_sessions_user_id_created_at",
                table: "reading_sessions",
                columns: new[] { "user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_collections_user_id",
                table: "user_collections",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_consents_user_id_document_type_version",
                table: "user_consents",
                columns: new[] { "user_id", "document_type", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_wallet_transactions_idempotency_key",
                table: "wallet_transactions",
                column: "idempotency_key",
                unique: true,
                filter: "idempotency_key IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa các đối tượng đã tạo ở Up theo thứ tự an toàn quan hệ phụ thuộc.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_requests");

            migrationBuilder.DropTable(
                name: "deposit_orders");

            migrationBuilder.DropTable(
                name: "deposit_promotions");

            migrationBuilder.DropTable(
                name: "email_otps");

            migrationBuilder.DropTable(
                name: "reading_sessions");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "user_collections");

            migrationBuilder.DropTable(
                name: "user_consents");

            migrationBuilder.DropTable(
                name: "wallet_transactions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
