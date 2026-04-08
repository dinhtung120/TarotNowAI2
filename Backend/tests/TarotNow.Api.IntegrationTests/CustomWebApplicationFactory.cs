

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.MongoDb;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.IntegrationTests;

// Factory tích hợp khởi chạy API với PostgreSQL + MongoDB test containers.
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    // Container PostgreSQL riêng cho integration tests.
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("tarotweb_test")
        .WithUsername("postgres")
        .WithPassword("postgres_test_password")
        .Build();

    // Container MongoDB riêng cho integration tests.
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder("mongo:7.0-jammy")
        .Build();

    /// <summary>
    /// Tùy biến host test: inject connection string container, auth giả lập và các service thay thế.
    /// Luồng này giúp test chạy độc lập môi trường local thật và tái lập được.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var postgresConnectionString = _dbContainer.GetConnectionString();
        var mongoConnectionString = _mongoContainer.GetConnectionString();

        // Ghi đè cấu hình hạ tầng để API test dùng đúng container runtime.
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSQL"] = postgresConnectionString,
                ["ConnectionStrings:MongoDB"] = mongoConnectionString,
                ["PaymentGateway:WebhookSecret"] = "TarotNow_Test_WebhookSecret_2026",
                ["Security:MfaEncryptionKey"] = "TarotNow_Test_MfaEncryption_2026",
                ["SystemConfig:DailyAiQuota"] = "3",
                ["SystemConfig:InFlightAiCap"] = "3",
                ["SystemConfig:ReadingRateLimitSeconds"] = "1"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Gỡ DbContext cũ để đăng ký lại bằng connection string test.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<TarotNow.Infrastructure.Persistence.ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Đăng ký lại ApplicationDbContext trỏ vào PostgreSQL container.
            services.AddDbContext<TarotNow.Infrastructure.Persistence.ApplicationDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString);
            });

            // Ép API dùng auth test scheme để kiểm thử endpoint cần authorization.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, options => { });

            // Dùng in-memory cache và loại bỏ Redis thật để test ổn định hơn.
            services.AddDistributedMemoryCache();
            services.RemoveAll<StackExchange.Redis.IConnectionMultiplexer>();

            // Ghi đè Mongo client/database theo container để cô lập dữ liệu test.
            services.RemoveAll<MongoDB.Driver.IMongoClient>();
            services.AddSingleton<MongoDB.Driver.IMongoClient>(new MongoDB.Driver.MongoClient(mongoConnectionString));

            services.RemoveAll<MongoDB.Driver.IMongoDatabase>();
            services.AddSingleton<MongoDB.Driver.IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<MongoDB.Driver.IMongoClient>();
                var url = new MongoDB.Driver.MongoUrl(mongoConnectionString);
                return client.GetDatabase(url.DatabaseName ?? "tarotweb_test");
            });
        });

        builder.UseEnvironment("Development");
    }

    /// <summary>
    /// Khởi tạo hạ tầng test trước khi chạy test suite.
    /// Luồng start containers, tạo extension SQL cần thiết, ensure schema và tạo stored procedures hỗ trợ ví.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Khởi chạy đồng thời hai container dữ liệu cho test.
        await _dbContainer.StartAsync();
        await _mongoContainer.StartAsync();

        // Bật extension `unaccent` để tương thích các truy vấn đã dùng trong ứng dụng.
        using (var rawConn = new Npgsql.NpgsqlConnection(_dbContainer.GetConnectionString()))
        {
            await rawConn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS unaccent;", rawConn);
            await cmd.ExecuteNonQueryAsync();
        }

        // Ensure database schema có đủ bảng cho integration tests.
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());

        using var context = new ApplicationDbContext(optionsBuilder.Options);
        await context.Database.EnsureCreatedAsync();

        // Khởi tạo thủ tục debit/refund để một số luồng test ví hoạt động giống production.
        var initSql = @"
        CREATE OR REPLACE PROCEDURE debit_currency(
            p_user_id UUID, p_currency VARCHAR, p_type VARCHAR, p_amount BIGINT, 
            p_reference_source VARCHAR, p_reference_id VARCHAR, p_description TEXT, p_idempotency_key VARCHAR
        ) LANGUAGE plpgsql AS $$
        DECLARE
            v_current_balance BIGINT;
        BEGIN
            IF EXISTS (SELECT 1 FROM wallet_transactions WHERE idempotency_key = p_idempotency_key) THEN
                RETURN;
            END IF;

            SELECT COALESCE(SUM(amount), 0) INTO v_current_balance FROM wallet_transactions 
            WHERE user_id = p_user_id AND currency = p_currency;

            IF v_current_balance < p_amount THEN
                RAISE EXCEPTION 'Insufficient balance';
            END IF;

            INSERT INTO wallet_transactions(id, user_id, currency, type, amount, balance_before, balance_after, reference_source, reference_id, description, idempotency_key, created_at)
            VALUES (gen_random_uuid(), p_user_id, p_currency, p_type, -p_amount, v_current_balance, v_current_balance - p_amount, p_reference_source, p_reference_id, p_description, p_idempotency_key, CURRENT_TIMESTAMP);
        END;
        $$;
        
        CREATE OR REPLACE PROCEDURE refund_currency(
            p_user_id UUID, p_currency VARCHAR, p_amount BIGINT, 
            p_reference_source VARCHAR, p_reference_id VARCHAR, p_description TEXT, p_idempotency_key VARCHAR
        ) LANGUAGE plpgsql AS $$
        DECLARE
            v_current_balance BIGINT;
        BEGIN
            IF EXISTS (SELECT 1 FROM wallet_transactions WHERE idempotency_key = p_idempotency_key) THEN
                RETURN;
            END IF;

            SELECT COALESCE(SUM(amount), 0) INTO v_current_balance FROM wallet_transactions 
            WHERE user_id = p_user_id AND currency = p_currency;

            INSERT INTO wallet_transactions(id, user_id, currency, type, amount, balance_before, balance_after, reference_source, reference_id, description, idempotency_key, created_at)
            VALUES (gen_random_uuid(), p_user_id, p_currency, 'AiRefund', p_amount, v_current_balance, v_current_balance + p_amount, p_reference_source, p_reference_id, p_description, p_idempotency_key, CURRENT_TIMESTAMP);
        END;
        $$;";

        await context.Database.ExecuteSqlRawAsync(initSql);
    }

    /// <summary>
    /// Giải phóng tài nguyên container sau khi kết thúc test.
    /// Luồng dispose tường minh để tránh rò rỉ process Docker khi chạy CI/local nhiều lần.
    /// </summary>
    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync().AsTask();
        await _mongoContainer.DisposeAsync().AsTask();
    }
}
