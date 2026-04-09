using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký tầng persistence cho PostgreSQL và MongoDB.
    /// Luồng xử lý: cấu hình ApplicationDbContext (Npgsql), tạo Mongo client/database singleton và MongoDbContext.
    /// </summary>
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var postgreSqlConnectionString = configuration.GetConnectionString("PostgreSQL");
        if (string.IsNullOrWhiteSpace(postgreSqlConnectionString))
        {
            throw new InvalidOperationException("Missing required configuration ConnectionStrings:PostgreSQL (env: CONNECTIONSTRINGS__POSTGRESQL).");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgreSqlConnectionString));

        var mongoConnectionString = configuration.GetConnectionString("MongoDB");
        if (string.IsNullOrWhiteSpace(mongoConnectionString))
        {
            throw new InvalidOperationException("Missing required configuration ConnectionStrings:MongoDB (env: CONNECTIONSTRINGS__MONGODB).");
        }

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var mongoUrl = new MongoUrl(mongoConnectionString);
            var databaseName = mongoUrl.DatabaseName ?? "tarotweb";
            // Fallback tên DB mặc định để tránh null khi connection string không chỉ rõ database.
            return client.GetDatabase(databaseName);
        });

        services.AddSingleton<MongoDbContext>();
    }
}
