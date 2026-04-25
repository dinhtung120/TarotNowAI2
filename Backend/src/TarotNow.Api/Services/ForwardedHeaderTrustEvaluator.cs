using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using HttpOverridesIPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace TarotNow.Api.Services;

public interface IForwardedHeaderTrustEvaluator
{
    bool IsTrustedProxy(IPAddress? remoteIpAddress);
}

/// <summary>
/// Đánh giá request có đi qua trusted proxy/network hay không để quyết định tin cậy forwarded headers.
/// </summary>
public sealed class ForwardedHeaderTrustEvaluator : IForwardedHeaderTrustEvaluator
{
    private readonly bool _forwardedHeadersEnabled;
    private readonly HashSet<IPAddress> _knownProxies;
    private readonly IReadOnlyList<HttpOverridesIPNetwork> _knownNetworks;

    public ForwardedHeaderTrustEvaluator(
        IConfiguration configuration,
        IOptions<ForwardedHeadersOptions> forwardedHeadersOptions)
    {
        _forwardedHeadersEnabled = configuration.GetValue<bool>("ForwardedHeaders:Enabled");
        var options = forwardedHeadersOptions.Value;
        _knownProxies = options.KnownProxies.ToHashSet();
        _knownNetworks = options.KnownNetworks.ToArray();
    }

    public bool IsTrustedProxy(IPAddress? remoteIpAddress)
    {
        if (!_forwardedHeadersEnabled || remoteIpAddress is null)
        {
            return false;
        }

        if (_knownProxies.Contains(remoteIpAddress))
        {
            return true;
        }

        foreach (var network in _knownNetworks)
        {
            if (network.Contains(remoteIpAddress))
            {
                return true;
            }
        }

        return false;
    }
}
