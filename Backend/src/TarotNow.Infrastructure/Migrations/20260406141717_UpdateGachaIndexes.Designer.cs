
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TarotNow.Infrastructure.Persistence;

#nullable disable

namespace TarotNow.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260406141717_UpdateGachaIndexes")]
    partial class UpdateGachaIndexes
    {
                protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TarotNow.Domain.Entities.AiRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("ChargeDiamond")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("charge_diamond");

                    b.Property<long>("ChargeGold")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("charge_gold");

                    b.Property<DateTimeOffset?>("CompletionMarkerAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completion_marker_at");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FallbackReason")
                        .HasColumnType("text")
                        .HasColumnName("fallback_reason");

                    b.Property<string>("FinishReason")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("finish_reason");

                    b.Property<DateTimeOffset?>("FirstTokenAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("first_token_at");

                    b.Property<short?>("FollowupSequence")
                        .HasColumnType("smallint")
                        .HasColumnName("followup_sequence");

                    b.Property<string>("IdempotencyKey")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("idempotency_key");

                    b.Property<string>("PolicyVersion")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("policy_version");

                    b.Property<string>("PromptVersion")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("prompt_version");

                    b.Property<string>("ReadingSessionRef")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("reading_session_ref");

                    b.Property<string>("RequestedLocale")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("requested_locale");

                    b.Property<short>("RetryCount")
                        .HasColumnType("smallint")
                        .HasColumnName("retry_count");

                    b.Property<string>("ReturnedLocale")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("returned_locale");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasDefaultValue("requested")
                        .HasColumnName("status");

                    b.Property<string>("TraceId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("trace_id");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_ai_requests");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("idx_ai_requests_idempotency")
                        .HasFilter("idempotency_key IS NOT NULL");

                    b.HasIndex("ReadingSessionRef")
                        .HasDatabaseName("idx_ai_requests_reading");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_ai_requests_user_id");

                    b.HasIndex("Status", "CreatedAt")
                        .HasDatabaseName("idx_ai_requests_status");

                    b.ToTable("ai_requests", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.ChatFinanceSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConversationRef")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("conversation_ref");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("ReaderId")
                        .HasColumnType("uuid")
                        .HasColumnName("reader_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long>("TotalFrozen")
                        .HasColumnType("bigint")
                        .HasColumnName("total_frozen");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_chat_finance_sessions");

                    b.HasIndex("ConversationRef")
                        .IsUnique()
                        .HasDatabaseName("ix_chat_finance_sessions_conversation_ref");

                    b.ToTable("chat_finance_sessions", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.ChatQuestionItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("AcceptedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("accepted_at");

                    b.Property<long>("AmountDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("amount_diamond");

                    b.Property<DateTime?>("AutoRefundAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("auto_refund_at");

                    b.Property<DateTime?>("AutoReleaseAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("auto_release_at");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.Property<string>("ConversationRef")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("conversation_ref");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DisputeWindowEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("dispute_window_end");

                    b.Property<DateTime?>("DisputeWindowStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("dispute_window_start");

                    b.Property<Guid>("FinanceSessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("finance_session_id");

                    b.Property<string>("IdempotencyKey")
                        .HasColumnType("text")
                        .HasColumnName("idempotency_key");

                    b.Property<DateTime?>("OfferExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("offer_expires_at");

                    b.Property<Guid>("PayerId")
                        .HasColumnType("uuid")
                        .HasColumnName("payer_id");

                    b.Property<string>("ProposalMessageRef")
                        .HasColumnType("text")
                        .HasColumnName("proposal_message_ref");

                    b.Property<DateTime?>("ReaderResponseDueAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("reader_response_due_at");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid")
                        .HasColumnName("receiver_id");

                    b.Property<DateTime?>("RefundedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refunded_at");

                    b.Property<DateTime?>("ReleasedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("released_at");

                    b.Property<DateTime?>("RepliedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("replied_at");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_chat_question_items");

                    b.HasIndex("FinanceSessionId")
                        .HasDatabaseName("ix_chat_question_items_finance_session_id");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("ix_chat_question_items_idempotency_key")
                        .HasFilter("idempotency_key IS NOT NULL");

                    b.ToTable("chat_question_items", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.DepositOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("AmountVnd")
                        .HasColumnType("bigint")
                        .HasColumnName("amount_vnd");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long>("DiamondAmount")
                        .HasColumnType("bigint")
                        .HasColumnName("diamond_amount");

                    b.Property<string>("FxSnapshot")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("fx_snapshot");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_at");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("status");

                    b.Property<string>("TransactionId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("transaction_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_deposit_orders");

                    b.HasIndex("Status")
                        .HasDatabaseName("ix_deposit_orders_status");

                    b.HasIndex("TransactionId")
                        .IsUnique()
                        .HasDatabaseName("ix_deposit_orders_transaction_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_deposit_orders_user_id");

                    b.ToTable("deposit_orders", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.DepositPromotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("BonusDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("bonus_diamond");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<long>("MinAmountVnd")
                        .HasColumnType("bigint")
                        .HasColumnName("min_amount_vnd");

                    b.HasKey("Id")
                        .HasName("pk_deposit_promotions");

                    b.HasIndex("MinAmountVnd")
                        .HasDatabaseName("ix_deposit_promotions_min_amount_vnd");

                    b.ToTable("deposit_promotions", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.EmailOtp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_used");

                    b.Property<string>("OtpCode")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("otp_code");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_email_otps");

                    b.HasIndex("UserId", "Type", "IsUsed", "ExpiresAt")
                        .HasDatabaseName("ix_email_otps_user_id_type_is_used_expires_at");

                    b.ToTable("email_otps", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.EntitlementConsume", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BucketId")
                        .HasColumnType("uuid")
                        .HasColumnName("bucket_id");

                    b.Property<DateTime>("ConsumedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("consumed_at");

                    b.Property<string>("EntitlementKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("entitlement_key");

                    b.Property<string>("IdempotencyKey")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("idempotency_key");

                    b.Property<string>("ReferenceId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("reference_id");

                    b.Property<string>("ReferenceSource")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("reference_source");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_entitlement_consumes");

                    b.HasIndex("BucketId")
                        .HasDatabaseName("ix_entitlement_consumes_bucket_id");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("ix_entitlement_consumes_idempotency_key");

                    b.HasIndex("UserId", "ConsumedAt")
                        .HasDatabaseName("ix_entitlement_consumes_user_time");

                    b.ToTable("entitlement_consumes", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.EntitlementMappingRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_enabled");

                    b.Property<decimal>("Ratio")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)")
                        .HasColumnName("ratio");

                    b.Property<string>("SourceKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("source_key");

                    b.Property<string>("TargetKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("target_key");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_entitlement_mapping_rules");

                    b.HasIndex("SourceKey", "TargetKey")
                        .IsUnique()
                        .HasDatabaseName("ix_mapping_rules_source_target");

                    b.ToTable("entitlement_mapping_rules", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.GachaBanner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("code");

                    b.Property<long>("CostDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("cost_diamond");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("DescriptionEn")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("description_en");

                    b.Property<string>("DescriptionVi")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("description_vi");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("effective_from");

                    b.Property<DateTime?>("EffectiveTo")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("effective_to");

                    b.Property<int>("HardPityCount")
                        .HasColumnType("integer")
                        .HasColumnName("hard_pity_count");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name_en");

                    b.Property<string>("NameVi")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name_vi");

                    b.Property<string>("OddsVersion")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("odds_version");

                    b.Property<bool>("PityEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("pity_enabled");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_gacha_banners");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_gacha_banners_code");

                    b.HasIndex("IsActive")
                        .HasDatabaseName("ix_gacha_banners_is_active");

                    b.ToTable("gacha_banners", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.GachaBannerItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BannerId")
                        .HasColumnType("uuid")
                        .HasColumnName("banner_id");

                    b.Property<string>("DisplayIcon")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("display_icon");

                    b.Property<string>("DisplayNameEn")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("display_name_en");

                    b.Property<string>("DisplayNameVi")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("display_name_vi");

                    b.Property<string>("Rarity")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("rarity");

                    b.Property<string>("RewardType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("reward_type");

                    b.Property<string>("RewardValue")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("reward_value");

                    b.Property<int>("WeightBasisPoints")
                        .HasColumnType("integer")
                        .HasColumnName("weight_basis_points");

                    b.HasKey("Id")
                        .HasName("pk_gacha_banner_items");

                    b.HasIndex("BannerId")
                        .HasDatabaseName("ix_gacha_banner_items_banner_id");

                    b.ToTable("gacha_banner_items", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.GachaRewardLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BannerId")
                        .HasColumnType("uuid")
                        .HasColumnName("banner_id");

                    b.Property<Guid>("BannerItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("banner_item_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("IdempotencyKey")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("idempotency_key");

                    b.Property<string>("OddsVersion")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("odds_version");

                    b.Property<int>("PityCountAtSpin")
                        .HasColumnType("integer")
                        .HasColumnName("pity_count_at_spin");

                    b.Property<string>("Rarity")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("rarity");

                    b.Property<string>("RewardType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("reward_type");

                    b.Property<string>("RewardValue")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("reward_value");

                    b.Property<string>("RngSeed")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("rng_seed");

                    b.Property<long>("SpentDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("spent_diamond");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<bool>("WasPityTriggered")
                        .HasColumnType("boolean")
                        .HasColumnName("was_pity_triggered");

                    b.HasKey("Id")
                        .HasName("pk_gacha_reward_logs");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("ix_gacha_reward_logs_idempotency_key");

                    b.HasIndex("UserId", "CreatedAt")
                        .IsDescending(false, true)
                        .HasDatabaseName("ix_gacha_reward_logs_user_id_created_at");

                    b.HasIndex("UserId", "BannerId", "Rarity", "CreatedAt")
                        .HasDatabaseName("ix_gacha_reward_logs_user_id_banner_id_rarity_created_at");

                    b.ToTable("gacha_reward_logs", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("created_by_ip");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("revoked_at");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("token");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_tokens");

                    b.HasIndex("Token")
                        .IsUnique()
                        .HasDatabaseName("ix_refresh_tokens_token");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_tokens_user_id");

                    b.ToTable("refresh_tokens", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.SubscriptionEntitlementBucket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("BusinessDate")
                        .HasColumnType("date")
                        .HasColumnName("business_date");

                    b.Property<int>("DailyQuota")
                        .HasColumnType("integer")
                        .HasColumnName("daily_quota");

                    b.Property<string>("EntitlementKey")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("entitlement_key");

                    b.Property<DateTime>("SubscriptionEndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("subscription_end_date");

                    b.Property<int>("UsedToday")
                        .HasColumnType("integer")
                        .HasColumnName("used_today");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("UserSubscriptionId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_subscription_id");

                    b.HasKey("Id")
                        .HasName("pk_subscription_entitlement_buckets");

                    b.HasIndex("BusinessDate")
                        .HasDatabaseName("ix_buckets_business_date");

                    b.HasIndex("UserSubscriptionId")
                        .HasDatabaseName("ix_buckets_subscription_id");

                    b.HasIndex("UserId", "EntitlementKey", "BusinessDate", "SubscriptionEndDate")
                        .HasDatabaseName("ix_buckets_user_key_date");

                    b.ToTable("subscription_entitlement_buckets", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.SubscriptionPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("integer")
                        .HasColumnName("display_order");

                    b.Property<int>("DurationDays")
                        .HasColumnType("integer")
                        .HasColumnName("duration_days");

                    b.Property<string>("EntitlementsJson")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("entitlements_json");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<long>("PriceDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("price_diamond");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_subscription_plans");

                    b.HasIndex("IsActive")
                        .HasDatabaseName("ix_subscription_plans_is_active")
                        .HasFilter("is_active = true");

                    b.ToTable("subscription_plans", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ActiveTitleRef")
                        .HasColumnType("text")
                        .HasColumnName("active_title_ref");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("CurrentStreak")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("current_streak");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("display_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<long>("Exp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("user_exp");

                    b.Property<DateOnly?>("LastStreakDate")
                        .HasColumnType("date")
                        .HasColumnName("last_streak_date");

                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1)
                        .HasColumnName("user_level");

                    b.Property<string>("MfaBackupCodesHashJson")
                        .HasColumnType("jsonb")
                        .HasColumnName("mfa_backup_codes_hash_json");

                    b.Property<bool>("MfaEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("mfa_enabled");

                    b.Property<string>("MfaSecretEncrypted")
                        .HasColumnType("text")
                        .HasColumnName("mfa_secret_encrypted");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password_hash");

                    b.Property<int>("PreBreakStreak")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("pre_break_streak");

                    b.Property<string>("ReaderStatus")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("reader_status");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("role");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasDefaultValue("pending")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.UserConsent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("ConsentedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("consented_at");

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("document_type");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("ip_address");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("user_agent");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_user_consents");

                    b.HasIndex("UserId", "DocumentType", "Version")
                        .IsUnique()
                        .HasDatabaseName("ix_user_consents_user_id_document_type_version");

                    b.ToTable("user_consents", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.UserSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<string>("IdempotencyKey")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("idempotency_key");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uuid")
                        .HasColumnName("plan_id");

                    b.Property<long>("PricePaidDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("price_paid_diamond");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("status");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_subscriptions");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("ix_user_subscriptions_idempotency_key");

                    b.HasIndex("PlanId")
                        .HasDatabaseName("ix_user_subscriptions_plan_id");

                    b.HasIndex("Status", "EndDate")
                        .HasDatabaseName("ix_user_subscriptions_status_end_date");

                    b.HasIndex("UserId", "Status")
                        .HasDatabaseName("ix_user_subscriptions_user_id_status");

                    b.ToTable("user_subscriptions", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.WalletTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint")
                        .HasColumnName("amount");

                    b.Property<long>("BalanceAfter")
                        .HasColumnType("bigint")
                        .HasColumnName("balance_after");

                    b.Property<long>("BalanceBefore")
                        .HasColumnType("bigint")
                        .HasColumnName("balance_before");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("IdempotencyKey")
                        .HasColumnType("text")
                        .HasColumnName("idempotency_key");

                    b.Property<string>("MetadataJson")
                        .HasColumnType("jsonb")
                        .HasColumnName("metadata_json");

                    b.Property<string>("ReferenceId")
                        .HasColumnType("text")
                        .HasColumnName("reference_id");

                    b.Property<string>("ReferenceSource")
                        .HasColumnType("text")
                        .HasColumnName("reference_source");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_wallet_transactions");

                    b.HasIndex("IdempotencyKey")
                        .IsUnique()
                        .HasDatabaseName("ix_wallet_transactions_idempotency_key")
                        .HasFilter("idempotency_key IS NOT NULL");

                    b.ToTable("wallet_transactions", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.WithdrawalRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uuid")
                        .HasColumnName("admin_id");

                    b.Property<string>("AdminNote")
                        .HasColumnType("text")
                        .HasColumnName("admin_note");

                    b.Property<long>("AmountDiamond")
                        .HasColumnType("bigint")
                        .HasColumnName("amount_diamond");

                    b.Property<long>("AmountVnd")
                        .HasColumnType("bigint")
                        .HasColumnName("amount_vnd");

                    b.Property<string>("BankAccountName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bank_account_name");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bank_account_number");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bank_name");

                    b.Property<DateOnly>("BusinessDateUtc")
                        .HasColumnType("date")
                        .HasColumnName("business_date_utc");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long>("FeeVnd")
                        .HasColumnType("bigint")
                        .HasColumnName("fee_vnd");

                    b.Property<long>("NetAmountVnd")
                        .HasColumnType("bigint")
                        .HasColumnName("net_amount_vnd");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_at");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_withdrawal_requests");

                    b.HasIndex("UserId", "BusinessDateUtc")
                        .IsUnique()
                        .HasDatabaseName("ix_withdrawal_one_per_day_active")
                        .HasFilter("status in ('pending','approved')");

                    b.ToTable("withdrawal_requests", (string)null);
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.AiRequest", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ai_requests_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.ChatQuestionItem", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.ChatFinanceSession", "FinanceSession")
                        .WithMany("QuestionItems")
                        .HasForeignKey("FinanceSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_chat_question_items_chat_finance_sessions_finance_session_id");

                    b.Navigation("FinanceSession");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.DepositOrder", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_deposit_orders_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.EmailOtp", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_email_otps_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.EntitlementConsume", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.SubscriptionEntitlementBucket", "Bucket")
                        .WithMany()
                        .HasForeignKey("BucketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_entitlement_consumes_subscription_entitlement_buckets_bucke~");

                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_entitlement_consumes_users_user_id");

                    b.Navigation("Bucket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.GachaBannerItem", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.GachaBanner", "Banner")
                        .WithMany("Items")
                        .HasForeignKey("BannerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_gacha_banner_items_gacha_banners_banner_id");

                    b.Navigation("Banner");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.SubscriptionEntitlementBucket", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.UserSubscription", "UserSubscription")
                        .WithMany()
                        .HasForeignKey("UserSubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_subscription_entitlement_buckets_user_subscriptions_user_su~");

                    b.Navigation("UserSubscription");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.User", b =>
                {
                    b.OwnsOne("TarotNow.Domain.Entities.UserWallet", "Wallet", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<long>("DiamondBalance")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasDefaultValue(0L)
                                .HasColumnName("diamond_balance");

                            b1.Property<long>("FrozenDiamondBalance")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasDefaultValue(0L)
                                .HasColumnName("frozen_diamond_balance");

                            b1.Property<long>("GoldBalance")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasDefaultValue(0L)
                                .HasColumnName("gold_balance");

                            b1.Property<long>("TotalDiamondsPurchased")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasDefaultValue(0L)
                                .HasColumnName("total_diamonds_purchased");

                            b1.HasKey("UserId")
                                .HasName("pk_users");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.Navigation("Wallet")
                        .IsRequired();
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.UserConsent", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany("Consents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_consents_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.UserSubscription", b =>
                {
                    b.HasOne("TarotNow.Domain.Entities.SubscriptionPlan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_user_subscriptions_subscription_plans_plan_id");

                    b.HasOne("TarotNow.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_subscriptions_users_user_id");

                    b.Navigation("Plan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.ChatFinanceSession", b =>
                {
                    b.Navigation("QuestionItems");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.GachaBanner", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("TarotNow.Domain.Entities.User", b =>
                {
                    b.Navigation("Consents");
                });
#pragma warning restore 612, 618
        }
    }
}
