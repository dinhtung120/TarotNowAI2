using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration tăng độ dài idempotency_key của entitlement_consumes để tránh truncate key dài.
    public partial class IncreaseEntitlementConsumeIdempotencyKeyLength : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: tăng max length idempotency_key từ 100 lên 255 ký tự.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idempotency_key",
                table: "entitlement_consumes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: trả max length idempotency_key về 100 ký tự như trước.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idempotency_key",
                table: "entitlement_consumes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
