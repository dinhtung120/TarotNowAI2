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

    // Đồng bộ việc start containers để tránh race-condition khi nhiều test class khởi tạo host.
    private readonly SemaphoreSlim _containerStartLock = new(1, 1);
    // Đánh dấu trạng thái containers đã sẵn sàng cho host test.
    private volatile bool _containersStarted;
    // Root path tạm cho file upload trong integration tests (mỗi factory một thư mục riêng).
    private readonly string _testStorageRoot = Path.Combine(
        Path.GetTempPath(),
        $"tarotnow-integration-uploads-{Guid.NewGuid():N}");

    // Bộ env tối thiểu giúp startup pass fail-fast validation trong CI khi không có .env ở root.
    private static readonly IReadOnlyDictionary<string, string> RequiredEnvironmentDefaults =
        new Dictionary<string, string>
        {
            ["CONNECTIONSTRINGS__POSTGRESQL"] = "Host=localhost;Port=5432;Database=tarotweb_test;Username=postgres;Password=postgres_test_password",
            ["CONNECTIONSTRINGS__MONGODB"] = "mongodb://localhost:27017/tarotweb_test",
            ["JWT__SECRETKEY"] = "TarotNow_Test_JwtSecret_1234567890_ABCDEFG",
            ["JWT__ISSUER"] = "TarotNowAI-Test",
            ["JWT__AUDIENCE"] = "TarotNowAI-Users-Test",
            ["PAYMENTGATEWAY__WEBHOOKSECRET"] = "TarotNow_Test_WebhookSecret_2026",
            ["SECURITY__MFAENCRYPTIONKEY"] = "TarotNow_Test_MfaEncryption_2026",
            ["SMTP__HOST"] = "smtp.test.local",
            ["SMTP__PORT"] = "2525",
            ["SMTP__USERNAME"] = "smtp-test-user",
            ["SMTP__PASSWORD"] = "TarotNow_Test_SmtpPassword_2026",
            ["SMTP__SENDEREMAIL"] = "noreply@test.local",
            ["SMTP__SENDERNAME"] = "TarotNow Test",
            ["AIPROVIDER__APIKEY"] = "tarotnow-test-ai-key",
            ["AIPROVIDER__BASEURL"] = "https://api.openai.com/v1/",
            ["AIPROVIDER__MODEL"] = "gpt-4.1-mini",
            ["AIPROVIDER__TIMEOUTSECONDS"] = "30",
            ["AIPROVIDER__MAXRETRIES"] = "0",
            ["CORS__ALLOWEDORIGINS__0"] = "http://localhost:3000"
        };

    /// <summary>
    /// Khởi tạo factory và bơm env defaults cho test process.
    /// Luồng này giúp test không phụ thuộc file .env ở root trên máy CI.
    /// </summary>
    public CustomWebApplicationFactory()
    {
        ApplyMissingEnvironmentDefaults();
        Directory.CreateDirectory(_testStorageRoot);
    }

    /// <summary>
    /// Tùy biến host test: inject connection string container, auth giả lập và các service thay thế.
    /// Luồng này giúp test chạy độc lập môi trường local thật và tái lập được.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        EnsureContainersStarted();
        var postgresConnectionString = _dbContainer.GetConnectionString();
        var mongoConnectionString = _mongoContainer.GetConnectionString();

        // Ghi đè cấu hình hạ tầng để API test dùng đúng container runtime.
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSQL"] = postgresConnectionString,
                ["ConnectionStrings:MongoDB"] = mongoConnectionString,
                ["Jwt:SecretKey"] = "TarotNow_Test_JwtSecret_1234567890_ABCDEFG",
                ["Jwt:Issuer"] = "TarotNowAI-Test",
                ["Jwt:Audience"] = "TarotNowAI-Users-Test",
                ["PaymentGateway:WebhookSecret"] = "TarotNow_Test_WebhookSecret_2026",
                ["Security:MfaEncryptionKey"] = "TarotNow_Test_MfaEncryption_2026",
                ["Smtp:Host"] = "smtp.test.local",
                ["Smtp:Port"] = "2525",
                ["Smtp:Username"] = "smtp-test-user",
                ["Smtp:Password"] = "TarotNow_Test_SmtpPassword_2026",
                ["Smtp:SenderEmail"] = "noreply@test.local",
                ["Smtp:SenderName"] = "TarotNow Test",
                ["AiProvider:ApiKey"] = "tarotnow-test-ai-key",
                ["AiProvider:BaseUrl"] = "https://api.openai.com/v1/",
                ["AiProvider:Model"] = "gpt-4.1-mini",
                ["AiProvider:TimeoutSeconds"] = "30",
                ["AiProvider:MaxRetries"] = "0",
                ["Cors:AllowedOrigins:0"] = "http://localhost:3000",
                ["FileStorage:RootPath"] = _testStorageRoot,
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
        // Đảm bảo containers đã start một lần duy nhất trước khi chạy setup DB.
        await EnsureContainersStartedAsync();

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
        TryCleanupTestStorage();
    }

    /// <summary>
    /// Start containers một lần duy nhất theo cơ chế lock để tránh chạy trùng khi host init song song.
    /// </summary>
    private async Task EnsureContainersStartedAsync()
    {
        if (_containersStarted)
        {
            return;
        }

        await _containerStartLock.WaitAsync();
        try
        {
            if (_containersStarted)
            {
                return;
            }

            await _dbContainer.StartAsync();
            await _mongoContainer.StartAsync();
            _containersStarted = true;
        }
        finally
        {
            _containerStartLock.Release();
        }
    }

    /// <summary>
    /// Wrapper đồng bộ cho ConfigureWebHost, vì API này không hỗ trợ async.
    /// </summary>
    private void EnsureContainersStarted()
    {
        EnsureContainersStartedAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Chỉ ghi đè các env đang thiếu để không phá cấu hình custom của developer.
    /// </summary>
    private void ApplyMissingEnvironmentDefaults()
    {
        foreach (var entry in RequiredEnvironmentDefaults)
        {
            var currentValue = Environment.GetEnvironmentVariable(entry.Key);
            if (!string.IsNullOrWhiteSpace(currentValue))
            {
                continue;
            }

            Environment.SetEnvironmentVariable(entry.Key, entry.Value);
        }
    }

    /// <summary>
    /// Dọn thư mục upload tạm của integration tests.
    /// Luồng best-effort để tránh test fail chỉ vì lỗi dọn dẹp sau cùng.
    /// </summary>
    private void TryCleanupTestStorage()
    {
        try
        {
            if (Directory.Exists(_testStorageRoot))
            {
                Directory.Delete(_testStorageRoot, recursive: true);
            }
        }
        catch
        {
            // Ignore cleanup failure.
        }
    }
}
