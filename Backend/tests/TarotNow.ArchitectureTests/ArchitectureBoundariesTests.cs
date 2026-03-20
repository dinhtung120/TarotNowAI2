using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;

namespace TarotNow.ArchitectureTests;

public class ArchitectureBoundariesTests
{
    [Fact]
    public void ProjectReferences_ShouldFollowCleanArchitectureDirection()
    {
        var backendRoot = FindBackendRoot();
        var domainRefs = GetProjectReferences(Path.Combine(backendRoot, "src", "TarotNow.Domain", "TarotNow.Domain.csproj"));
        var applicationRefs = GetProjectReferences(Path.Combine(backendRoot, "src", "TarotNow.Application", "TarotNow.Application.csproj"));
        var infrastructureRefs = GetProjectReferences(Path.Combine(backendRoot, "src", "TarotNow.Infrastructure", "TarotNow.Infrastructure.csproj"));
        var apiRefs = GetProjectReferences(Path.Combine(backendRoot, "src", "TarotNow.Api", "TarotNow.Api.csproj"));

        domainRefs.Should().BeEmpty("Domain must not depend on outer layers");
        applicationRefs.Should().Equal("../TarotNow.Domain/TarotNow.Domain.csproj");
        infrastructureRefs.Should().BeEquivalentTo(
            new[]
            {
                "../TarotNow.Domain/TarotNow.Domain.csproj",
                "../TarotNow.Application/TarotNow.Application.csproj"
            });
        apiRefs.Should().BeEquivalentTo(
            new[]
            {
                "../TarotNow.Application/TarotNow.Application.csproj",
                "../TarotNow.Infrastructure/TarotNow.Infrastructure.csproj"
            });
    }

    [Fact]
    public void ApplicationLayer_ShouldNotIntroduceForbiddenDependencies()
    {
        var backendRoot = FindBackendRoot();
        var applicationRoot = Path.Combine(backendRoot, "src", "TarotNow.Application");
        var forbiddenUsingPatterns = new[]
        {
            @"using\s+TarotNow\.Infrastructure",
            @"using\s+Microsoft\.EntityFrameworkCore",
            @"using\s+MongoDB\.",
            @"using\s+Npgsql",
            @"using\s+StackExchange\.Redis",
            @"using\s+Microsoft\.AspNetCore"
        };

        var violations = FindFilesMatchingPatterns(applicationRoot, forbiddenUsingPatterns);

        violations.Should().BeEmpty("Application layer must not depend on infrastructure/web namespaces");
    }

    [Fact]
    public void ApplicationLayer_ShouldNotUseDomainExceptionType()
    {
        var backendRoot = FindBackendRoot();
        var applicationRoot = Path.Combine(backendRoot, "src", "TarotNow.Application");
        var forbiddenPatterns = new[]
        {
            @"using\s+TarotNow\.Domain\.Exceptions",
            @"\bDomainException\b"
        };

        var violations = FindFilesMatchingPatterns(applicationRoot, forbiddenPatterns);

        violations.Should().BeEmpty("Sprint 5 standardizes business-rule errors to Application.BusinessRuleException");
    }

    [Fact]
    public void DomainLayer_ShouldNotContainLegacyDomainExceptionType()
    {
        var backendRoot = FindBackendRoot();
        var domainRoot = Path.Combine(backendRoot, "src", "TarotNow.Domain");
        var forbiddenPatterns = new[]
        {
            @"class\s+DomainException\b"
        };

        var violations = FindFilesMatchingPatterns(domainRoot, forbiddenPatterns);

        violations.Should().BeEmpty("Sprint 7 removes legacy DomainException to avoid dual exception models");
    }

    [Fact]
    public void DomainLayer_ShouldNotIntroduceForbiddenDependencies()
    {
        var backendRoot = FindBackendRoot();
        var domainRoot = Path.Combine(backendRoot, "src", "TarotNow.Domain");
        var forbiddenUsingPatterns = new[]
        {
            @"using\s+TarotNow\.Application",
            @"using\s+TarotNow\.Infrastructure",
            @"using\s+Microsoft\.EntityFrameworkCore",
            @"using\s+MongoDB\.",
            @"using\s+Npgsql",
            @"using\s+StackExchange\.Redis",
            @"using\s+Microsoft\.AspNetCore"
        };

        var violations = FindFilesMatchingPatterns(domainRoot, forbiddenUsingPatterns);

        violations.Should().BeEmpty("Domain layer must stay isolated from application/infrastructure frameworks");
    }

    [Fact]
    public void Domain_ShouldNotIntroduceNewPersistenceAttributesOutsideCurrentAllowlist()
    {
        var backendRoot = FindBackendRoot();
        var domainRoot = Path.Combine(backendRoot, "src", "TarotNow.Domain");
        var persistenceAttributeRegex = new Regex(@"\[(Table|Column|Key)\b", RegexOptions.Compiled);

        var actual = Directory
            .GetFiles(domainRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => persistenceAttributeRegex.IsMatch(File.ReadAllText(path)))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        var allowlist = Array.Empty<string>();

        actual.Should().Equal(allowlist,
            "Sprint 3 removes persistence annotations from Domain; new mapping must stay in Infrastructure");
    }

    [Fact]
    public void Api_ShouldNotIntroduceNewDirectDbOrRepositoryDependenciesOutsideAllowlist()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var directDependencyPatterns = new[]
        {
            @"private\s+readonly\s+I[A-Za-z0-9_]*Repository\b",
            @"\bApplicationDbContext\b",
            @"\bMongoDbContext\b"
        };

        var actualFiles = FindFilesMatchingPatterns(apiRoot, directDependencyPatterns)
            .Where(path => path.Contains("/Controllers/", StringComparison.Ordinal) ||
                           path.Contains("/Hubs/", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        var allowlist = Array.Empty<string>();

        actualFiles.Should().Equal(allowlist,
            "Sprint 0 allows current exceptions while preventing new direct dependencies");
    }

    [Fact]
    public void Api_ShouldNotIntroduceNewConcreteOpenAiProviderUsageOutsideAllowlist()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var openAiProviderRegex = new Regex(@"\bOpenAiProvider\b", RegexOptions.Compiled);

        var actual = Directory
            .GetFiles(apiRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => openAiProviderRegex.IsMatch(File.ReadAllText(path)))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        var allowlist = Array.Empty<string>();

        actual.Should().Equal(allowlist,
            "Sprint 0 keeps current concrete provider dependency explicit until Sprint 2 removes it");
    }

    [Fact]
    public void ApiLayer_ShouldNotIntroduceDomainNamespaceDependencies()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var forbiddenUsingPatterns = new[]
        {
            @"using\s+TarotNow\.Domain(\.|;)",
            @"\bTarotNow\.Domain\."
        };

        var violations = FindFilesMatchingPatterns(apiRoot, forbiddenUsingPatterns);

        violations.Should().BeEmpty("API should stay at the presentation/application boundary without direct Domain namespace usage");
    }

    [Fact]
    public void ApiLayer_ShouldNotLogAiTelemetryDirectly()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var forbiddenPatterns = new[]
        {
            @"\.LogRequestAsync\s*\("
        };

        var violations = FindFilesMatchingPatterns(apiRoot, forbiddenPatterns)
            .Where(path => path.Contains("/Controllers/", StringComparison.Ordinal) ||
                           path.Contains("/Hubs/", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("Sprint 8 moves AI telemetry logging from API controllers into Application handlers");
    }

    private static string[] FindFilesMatchingPatterns(string root, IEnumerable<string> patterns)
    {
        var regexes = patterns.Select(pattern => new Regex(pattern, RegexOptions.Compiled)).ToArray();

        return Directory
            .GetFiles(root, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
            {
                var text = File.ReadAllText(path);
                return regexes.Any(regex => regex.IsMatch(text));
            })
            .Select(path => ToBackendRelativePath(FindBackendRoot(), path))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

    private static string[] GetProjectReferences(string csprojPath)
    {
        var document = XDocument.Load(csprojPath);

        return document
            .Descendants("ProjectReference")
            .Select(element => (string?)element.Attribute("Include"))
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => NormalizePath(value!))
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
    }

    private static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
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
