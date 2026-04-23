using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemConfigsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_configs",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    value_kind = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false, defaultValue: "scalar"),
                    description = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_configs", x => x.key);
                    table.CheckConstraint("ck_system_configs_value_kind", "\"value_kind\" IN ('scalar', 'json')");
                    table.ForeignKey(
                        name: "fk_system_configs_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_system_configs_updated_at",
                table: "system_configs",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "ix_system_configs_updated_by",
                table: "system_configs",
                column: "updated_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_configs");
        }
    }
}
