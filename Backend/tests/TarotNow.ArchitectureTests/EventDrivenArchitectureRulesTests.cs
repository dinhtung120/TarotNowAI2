using System.Text.RegularExpressions;
using FluentAssertions;

namespace TarotNow.ArchitectureTests;

/// <summary>
/// Bộ test kiến trúc cho các quy tắc event-driven sau refactor.
/// </summary>
public sealed class EventDrivenArchitectureRulesTests
{
    /// <summary>
    /// Xác nhận command handlers không inject side-effect services trực tiếp.
    /// </summary>
    [Fact]
    public void CommandHandlers_ShouldNotInjectDirectSideEffectServices()
    {
        var backendRoot = FindBackendRoot();
        var featuresRoot = Path.Combine(backendRoot, "src", "TarotNow.Application", "Features");
        var allowedLegacyPaths = new HashSet<string>(StringComparer.Ordinal)
        {
            "src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.cs"
        };
        var forbiddenPatterns = new[]
        {
            @"\bI\w*PushService\b",
            @"\bI\w*Notification(?:Service|Sender|Publisher|Client)?\b",
            @"\bI\w*Email(?:Sender|Service|Client)?\b",
            @"\bI\w*Webhook(?:Client|Service|Provider)?\b",
            @"\bI\w*RealtimeBridge(?:Source|Publisher|Client)?\b",
            @"\bIAiProvider\b",
            @"\bIHubContext<"
        };

        var regexes = forbiddenPatterns.Select(pattern => new Regex(pattern, RegexOptions.Compiled)).ToArray();
        var violations = Directory
            .GetFiles(featuresRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains("/Commands/", StringComparison.Ordinal))
            .Where(path =>
            {
                var text = File.ReadAllText(path);
                return text.Contains("CommandHandler", StringComparison.Ordinal)
                       && regexes.Any(regex => regex.IsMatch(text));
            })
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .Where(path => !allowedLegacyPaths.Contains(path))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("Command handlers must publish domain events instead of calling side-effect services directly.");
    }

    /// <summary>
    /// Xác nhận controllers không broadcast realtime trực tiếp bằng HubContext.
    /// </summary>
    [Fact]
    public void Controllers_ShouldNotBroadcastRealtimeDirectly()
    {
        var backendRoot = FindBackendRoot();
        var controllersRoot = Path.Combine(backendRoot, "src", "TarotNow.Api", "Controllers");
        var forbiddenPatterns = new[]
        {
            new Regex(@"\bIHubContext<", RegexOptions.Compiled),
            new Regex(@"\.Clients\.", RegexOptions.Compiled)
        };

        var violations = Directory
            .GetFiles(controllersRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
            {
                var text = File.ReadAllText(path);
                return forbiddenPatterns.Any(pattern => pattern.IsMatch(text));
            })
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("Realtime broadcast from controllers is forbidden; use domain event handlers + Redis bridge.");
    }

    /// <summary>
    /// Xác nhận hubs không broadcast trực tiếp các event đã migrate qua Redis bridge.
    /// </summary>
    [Fact]
    public void Hubs_ShouldNotBroadcastMigratedRealtimeEventsDirectly()
    {
        var backendRoot = FindBackendRoot();
        var hubsRoot = Path.Combine(backendRoot, "src", "TarotNow.Api", "Hubs");
        var forbiddenEventNames = new[]
        {
            "notification.new",
            "wallet.balance_changed",
            "message.created",
            "conversation.updated",
            "chat.unread_changed",
            "gacha.result",
            "gamification.quest_completed",
            "gamification.achievement_unlocked",
            "gamification.card_level_up"
        };

        var escaped = forbiddenEventNames.Select(Regex.Escape);
        var pattern = new Regex($@"SendAsync\(\s*""({string.Join("|", escaped)})""", RegexOptions.Compiled);
        var violations = Directory
            .GetFiles(hubsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
            {
                var text = File.ReadAllText(path);
                return pattern.IsMatch(text);
            })
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("Migrated realtime events must be published to Redis and forwarded by the bridge, not sent directly from hubs.");
    }

    /// <summary>
    /// Xác nhận command wallet mutation luôn kèm publish MoneyChangedDomainEvent trong cùng module command.
    /// </summary>
    [Fact]
    public void WalletMutationCommands_ShouldPublishMoneyChangedDomainEvent()
    {
        var backendRoot = FindBackendRoot();
        var featuresRoot = Path.Combine(backendRoot, "src", "TarotNow.Application", "Features");
        var commandFiles = Directory
            .GetFiles(featuresRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains("/Commands/", StringComparison.Ordinal))
            .ToArray();

        var mutationRegex = new Regex(@"\b_wallet(?:Repo|Repository)\.(CreditAsync|DebitAsync|FreezeAsync|RefundAsync|ConsumeAsync|ReleaseAsync)\s*\(", RegexOptions.Compiled);
        var eventRegex = new Regex(@"MoneyChangedDomainEvent", RegexOptions.Compiled);

        var groupedByDirectory = commandFiles
            .GroupBy(path => Path.GetDirectoryName(path) ?? string.Empty, StringComparer.Ordinal)
            .Where(group => group.Any(path => mutationRegex.IsMatch(File.ReadAllText(path))))
            .Select(group => new
            {
                Directory = group.Key,
                HasMoneyChangedPublish = group.Any(path => eventRegex.IsMatch(File.ReadAllText(path)))
            })
            .Where(group => !group.HasMoneyChangedPublish)
            .Select(group => ToBackendRelativePath(backendRoot, group.Directory))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        groupedByDirectory.Should().BeEmpty("Wallet mutations in commands must publish MoneyChangedDomainEvent for realtime and downstream consistency.");
    }

    /// <summary>
    /// Xác nhận các command handlers đã migrate theo event-only không còn inject repository/service/provider.
    /// </summary>
    [Fact]
    public void MigratedEventOnlyCommandHandlers_ShouldNotInjectRepositoryServiceProviderDependencies()
    {
        var backendRoot = FindBackendRoot();
        var migratedRoots = new[]
        {
            Path.Combine(backendRoot, "src", "TarotNow.Application", "Features", "Community", "Commands", "CreatePost"),
            Path.Combine(backendRoot, "src", "TarotNow.Application", "Features", "Community", "Commands", "AddComment"),
            Path.Combine(backendRoot, "src", "TarotNow.Application", "Features", "Reading", "Commands", "InitSession"),
            Path.Combine(backendRoot, "src", "TarotNow.Application", "Features", "Reading", "Commands", "RevealSession")
        };
        var forbidden = new Regex(@"\bI\w*(Repository|Service|Provider)\b", RegexOptions.Compiled);
        var allowed = new HashSet<string>(StringComparer.Ordinal)
        {
            "IInlineDomainEventDispatcher"
        };

        var violations = migratedRoots
            .SelectMany(root => Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories))
            .SelectMany(path => CollectDisallowedDependencyMentions(path, forbidden, allowed)
                .Select(match => $"{ToBackendRelativePath(backendRoot, path)} -> {match}"))
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty(
            "Migrated event-only command handlers must only dispatch domain events and must not inject repository/service/provider dependencies.");
    }

    /// <summary>
    /// Khóa baseline nợ kiến trúc hiện tại để không phát sinh thêm command handler phụ thuộc trực tiếp repository/service/provider.
    /// </summary>
    [Fact]
    public void CommandHandlers_DependencyDebtBaseline_ShouldNotIncrease()
    {
        const int currentBaselineCount = 55;

        var backendRoot = FindBackendRoot();
        var featuresRoot = Path.Combine(backendRoot, "src", "TarotNow.Application", "Features");
        var forbidden = new Regex(@"\bI\w*(Repository|Service|Provider)\b", RegexOptions.Compiled);

        var violatingFiles = Directory
            .GetFiles(featuresRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains("/Commands/", StringComparison.Ordinal))
            .Where(path =>
            {
                var text = File.ReadAllText(path);
                return text.Contains("CommandHandler", StringComparison.Ordinal)
                       && forbidden.IsMatch(text);
            })
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violatingFiles.Length.Should().BeLessThanOrEqualTo(
            currentBaselineCount,
            "we are migrating command handlers to event-only architecture incrementally; dependency debt must never grow beyond the locked baseline.");
    }

    private static IReadOnlyList<string> CollectDisallowedDependencyMentions(
        string path,
        Regex forbidden,
        IReadOnlySet<string> allowedInterfaces)
    {
        var text = File.ReadAllText(path);
        if (!text.Contains("CommandHandler", StringComparison.Ordinal))
        {
            return Array.Empty<string>();
        }

        var matches = forbidden.Matches(text);
        if (matches.Count == 0)
        {
            return Array.Empty<string>();
        }

        var disallowed = new List<string>();
        foreach (Match match in matches)
        {
            if (allowedInterfaces.Contains(match.Value))
            {
                continue;
            }

            disallowed.Add(match.Value);
        }

        return disallowed;
    }

    private static string FindBackendRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            var hasSolution = File.Exists(Path.Combine(current.FullName, "TarotNow.slnx"));
            var hasSrc = Directory.Exists(Path.Combine(current.FullName, "src"));
            var hasTests = Directory.Exists(Path.Combine(current.FullName, "tests"));

            if (hasSolution && hasSrc && hasTests)
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Cannot locate Backend root from test output directory.");
    }

    private static string ToBackendRelativePath(string backendRoot, string fullPath)
    {
        return Path.GetRelativePath(backendRoot, fullPath).Replace('\\', '/');
    }
}
