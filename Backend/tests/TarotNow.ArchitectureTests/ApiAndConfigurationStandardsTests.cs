using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TarotNow.ArchitectureTests;

public class ApiAndConfigurationStandardsTests
{
    [Fact]
    public void ApiControllers_ShouldDeclareApiVersionMetadata()
    {
        var backendRoot = FindBackendRoot();
        var controllerFiles = Directory.GetFiles(Path.Combine(backendRoot, "src", "TarotNow.Api", "Controllers"), "*.cs", SearchOption.TopDirectoryOnly);
        var violations = new List<string>();

        foreach (var file in controllerFiles)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            var root = tree.GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classNode in classes)
            {
                var attributes = classNode.AttributeLists.SelectMany(x => x.Attributes).ToArray();
                var isApiController = attributes.Any(attribute => attribute.Name.ToString().EndsWith("ApiController", StringComparison.Ordinal));
                if (!isApiController)
                {
                    continue;
                }

                var hasVersionMetadata = attributes.Any(attribute =>
                    attribute.Name.ToString().EndsWith("ApiVersion", StringComparison.Ordinal) ||
                    attribute.Name.ToString().EndsWith("ApiVersionNeutral", StringComparison.Ordinal));

                if (!hasVersionMetadata)
                {
                    violations.Add(ToBackendRelativePath(backendRoot, file));
                }
            }
        }

        violations.Should().BeEmpty("all API controllers must declare explicit API version metadata.");
    }

    [Fact]
    public void ApiLayer_ShouldNotHardcodeV1RouteLiteralsInAttributes()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var routeLiteralRegex = new Regex(@"\[(Route|HttpGet|HttpPost|HttpPut|HttpPatch|HttpDelete)\(""\/?api\/v1", RegexOptions.Compiled);

        var violations = Directory
            .GetFiles(apiRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => routeLiteralRegex.IsMatch(File.ReadAllText(path)))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("API attributes must use versioned constants/templates instead of hardcoded /api/v1 literals.");
    }

    [Fact]
    public void InfrastructureServices_ShouldUseOptionsPatternInsteadOfIConfiguration()
    {
        var backendRoot = FindBackendRoot();
        var infraRoot = Path.Combine(backendRoot, "src", "TarotNow.Infrastructure");
        var allowedFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "DependencyInjection.cs",
            "DependencyInjection.Auth.cs",
            "DependencyInjection.Cache.cs",
            "DependencyInjection.Persistence.cs",
            "ApplicationDbContextFactory.cs"
        };

        var forbiddenRegex = new Regex(@"\bIConfiguration\b", RegexOptions.Compiled);

        var violations = Directory
            .GetFiles(infraRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !allowedFileNames.Contains(Path.GetFileName(path)))
            .Where(path => forbiddenRegex.IsMatch(File.ReadAllText(path)))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("runtime services/repositories should bind strongly-typed options and avoid IConfiguration directly.");
    }

    [Fact]
    public void Infrastructure_ShouldNotInstantiateSmtpClientDirectlyOutsideCompositionRoot()
    {
        var backendRoot = FindBackendRoot();
        var infraRoot = Path.Combine(backendRoot, "src", "TarotNow.Infrastructure");
        var violations = Directory
            .GetFiles(infraRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !Path.GetFileName(path).StartsWith("DependencyInjection", StringComparison.Ordinal))
            .Where(path => File.ReadAllText(path).Contains("new SmtpClient(", StringComparison.Ordinal))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .ToArray();

        violations.Should().BeEmpty("SmtpClient should be created by DI container, not with new() inside services.");
    }

    [Fact]
    public void ApiLayer_ShouldUseFeatureFlagConstants()
    {
        var backendRoot = FindBackendRoot();
        var apiRoot = Path.Combine(backendRoot, "src", "TarotNow.Api");
        var rawFlagRegex = new Regex(@"IsEnabledAsync\(""[A-Za-z0-9_.-]+""\)", RegexOptions.Compiled);

        var violations = Directory
            .GetFiles(apiRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.EndsWith("FeatureFlags.cs", StringComparison.Ordinal))
            .Where(path => rawFlagRegex.IsMatch(File.ReadAllText(path)))
            .Select(path => ToBackendRelativePath(backendRoot, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        violations.Should().BeEmpty("feature flag keys should come from constants to avoid magic strings.");
    }

    [Fact]
    public void ApiHttpActions_ShouldHaveXmlSummaryComments()
    {
        var backendRoot = FindBackendRoot();
        var controllerFiles = Directory.GetFiles(Path.Combine(backendRoot, "src", "TarotNow.Api", "Controllers"), "*.cs", SearchOption.TopDirectoryOnly);
        var violations = new List<string>();

        foreach (var file in controllerFiles)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            var root = tree.GetRoot();
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();

            foreach (var method in methods)
            {
                var hasHttpAttribute = method.AttributeLists
                    .SelectMany(list => list.Attributes)
                    .Any(attribute => attribute.Name.ToString().StartsWith("Http", StringComparison.Ordinal));

                if (!hasHttpAttribute)
                {
                    continue;
                }

                if (!HasSummaryDocumentation(method))
                {
                    var line = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line}");
                }
            }
        }

        violations.Should().BeEmpty("each public HTTP endpoint should expose XML summary comments for OpenAPI documentation.");
    }

    private static bool HasSummaryDocumentation(MethodDeclarationSyntax method)
    {
        var docs = method.GetLeadingTrivia()
            .Select(trivia => trivia.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>();

        foreach (var doc in docs)
        {
            var elements = doc.Content.OfType<XmlElementSyntax>();
            if (elements.Any(element => string.Equals(element.StartTag.Name.LocalName.Text, "summary", StringComparison.Ordinal)))
            {
                return true;
            }
        }

        return false;
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
