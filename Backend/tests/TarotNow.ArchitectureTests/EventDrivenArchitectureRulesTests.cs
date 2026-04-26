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
    /// Xác nhận toàn bộ command handlers đã chuyển sang event-only, không còn phụ thuộc trực tiếp repository/service/provider.
    /// </summary>
    [Fact]
    public void CommandHandlers_ShouldNotInjectRepositoryServiceProviderDependencies()
    {
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

        violatingFiles.Should().BeEmpty(
            "all command handlers must dispatch domain events via IInlineDomainEventDispatcher and must not inject repository/service/provider dependencies directly.");
    }

    /// <summary>
    /// Xác nhận command-requested domain events của command critical (IdempotencyKey/AiRequestId) đều khai báo idempotency inline.
    /// </summary>
    [Fact]
    public void CommandRequestedDomainEvents_WithCriticalIdempotencyCommands_ShouldImplementIIdempotentDomainEvent()
    {
        var backendRoot = FindBackendRoot();
        var featuresRoot = Path.Combine(backendRoot, "src", "TarotNow.Application", "Features");
        var eventOnlyFiles = Directory.GetFiles(featuresRoot, "*CommandHandler.EventOnly.cs", SearchOption.AllDirectories);
        var commandFiles = Directory.GetFiles(featuresRoot, "*.cs", SearchOption.AllDirectories);
        var commandClassRegex = new Regex(@"\b(?:class|record|struct)\s+(?<name>\w+Command)\b", RegexOptions.Compiled);
        var commandByType = commandFiles
            .SelectMany(path =>
            {
                var text = File.ReadAllText(path);
                var matches = commandClassRegex.Matches(text);
                return matches
                    .Cast<Match>()
                    .Select(match => new { CommandType = match.Groups["name"].Value, Path = path, Text = text });
            })
            .GroupBy(item => item.CommandType, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

        var commandPropertyRegex = new Regex(@"\bpublic\s+(?<command>\w+Command)\s+Command\s*\{", RegexOptions.Compiled);
        var criticalIdempotencyRegex = new Regex(@"\bIdempotencyKey\b", RegexOptions.Compiled);
        var eventContractRegex = new Regex(@"\bclass\s+\w+CommandHandlerRequestedDomainEvent\s*:\s*IIdempotentDomainEvent\b", RegexOptions.Compiled);
        var keyBuilderRegex = new Regex(@"\bCommandEventIdempotencyKey\.Build\s*\(", RegexOptions.Compiled);

        var violations = new List<string>();
        foreach (var eventFile in eventOnlyFiles)
        {
            var eventText = File.ReadAllText(eventFile);
            var commandMatch = commandPropertyRegex.Match(eventText);
            if (!commandMatch.Success)
            {
                continue;
            }

            var commandType = commandMatch.Groups["command"].Value;
            if (!commandByType.TryGetValue(commandType, out var commandInfo))
            {
                violations.Add($"{ToBackendRelativePath(backendRoot, eventFile)} (missing command file for {commandType})");
                continue;
            }

            var isCriticalCommand = criticalIdempotencyRegex.IsMatch(commandInfo.Text)
                                    || string.Equals(commandType, "CompleteAiStreamCommand", StringComparison.Ordinal);
            if (!isCriticalCommand)
            {
                continue;
            }

            if (!eventContractRegex.IsMatch(eventText) || !keyBuilderRegex.IsMatch(eventText))
            {
                violations.Add(ToBackendRelativePath(backendRoot, eventFile));
            }
        }

        violations
            .Distinct(StringComparer.Ordinal)
            .OrderBy(path => path, StringComparer.Ordinal)
            .Should()
            .BeEmpty("critical command-requested events must implement IIdempotentDomainEvent with CommandEventIdempotencyKey.Build(...).");
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
