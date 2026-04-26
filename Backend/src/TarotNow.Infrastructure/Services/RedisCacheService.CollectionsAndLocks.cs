namespace TarotNow.Infrastructure.Services;

public partial class RedisCacheService
{
    /// <inheritdoc />
    public async Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(member))
        {
            return;
        }

        var normalizedMember = member.Trim();
        if (_redisDatabase != null)
        {
            await _redisDatabase.SetAddAsync(key, normalizedMember);
            if (expiration.HasValue)
            {
                await _redisDatabase.KeyExpireAsync(key, expiration.Value);
            }

            return;
        }

        var list = await GetAsync<List<string>>(key, cancellationToken) ?? new List<string>();
        if (!list.Contains(normalizedMember, StringComparer.Ordinal))
        {
            list.Add(normalizedMember);
        }

        await SetAsync(key, list, expiration, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(member))
        {
            return;
        }

        var normalizedMember = member.Trim();
        if (_redisDatabase != null)
        {
            await _redisDatabase.SetRemoveAsync(key, normalizedMember);
            return;
        }

        var list = await GetAsync<List<string>>(key, cancellationToken);
        if (list is null || list.Count == 0)
        {
            return;
        }

        list.RemoveAll(x => string.Equals(x, normalizedMember, StringComparison.Ordinal));
        if (list.Count == 0)
        {
            await RemoveAsync(key, cancellationToken);
            return;
        }

        await SetAsync(key, list, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            var members = await _redisDatabase.SetMembersAsync(key);
            if (members.Length == 0)
            {
                return Array.Empty<string>();
            }

            return members
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        var list = await GetAsync<List<string>>(key, cancellationToken);
        if (list is null || list.Count == 0)
        {
            return Array.Empty<string>();
        }

        return list
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

}
