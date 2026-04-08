using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    // Migration placeholder cho thay đổi active title ref (không phát sinh thao tác schema).
    public partial class AddActiveTitleRef : Migration
    {
        /// <summary>
        /// Áp dụng thay đổi schema theo hướng nâng cấp.
        /// Luồng xử lý: migration này không có thao tác vì thay đổi đã được xử lý ở migration khác.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <summary>
        /// Hoàn tác thay đổi schema của migration này khi rollback.
        /// Luồng xử lý: migration này không có thao tác rollback tương ứng.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
