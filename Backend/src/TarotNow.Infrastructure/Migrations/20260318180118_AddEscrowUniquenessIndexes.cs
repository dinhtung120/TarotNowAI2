using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEscrowUniquenessIndexes : Migration
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
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
