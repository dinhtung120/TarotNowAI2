using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.MongoDb;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Fixture dùng chung cho các Integration Tests của tầng API.
/// Khởi tạo và quản lý vòng đời của Docker containers (PostgreSQL, MongoDB) thông qua Testcontainers.
/// </summary>
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("tarotweb_test")
        .WithUsername("postgres")
        .WithPassword("postgres_test_password")
        .Build();

    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:7.0-jammy")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var postgresConnectionString = _dbContainer.GetConnectionString();
        
        builder.ConfigureServices(services =>
        {
            // Xoá connection cũ
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<TarotNow.Infrastructure.Persistence.ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add Entity Framework Core using Postgres Testcontainer
            services.AddDbContext<TarotNow.Infrastructure.Persistence.ApplicationDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString);
            });

            // Register TestAuthHandler by default for Integration Tests
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, options => { });
        });

        // Đặt environment để load appsettings phù hợp nếu cần
        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        // Khởi động các Docker containers trước khi chạy test
        await _dbContainer.StartAsync();
        await _mongoContainer.StartAsync();

        // Bật Extension "unaccent" cho Testcontainer tránh lỗi "unrecognized dictionary parameter: unaccent"
        using (var rawConn = new Npgsql.NpgsqlConnection(_dbContainer.GetConnectionString()))
        {
            await rawConn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS unaccent;", rawConn);
            await cmd.ExecuteNonQueryAsync();
        }

        // Tự động Add Schema + Apply Migration cho Postgres Test Container
        // Thay vì chạy code Migration riêng lẻ, dùng schema raw của ứng dụng test là ổn định nhất
        // Hoặc có thể migrate tự động: EnsureCreatedAsync()
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());
        
        using var context = new ApplicationDbContext(optionsBuilder.Options);
        await context.Database.EnsureCreatedAsync();

        // Run Init Stored procedures required for Wallet & Refund system
        var initSql = @"
        CREATE OR REPLACE PROCEDURE debit_currency(
            p_user_id UUID, p_currency_type VARCHAR, p_transaction_type VARCHAR, p_amount BIGINT, 
            p_reference_source VARCHAR, p_reference_id VARCHAR, p_description TEXT, p_idempotency_key VARCHAR
        ) LANGUAGE plpgsql AS $$
        DECLARE
            v_current_balance BIGINT;
        BEGIN
            IF EXISTS (SELECT 1 FROM wallet_transactions WHERE idempotency_key = p_idempotency_key) THEN
                RETURN;
            END IF;

            SELECT COALESCE(SUM(amount), 0) INTO v_current_balance FROM wallet_transactions 
            WHERE user_id = p_user_id AND currency_type = p_currency_type;

            IF v_current_balance < p_amount THEN
                RAISE EXCEPTION 'Insufficient balance';
            END IF;

            INSERT INTO wallet_transactions(id, user_id, currency_type, transaction_type, amount, reference_source, reference_id, description, idempotency_key, created_at)
            VALUES (gen_random_uuid(), p_user_id, p_currency_type, p_transaction_type, -p_amount, p_reference_source, p_reference_id, p_description, p_idempotency_key, CURRENT_TIMESTAMP);
        END;
        $$;
        
        CREATE OR REPLACE PROCEDURE refund_currency(
            p_user_id UUID, p_currency_type VARCHAR, p_amount BIGINT, 
            p_reference_source VARCHAR, p_reference_id VARCHAR, p_description TEXT, p_idempotency_key VARCHAR
        ) LANGUAGE plpgsql AS $$
        BEGIN
            IF EXISTS (SELECT 1 FROM wallet_transactions WHERE idempotency_key = p_idempotency_key) THEN
                RETURN;
            END IF;

            INSERT INTO wallet_transactions(id, user_id, currency_type, transaction_type, amount, reference_source, reference_id, description, idempotency_key, created_at)
            VALUES (gen_random_uuid(), p_user_id, p_currency_type, 'AiRefund', p_amount, p_reference_source, p_reference_id, p_description, p_idempotency_key, CURRENT_TIMESTAMP);
        END;
        $$;";
        
        await context.Database.ExecuteSqlRawAsync(initSql);
    }

    public new async Task DisposeAsync()
    {
        // Dọn dẹp containers sau khi test xong
        await _dbContainer.DisposeAsync().AsTask();
        await _mongoContainer.DisposeAsync().AsTask();
    }
}
