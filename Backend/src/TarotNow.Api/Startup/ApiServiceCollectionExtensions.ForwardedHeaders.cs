using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using System.Net;
using TarotNow.Api.Options;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Cấu hình trusted proxy cho forwarded headers.
    /// Luồng xử lý: chỉ bật khi có cờ enable, parse known proxies/networks và fail-fast nếu không có danh sách tin cậy.
    /// </summary>
    private static void ConfigureForwardedHeaders(IServiceCollection services, IConfiguration configuration)
    {
        var enabled = configuration.GetValue<bool>("ForwardedHeaders:Enabled");
        if (!enabled)
        {
            return;
        }

        var runtimeOptions = ResolveForwardedHeadersRuntimeOptions(configuration);
        var knownProxies = ReadTrustedForwardedHeaderEntries(configuration, "ForwardedHeaders:KnownProxies");
        var knownNetworks = ReadTrustedForwardedHeaderEntries(configuration, "ForwardedHeaders:KnownNetworks");

        if (knownProxies.Length == 0 && knownNetworks.Length == 0)
        {
            throw new InvalidOperationException(
                "ForwardedHeaders is enabled but no trusted proxy/network is configured. Set ForwardedHeaders:KnownProxies or ForwardedHeaders:KnownNetworks.");
        }

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = runtimeOptions.ForwardLimit;
            options.RequireHeaderSymmetry = false;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
            AddKnownProxies(options, knownProxies);
            AddKnownNetworks(options, knownNetworks);

            if (options.KnownIPNetworks.Count == 0 && options.KnownProxies.Count == 0)
            {
                throw new InvalidOperationException(
                    "ForwardedHeaders enabled but no valid trusted proxies/networks were parsed.");
            }
        });
    }

    private static string[] ReadTrustedForwardedHeaderEntries(IConfiguration configuration, string sectionName)
    {
        return configuration.GetSection(sectionName)
            .Get<string[]>()?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray() ?? Array.Empty<string>();
    }

    private static void AddKnownProxies(ForwardedHeadersOptions options, IEnumerable<string> knownProxies)
    {
        foreach (var proxy in knownProxies)
        {
            if (IPAddress.TryParse(proxy, out var parsedProxy))
            {
                options.KnownProxies.Add(parsedProxy);
            }
        }
    }

    private static void AddKnownNetworks(ForwardedHeadersOptions options, IEnumerable<string> knownNetworks)
    {
        foreach (var network in knownNetworks)
        {
            var parsedNetwork = TryParseNetwork(network);
            if (!parsedNetwork.HasValue)
            {
                continue;
            }

            options.KnownIPNetworks.Add(parsedNetwork.Value);
        }
    }

    private static System.Net.IPNetwork? TryParseNetwork(string network)
    {
        var parts = network.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return null;
        }

        if (!IPAddress.TryParse(parts[0], out var prefix))
        {
            return null;
        }

        if (!int.TryParse(parts[1], out var prefixLength))
        {
            return null;
        }

        return System.Net.IPNetwork.Parse($"{prefix}/{prefixLength}");
    }

    private static ForwardedHeadersRuntimeOptions ResolveForwardedHeadersRuntimeOptions(IConfiguration configuration)
    {
        var configured = configuration.GetSection("ForwardedHeaders").Get<ForwardedHeadersRuntimeOptions>() ?? new();
        return new ForwardedHeadersRuntimeOptions
        {
            ForwardLimit = Math.Clamp(configured.ForwardLimit, 1, 20)
        };
    }
}
