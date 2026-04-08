using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration thêm cột active_title_ref vào bảng users.
    public partial class AddUserActiveTitleRef : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: thêm cột active_title_ref nullable để lưu danh hiệu đang kích hoạt.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "active_title_ref",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa cột active_title_ref khỏi bảng users.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active_title_ref",
                table: "users");
        }
    }
}
