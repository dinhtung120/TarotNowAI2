using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        var mongoConnectionString = configuration.GetConnectionString("MongoDB")
            ?? "mongodb://localhost:27017/tarotweb";

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var mongoUrl = new MongoUrl(mongoConnectionString);
            var databaseName = mongoUrl.DatabaseName ?? "tarotweb";
            return client.GetDatabase(databaseName);
        });

        services.AddSingleton<MongoDbContext>();
    }
}
