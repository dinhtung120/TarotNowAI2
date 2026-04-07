using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
        public partial class AddStreakColumnsToUsers : Migration
    {
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
