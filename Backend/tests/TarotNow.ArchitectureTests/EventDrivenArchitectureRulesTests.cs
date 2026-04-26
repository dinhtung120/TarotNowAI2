using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TarotNow.ArchitectureTests;

/// <summary>
/// Bộ test kiến trúc cho các quy tắc event-driven sau refactor.
/// </summary>
public sealed class EventDrivenArchitectureRulesTests
{
    /// <summary>
    /// Cấm tồn tại bất kỳ class legacy kiểu *CommandExecutor.
    /// </summary>
    [Fact]
    public void Application_ShouldNotContainCommandExecutorClasses()
    {
        var backendRoot = FindBackendRoot();
        var applicationFiles = GetApplicationSourceFiles(backendRoot);
        var violations = new List<string>();

        foreach (var file in applicationFiles)
        {
            var root = ParseSyntaxRoot(file);
            var executorClasses = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(classNode => classNode.Identifier.Text.EndsWith("CommandExecutor", StringComparison.Ordinal))
                .ToArray();

            if (executorClasses.Length == 0)
            {
                continue;
            }

            foreach (var classNode in executorClasses)
            {
                var line = classNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line}");
            }
        }

        violations.Should().BeEmpty("legacy executor classes must be removed and logic must live in requested domain event handlers.");
    }

    /// <summary>
    /// Cấm mọi khai báo hoặc tham chiếu contract ICommandExecutionExecutor.
    /// </summary>
    [Fact]
    public void Application_ShouldNotReferenceICommandExecutionExecutorContract()
    {
        var backendRoot = FindBackendRoot();
        var applicationFiles = GetApplicationSourceFiles(backendRoot);
        var violations = new List<string>();

        foreach (var file in applicationFiles)
        {
            var root = ParseSyntaxRoot(file);

            var interfaceDeclarations = root
                .DescendantNodes()
                .OfType<InterfaceDeclarationSyntax>()
                .Where(interfaceNode => string.Equals(interfaceNode.Identifier.Text, "ICommandExecutionExecutor", StringComparison.Ordinal))
                .Select(interfaceNode => interfaceNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1);

            var typeReferences = root
                .DescendantNodes()
                .OfType<GenericNameSyntax>()
                .Where(genericNode => string.Equals(genericNode.Identifier.Text, "ICommandExecutionExecutor", StringComparison.Ordinal))
                .Select(genericNode => genericNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1);

            foreach (var line in interfaceDeclarations.Concat(typeReferences))
            {
                violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line}");
            }
        }

        violations.Should().BeEmpty("ICommandExecutionExecutor<,> contract must be fully removed from Application layer.");
    }

    /// <summary>
    /// Cấm để lại legacy wrappers trong DomainEvents/Handlers/CommandDispatch.
    /// </summary>
    [Fact]
    public void LegacyCommandDispatchFolder_ShouldBeEmpty()
    {
        var backendRoot = FindBackendRoot();
        var commandDispatchRoot = Path.Combine(
            backendRoot,
            "src",
            "TarotNow.Application",
            "DomainEvents",
            "Handlers",
            "CommandDispatch");

        var remainingLegacyWrappers = Directory.Exists(commandDispatchRoot)
            ? Directory.GetFiles(commandDispatchRoot, "*.cs", SearchOption.AllDirectories)
                .Select(path => ToBackendRelativePath(backendRoot, path))
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray()
            : Array.Empty<string>();

        remainingLegacyWrappers.Should().BeEmpty("command-dispatch wrappers must be removed to avoid duplicate handling paths.");
    }

    /// <summary>
    /// Xác nhận toàn bộ command handlers (IRequestHandler<,>) chỉ phụ thuộc IInlineDomainEventDispatcher.
    /// </summary>
    [Fact]
    public void CommandHandlers_ShouldOnlyDependOnInlineDomainEventDispatcher()
    {
        var backendRoot = FindBackendRoot();
        var commandFiles = Directory
            .GetFiles(
                Path.Combine(backendRoot, "src", "TarotNow.Application", "Features"),
                "*.cs",
                SearchOption.AllDirectories)
            .Where(path => path.Contains("/Commands/", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        var allowlistNoDispatcherConstructors = new HashSet<string>(StringComparer.Ordinal)
        {
            "ProcessDepositCommandHandler"
        };

        var violations = new List<string>();
        foreach (var file in commandFiles)
        {
            var root = ParseSyntaxRoot(file);
            var handlerClasses = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(classNode => classNode.BaseList is not null
                                    && classNode.BaseList.Types.Any(baseType =>
                                        baseType.Type.ToString().Contains("IRequestHandler", StringComparison.Ordinal))
                                    && !classNode.Identifier.Text.Contains("RequestedDomainEventHandler", StringComparison.Ordinal))
                .ToArray();

            foreach (var handlerClass in handlerClasses)
            {
                var constructors = handlerClass.Members
                    .OfType<ConstructorDeclarationSyntax>()
                    .ToArray();
                var className = handlerClass.Identifier.Text;

                if (constructors.Length == 0)
                {
                    if (allowlistNoDispatcherConstructors.Contains(className))
                    {
                        continue;
                    }

                    var line = handlerClass.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line} {className} (missing constructor)");
                    continue;
                }

                foreach (var constructor in constructors)
                {
                    var parameterTypes = constructor.ParameterList.Parameters
                        .Select(parameter => parameter.Type?.ToString() ?? string.Empty)
                        .ToArray();

                    var isThinConstructor = parameterTypes.Length == 1
                                            && string.Equals(
                                                parameterTypes[0],
                                                "IInlineDomainEventDispatcher",
                                                StringComparison.Ordinal);

                    if (isThinConstructor)
                    {
                        continue;
                    }

                    var line = constructor.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    var deps = parameterTypes.Length == 0 ? "(none)" : string.Join(", ", parameterTypes);
                    violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line} {className} ({deps})");
                }
            }
        }

        violations.Should().BeEmpty(
            "command handlers must stay thin and only inject IInlineDomainEventDispatcher.");
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

    private static string[] GetApplicationSourceFiles(string backendRoot)
    {
        var appRoot = Path.Combine(backendRoot, "src", "TarotNow.Application");
        return Directory
            .GetFiles(appRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();
    }

    private static CompilationUnitSyntax ParseSyntaxRoot(string filePath)
    {
        var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath), path: filePath);
        return tree.GetCompilationUnitRoot();
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
