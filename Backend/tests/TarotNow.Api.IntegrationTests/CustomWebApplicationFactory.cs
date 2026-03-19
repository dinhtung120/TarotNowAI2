/*
 * FILE: CustomWebApplicationFactory.cs
 * MỤC ĐÍCH: Fixture dùng chung cho Integration Tests — tạo môi trường test thật.
 *   Khởi tạo Docker containers (PostgreSQL + MongoDB) qua Testcontainers library.
 *
 *   TẠI SAO DÙNG TESTCONTAINERS THAY VÌ IN-MEMORY DB?
 *   → InMemory DB (EF Core): không hỗ trợ Raw SQL, FOR UPDATE, stored procedures.
 *   → Testcontainers: tạo PostgreSQL/MongoDB thật trong Docker → test chính xác 100%.
 *   → Mỗi test class dùng database riêng → không conflict giữa các test.
 *
 *   CÁC BƯỚC KHỞI TẠO:
 *   1. Tạo Docker containers: PostgreSQL 16 + MongoDB 7.0
 *   2. Override connection strings → trỏ vào containers
 *   3. Override auth → TestAuthHandler (mock JWT)
 *   4. Override Redis → InMemory cache (không cần Redis container)
 *   5. Override MongoDB → trỏ vào MongoDB container
 *   6. EnsureCreatedAsync(): tạo schema tự động từ EF Core model
 *   7. Tạo stored procedures (debit_currency, refund_currency) cho wallet tests
 *   8. Tạo extension unaccent cho full-text search
 *
 *   IAsyncLifetime: InitializeAsync chạy TRƯỚC tests, DisposeAsync chạy SAU tests.
 */

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

/// <summary>
/// Factory tạo TestServer + Docker containers cho integration tests.
/// </summary>
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    // Docker containers: PostgreSQL 16 Alpine (nhẹ) + MongoDB 7.0
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("tarotweb_test")
        .WithUsername("postgres")
        .WithPassword("postgres_test_password")
        .Build();

    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder("mongo:7.0-jammy")
        .Build();

    /// <summary>
    /// Override cấu hình web host: thay database, auth, cache bằng test versions.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var postgresConnectionString = _dbContainer.GetConnectionString();
        var mongoConnectionString = _mongoContainer.GetConnectionString();
        
        // Override config: trỏ connection strings vào Docker containers
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSQL"] = postgresConnectionString,
                ["ConnectionStrings:MongoDB"] = mongoConnectionString,
                ["PaymentGateway:WebhookSecret"] = "TarotNow_Test_WebhookSecret_2026",
                ["Security:MfaEncryptionKey"] = "TarotNow_Test_MfaEncryption_2026"
            });
        });
        
        builder.ConfigureServices(services =>
        {
            // Remove production DbContext → thay bằng test DbContext
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<TarotNow.Infrastructure.Persistence.ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Đăng ký DbContext trỏ vào PostgreSQL container
            services.AddDbContext<TarotNow.Infrastructure.Persistence.ApplicationDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString);
            });

            // TestAuthHandler: mock authentication (không cần JWT thật)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, options => { });

            // Override Redis → InMemory (không cần Docker cho cache)
            services.AddDistributedMemoryCache();
            services.RemoveAll<StackExchange.Redis.IConnectionMultiplexer>();

            // Override MongoDB → trỏ vào MongoDB container
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
    /// Chạy TRƯỚC tất cả tests: khởi động containers + tạo schema + stored procedures.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Khởi động Docker containers
        await _dbContainer.StartAsync();
        await _mongoContainer.StartAsync();

        // Tạo extension unaccent (cần cho full-text search tiếng Việt)
        using (var rawConn = new Npgsql.NpgsqlConnection(_dbContainer.GetConnectionString()))
        {
            await rawConn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS unaccent;", rawConn);
            await cmd.ExecuteNonQueryAsync();
        }

        // Tạo schema từ EF Core model (thay vì chạy migrations)
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());
        
        using var context = new ApplicationDbContext(optionsBuilder.Options);
        await context.Database.EnsureCreatedAsync();

        // Tạo stored procedures cần cho wallet tests (debit + refund)
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

    /// <summary>Dọn dẹp Docker containers sau khi tất cả tests xong.</summary>
    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync().AsTask();
        await _mongoContainer.DisposeAsync().AsTask();
    }
}
