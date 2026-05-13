using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Infrastructure.BackgroundJobs;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.UnitTests.Cache;

public sealed class RedisCacheRegistrationTests
{
    [Fact]
    public void RedisCacheRegistration_ShouldThrow_WhenRequireRedisTrueAndConnectionMissing()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Redis:RequireRedis"] = "true",
            ["ConnectionStrings:Redis"] = "",
            ["Redis:InstanceName"] = "TarotNowTest:"
        });

        var exception = Assert.Throws<TargetInvocationException>(() => InvokeAddRedisCaching(configuration));
        var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Contains("Redis is required but Redis configuration is missing", inner.Message);
    }

    [Fact]
    public void RedisCacheRegistration_ShouldUseMemoryFallback_WhenRequireRedisFalseInDevelopment()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Redis:RequireRedis"] = "false",
            ["ConnectionStrings:Redis"] = "",
            ["Redis:InstanceName"] = "TarotNowTest:"
        });

        using var environment = new EnvironmentVariableScope("ASPNETCORE_ENVIRONMENT", "Development");
        var services = InvokeAddRedisCaching(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        var cacheState = serviceProvider.GetRequiredService<CacheBackendState>();
        Assert.False(cacheState.UsesRedis);
    }

    [Fact]
    public void RedisCacheRegistration_ShouldRequireExplicitPolicy_ForProductionLikeEnvironment()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Redis"] = "",
            ["Redis:InstanceName"] = "TarotNowTest:"
        });

        using var environment = new EnvironmentVariableScope("ASPNETCORE_ENVIRONMENT", "Production");
        var exception = Assert.Throws<TargetInvocationException>(() => InvokeAddRedisCaching(configuration));
        var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Contains("Redis is required but Redis configuration is missing", inner.Message);
    }

    [Fact]
    public void RedisCacheRegistration_ShouldPreserveRefreshConsistencyRequirement()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Redis:RequireRedis"] = "true",
            ["AuthSecurity:RequireRedisForRefreshConsistency"] = "true",
            ["ConnectionStrings:Redis"] = "",
            ["Redis:InstanceName"] = "TarotNowTest:"
        });

        var exception = Assert.Throws<TargetInvocationException>(() => InvokeAddRedisCaching(configuration));
        var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Contains("Redis is required but Redis configuration is missing", inner.Message);
    }

    private static IConfiguration BuildConfiguration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    private static ServiceCollection InvokeAddRedisCaching(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var method = typeof(TarotNow.Infrastructure.DependencyInjection).GetMethod(
            "AddRedisCaching",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        method!.Invoke(null, [services, configuration]);
        return services;
    }

    private sealed class EnvironmentVariableScope : IDisposable
    {
        private readonly string _name;
        private readonly string? _originalValue;

        public EnvironmentVariableScope(string name, string value)
        {
            _name = name;
            _originalValue = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(_name, _originalValue);
        }
    }
}
