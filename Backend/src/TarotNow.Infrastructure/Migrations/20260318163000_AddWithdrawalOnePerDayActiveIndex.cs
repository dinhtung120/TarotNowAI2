using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
        [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260318163000_AddWithdrawalOnePerDayActiveIndex")]
    public partial class AddWithdrawalOnePerDayActiveIndex : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests",
                columns: new[] { "user_id", "business_date_utc" },
                unique: true,
                filter: "status in ('pending','approved')");
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_withdrawal_one_per_day_active",
                table: "withdrawal_requests");
        }
    }
}
