using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260414153000_AddUniqueActiveAuthSessionDeviceIndex")]
    public partial class AddUniqueActiveAuthSessionDeviceIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_auth_sessions_user_id_device_id",
                table: "auth_sessions");

            migrationBuilder.CreateIndex(
                name: "ix_auth_sessions_user_id_device_id_active",
                table: "auth_sessions",
                columns: new[] { "user_id", "device_id" },
                unique: true,
                filter: "revoked_at_utc IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_auth_sessions_user_id_device_id_active",
                table: "auth_sessions");

            migrationBuilder.CreateIndex(
                name: "ix_auth_sessions_user_id_device_id",
                table: "auth_sessions",
                columns: new[] { "user_id", "device_id" });
        }
    }
}
