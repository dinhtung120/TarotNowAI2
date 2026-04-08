using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260318163000_AddWithdrawalOnePerDayActiveIndex")]
    // Migration thêm unique index giới hạn mỗi user một yêu cầu rút active mỗi ngày.
    public partial class AddWithdrawalOnePerDayActiveIndex : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: tạo unique index có filter trạng thái pending/approved để chặn gửi trùng theo ngày.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests",
                columns: new[] { "user_id", "business_date_utc" },
                unique: true,
                filter: "status in ('pending','approved')");
        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: xóa index giới hạn one-per-day đã tạo trong Up.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests");
        }
    }
}
