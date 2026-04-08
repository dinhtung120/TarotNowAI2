using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration thêm index unique cho idempotency escrow và conversation_ref phiên tài chính.
    public partial class AddEscrowUniquenessIndexes : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: tạo index unique cho idempotency_key và conversation_ref nhằm chặn dữ liệu trùng.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_chat_question_items_idempotency_key",
                table: "chat_question_items",
                column: "idempotency_key",
                unique: true,
                filter: "idempotency_key IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_chat_finance_sessions_conversation_ref",
                table: "chat_finance_sessions",
                column: "conversation_ref",
                unique: true);
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa các index unique đã tạo ở bước Up.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_chat_question_items_idempotency_key",
                table: "chat_question_items");

            migrationBuilder.DropIndex(
                name: "ix_chat_finance_sessions_conversation_ref",
                table: "chat_finance_sessions");
        }
    }
}
