using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProvablyFair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_email_otps_users_UserId",
                table: "email_otps");

            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_UserId",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wallet_transactions",
                table: "wallet_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_collections",
                table: "user_collections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_refresh_tokens",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reading_sessions",
                table: "reading_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_email_otps",
                table: "email_otps");

            migrationBuilder.DropColumn(
                name: "HasConsented",
                table: "users");

            migrationBuilder.DropColumn(
                name: "client_seed",
                table: "reading_sessions");

            migrationBuilder.DropColumn(
                name: "nonce",
                table: "reading_sessions");

            migrationBuilder.DropColumn(
                name: "server_seed",
                table: "reading_sessions");

            migrationBuilder.DropColumn(
                name: "server_seed_hash",
                table: "reading_sessions");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "users",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "users",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Exp",
                table: "users",
                newName: "exp");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "GoldBalance",
                table: "users",
                newName: "gold_balance");

            migrationBuilder.RenameColumn(
                name: "FrozenDiamondBalance",
                table: "users",
                newName: "frozen_diamond_balance");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "users",
                newName: "display_name");

            migrationBuilder.RenameColumn(
                name: "DiamondBalance",
                table: "users",
                newName: "diamond_balance");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "users",
                newName: "date_of_birth");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_users_Username",
                table: "users",
                newName: "ix_users_username");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameIndex(
                name: "IX_user_collections_user_id",
                table: "user_collections",
                newName: "ix_user_collections_user_id");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "refresh_tokens",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "refresh_tokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "refresh_tokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "refresh_tokens",
                newName: "revoked_at");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "refresh_tokens",
                newName: "expires_at");

            migrationBuilder.RenameColumn(
                name: "CreatedByIp",
                table: "refresh_tokens",
                newName: "created_by_ip");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "refresh_tokens",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_tokens_Token",
                table: "refresh_tokens",
                newName: "ix_refresh_tokens_token");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_tokens_UserId",
                table: "refresh_tokens",
                newName: "ix_refresh_tokens_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_reading_sessions_user_id_created_at",
                table: "reading_sessions",
                newName: "ix_reading_sessions_user_id_created_at");

            migrationBuilder.RenameIndex(
                name: "IX_reading_sessions_user_id",
                table: "reading_sessions",
                newName: "ix_reading_sessions_user_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "email_otps",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "email_otps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "email_otps",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "OtpCode",
                table: "email_otps",
                newName: "otp_code");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "email_otps",
                newName: "is_used");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "email_otps",
                newName: "expires_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "email_otps",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_email_otps_UserId_Type_IsUsed_ExpiresAt",
                table: "email_otps",
                newName: "ix_email_otps_user_id_type_is_used_expires_at");

            migrationBuilder.AddColumn<string>(
                name: "reader_status",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "total_diamonds_purchased",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "email_otps",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "pk_wallet_transactions",
                table: "wallet_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_collections",
                table: "user_collections",
                columns: new[] { "user_id", "card_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_reading_sessions",
                table: "reading_sessions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_email_otps",
                table: "email_otps",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_email_otps_users_user_id",
                table: "email_otps",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_tokens_users_user_id",
                table: "refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_email_otps_users_user_id",
                table: "email_otps");

            migrationBuilder.DropForeignKey(
                name: "fk_refresh_tokens_users_user_id",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_wallet_transactions",
                table: "wallet_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_collections",
                table: "user_collections");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_reading_sessions",
                table: "reading_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_email_otps",
                table: "email_otps");

            migrationBuilder.DropColumn(
                name: "reader_status",
                table: "users");

            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.DropColumn(
                name: "total_diamonds_purchased",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "users",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "exp",
                table: "users",
                newName: "Exp");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "gold_balance",
                table: "users",
                newName: "GoldBalance");

            migrationBuilder.RenameColumn(
                name: "frozen_diamond_balance",
                table: "users",
                newName: "FrozenDiamondBalance");

            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "users",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "diamond_balance",
                table: "users",
                newName: "DiamondBalance");

            migrationBuilder.RenameColumn(
                name: "date_of_birth",
                table: "users",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_users_username",
                table: "users",
                newName: "IX_users_Username");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.RenameIndex(
                name: "ix_user_collections_user_id",
                table: "user_collections",
                newName: "IX_user_collections_user_id");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "refresh_tokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "refresh_tokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "refresh_tokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "revoked_at",
                table: "refresh_tokens",
                newName: "RevokedAt");

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "refresh_tokens",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "created_by_ip",
                table: "refresh_tokens",
                newName: "CreatedByIp");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "refresh_tokens",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                newName: "IX_refresh_tokens_Token");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                newName: "IX_refresh_tokens_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_reading_sessions_user_id_created_at",
                table: "reading_sessions",
                newName: "IX_reading_sessions_user_id_created_at");

            migrationBuilder.RenameIndex(
                name: "ix_reading_sessions_user_id",
                table: "reading_sessions",
                newName: "IX_reading_sessions_user_id");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "email_otps",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "email_otps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "email_otps",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "otp_code",
                table: "email_otps",
                newName: "OtpCode");

            migrationBuilder.RenameColumn(
                name: "is_used",
                table: "email_otps",
                newName: "IsUsed");

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "email_otps",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "email_otps",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_email_otps_user_id_type_is_used_expires_at",
                table: "email_otps",
                newName: "IX_email_otps_UserId_Type_IsUsed_ExpiresAt");

            migrationBuilder.AddColumn<bool>(
                name: "HasConsented",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "client_seed",
                table: "reading_sessions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "nonce",
                table: "reading_sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "server_seed",
                table: "reading_sessions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "server_seed_hash",
                table: "reading_sessions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "email_otps",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_wallet_transactions",
                table: "wallet_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_collections",
                table: "user_collections",
                columns: new[] { "user_id", "card_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_refresh_tokens",
                table: "refresh_tokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reading_sessions",
                table: "reading_sessions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_email_otps",
                table: "email_otps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_email_otps_users_UserId",
                table: "email_otps",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_UserId",
                table: "refresh_tokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
