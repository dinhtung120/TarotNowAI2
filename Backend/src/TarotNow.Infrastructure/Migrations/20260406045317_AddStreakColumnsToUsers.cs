using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration bổ sung các cột streak vào bảng users.
    public partial class AddStreakColumnsToUsers : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: thêm current_streak, last_streak_date và pre_break_streak vào users.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "current_streak",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "last_streak_date",
                table: "users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pre_break_streak",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa các cột streak đã thêm ở bước Up.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_streak",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_streak_date",
                table: "users");

            migrationBuilder.DropColumn(
                name: "pre_break_streak",
                table: "users");
        }
    }
}
