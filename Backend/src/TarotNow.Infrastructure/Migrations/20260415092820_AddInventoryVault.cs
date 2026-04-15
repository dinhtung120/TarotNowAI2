using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryVault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory_item_use_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    idempotency_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    item_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    target_card_id = table.Column<int>(type: "integer", nullable: true),
                    processed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory_item_use_operations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_definitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    enhancement_type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    is_consumable = table.Column<bool>(type: "boolean", nullable: false),
                    is_permanent = table.Column<bool>(type: "boolean", nullable: false),
                    effect_value = table.Column<int>(type: "integer", nullable: false),
                    success_rate_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    name_vi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_en = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_zh = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description_vi = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    description_en = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    description_zh = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    acquired_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_items_item_definitions_item_definition_id",
                        column: x => x.item_definition_id,
                        principalTable: "item_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_inventory_item_use_operations_user_id_idempotency_key",
                table: "inventory_item_use_operations",
                columns: new[] { "user_id", "idempotency_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_inventory_item_use_operations_user_id_processed_at_utc",
                table: "inventory_item_use_operations",
                columns: new[] { "user_id", "processed_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_item_definitions_code",
                table: "item_definitions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_definitions_type_is_active",
                table: "item_definitions",
                columns: new[] { "type", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_user_items_item_definition_id",
                table: "user_items",
                column: "item_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_items_user_id",
                table: "user_items",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_items_user_id_item_definition_id",
                table: "user_items",
                columns: new[] { "user_id", "item_definition_id" },
                unique: true);

            migrationBuilder.InsertData(
                table: "item_definitions",
                columns: new[]
                {
                    "id",
                    "code",
                    "type",
                    "enhancement_type",
                    "rarity",
                    "is_consumable",
                    "is_permanent",
                    "effect_value",
                    "success_rate_percent",
                    "name_vi",
                    "name_en",
                    "name_zh",
                    "description_vi",
                    "description_en",
                    "description_zh",
                    "icon_url",
                    "is_active",
                    "created_at_utc",
                    "updated_at_utc",
                },
                values: new object[,]
                {
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008301111"),
                        "exp_booster",
                        "card_enhancer",
                        "exp",
                        "common",
                        true,
                        false,
                        100,
                        100m,
                        "Bùa Tăng EXP",
                        "EXP Booster",
                        "经验增幅符",
                        "Tăng 100 EXP cho một lá bài.",
                        "Increase 100 EXP for one card.",
                        "为一张卡牌增加100经验。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008302222"),
                        "power_booster",
                        "card_enhancer",
                        "power",
                        "common",
                        true,
                        false,
                        15,
                        100m,
                        "Bùa Tăng Sức Mạnh",
                        "Power Booster",
                        "力量增幅符",
                        "Tăng 15 ATK cho một lá bài.",
                        "Increase 15 ATK for one card.",
                        "为一张卡牌增加15攻击。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008303333"),
                        "defense_booster",
                        "card_enhancer",
                        "defense",
                        "common",
                        true,
                        false,
                        15,
                        100m,
                        "Bùa Tăng Phòng Thủ",
                        "Defense Booster",
                        "防御增幅符",
                        "Tăng 15 DEF cho một lá bài.",
                        "Increase 15 DEF for one card.",
                        "为一张卡牌增加15防御。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008304444"),
                        "level_upgrader",
                        "card_enhancer",
                        "level_upgrade",
                        "rare",
                        true,
                        false,
                        1,
                        35m,
                        "Bí Bảo Thăng Cấp",
                        "Level Upgrader",
                        "升级秘宝",
                        "Có 35% cơ hội tăng 1 cấp cho lá bài.",
                        "35% chance to upgrade card level by 1.",
                        "有35%概率将卡牌提升1级。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008305555"),
                        "free_draw_ticket",
                        "reading_booster",
                        "free_draw",
                        "uncommon",
                        true,
                        false,
                        1,
                        100m,
                        "Vé Xem Bài Miễn Phí",
                        "Free Draw Ticket",
                        "免费占卜券",
                        "Cấp 1 lượt xem bài miễn phí.",
                        "Grant 1 free tarot draw.",
                        "可获得1次免费占卜。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008306666"),
                        "daily_fortune_scroll",
                        "consumable_special",
                        "luck",
                        "rare",
                        true,
                        false,
                        10,
                        100m,
                        "Cuộn Vận May",
                        "Daily Fortune Scroll",
                        "每日幸运卷轴",
                        "Tăng 10 điểm may mắn tạm thời.",
                        "Apply 10 temporary luck points.",
                        "临时增加10点幸运值。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008307777"),
                        "mystery_card_pack",
                        "consumable_special",
                        null,
                        "epic",
                        true,
                        false,
                        1,
                        100m,
                        "Gói Bài Bí Ẩn",
                        "Mystery Card Pack",
                        "神秘卡包",
                        "Mở gói bí ẩn để nhận vật phẩm tarot.",
                        "Open a mystery pack to obtain tarot items.",
                        "开启神秘卡包获得塔罗道具。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        new Guid("5f76748d-e6ae-4be9-b675-2d7008308888"),
                        "rare_title_lucky_star",
                        "rare_title",
                        null,
                        "legendary",
                        false,
                        true,
                        1,
                        100m,
                        "Danh Hiệu: Ngôi Sao May Mắn",
                        "Title: Lucky Star",
                        "称号：幸运之星",
                        "Danh hiệu hiếm, sở hữu vĩnh viễn.",
                        "Rare permanent title item.",
                        "稀有永久称号道具。",
                        null,
                        true,
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                    },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008301111"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008302222"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008303333"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008304444"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008305555"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008306666"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008307777"));
            migrationBuilder.DeleteData(
                table: "item_definitions",
                keyColumn: "id",
                keyValue: new Guid("5f76748d-e6ae-4be9-b675-2d7008308888"));

            migrationBuilder.DropTable(
                name: "inventory_item_use_operations");

            migrationBuilder.DropTable(
                name: "user_items");

            migrationBuilder.DropTable(
                name: "item_definitions");
        }
    }
}
