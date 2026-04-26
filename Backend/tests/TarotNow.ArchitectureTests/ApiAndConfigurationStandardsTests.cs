using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TarotNow.ArchitectureTests;

// Bộ test kiến trúc kiểm soát chuẩn API và cấu hình hệ thống.
public class ApiAndConfigurationStandardsTests
{
    /// <summary>
    /// Xác nhận mọi API controller khai báo metadata version rõ ràng.
    /// Luồng quét toàn bộ controller, lọc class có ApiController và kiểm tra có ApiVersion/ApiVersionNeutral.
    /// </summary>
    [Fact]
    public void ApiControllers_ShouldDeclareApiVersionMetadata()
    {
        var backendRoot = FindBackendRoot();
        var controllerFiles = Directory.GetFiles(Path.Combine(backendRoot, "src", "TarotNow.Api", "Controllers"), "*.cs", SearchOption.TopDirectoryOnly);
        var violations = new List<string>();

        // Duyệt từng file controller để parse syntax tree và kiểm tra attribute class.
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
                    // Bỏ qua class không phải API controller.
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

    /// <summary>
    /// Xác nhận API layer không hardcode route literal `/api/v1` trong attribute.
    /// Luồng dùng regex quét toàn bộ file API để bắt magic string route version cứng.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Infrastructure service không dùng IConfiguration trực tiếp ngoài allowlist.
    /// Luồng này ép các service runtime đọc cấu hình qua Options pattern typed.
    /// </summary>
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

    /// <summary>
    /// Xác nhận không khởi tạo SmtpClient trực tiếp ngoài composition root.
    /// Luồng này bảo vệ nguyên tắc tạo client qua DI để dễ test và kiểm soát lifecycle.
    /// </summary>
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

    /// <summary>
    /// Xác nhận API layer dùng hằng số feature flag thay vì chuỗi cứng.
    /// Luồng regex phát hiện các lời gọi `IsEnabledAsync(\"...\")` ngoài file hằng.
    /// </summary>
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

    /// <summary>
    /// Xác nhận mọi HTTP action có XML summary comment cho OpenAPI.
    /// Luồng parse syntax tree từng controller và kiểm tra documentation trivia ở method có Http* attribute.
    /// </summary>
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
                    // Chỉ enforce tài liệu cho HTTP action methods.
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

    /// <summary>
    /// Xác nhận middleware pipeline chạy authentication trước rate limiter để partition key theo user hoạt động đúng.
    /// </summary>
    [Fact]
    public void ApiPipeline_ShouldAuthenticateBeforeRateLimiting()
    {
        var backendRoot = FindBackendRoot();
        var pipelineFile = Path.Combine(
            backendRoot,
            "src",
            "TarotNow.Api",
            "Startup",
            "ApiApplicationBuilderExtensions.cs");
        var content = File.ReadAllText(pipelineFile);

        var authIndex = content.IndexOf("app.UseAuthentication();", StringComparison.Ordinal);
        var limiterIndex = content.IndexOf("app.UseRateLimiter();", StringComparison.Ordinal);

        authIndex.Should().BeGreaterThanOrEqualTo(0);
        limiterIndex.Should().BeGreaterThanOrEqualTo(0);
        authIndex.Should().BeLessThan(
            limiterIndex,
            "UseAuthentication must be registered before UseRateLimiter.");
    }

    /// <summary>
    /// Xác nhận mọi HTTP action yêu cầu authorize đều phải có metadata rate limiting ở method hoặc class.
    /// </summary>
    [Fact]
    public void AuthorizedHttpActions_ShouldDeclareRateLimitingMetadata()
    {
        var backendRoot = FindBackendRoot();
        var controllerFiles = Directory.GetFiles(
            Path.Combine(backendRoot, "src", "TarotNow.Api", "Controllers"),
            "*.cs",
            SearchOption.TopDirectoryOnly);
        var classDeclarations = new List<(string File, ClassDeclarationSyntax ClassNode)>();

        foreach (var file in controllerFiles)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            var root = tree.GetRoot();
            classDeclarations.AddRange(
                root.DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Select(classNode => (file, classNode)));
        }

        var groupedByClass = classDeclarations
            .GroupBy(item => item.ClassNode.Identifier.Text, StringComparer.Ordinal);
        var violations = new List<string>();

        foreach (var classGroup in groupedByClass)
        {
            var classHasAuthorize = classGroup.Any(item => HasAnyAttribute(
                item.ClassNode.AttributeLists,
                "Authorize"));
            var classHasRateLimit = classGroup.Any(item => HasAnyAttribute(
                item.ClassNode.AttributeLists,
                "EnableRateLimiting",
                "DisableRateLimiting"));

            foreach (var (file, classNode) in classGroup)
            {
                foreach (var method in classNode.Members.OfType<MethodDeclarationSyntax>())
                {
                    if (!HasHttpAttribute(method.AttributeLists))
                    {
                        continue;
                    }

                    if (HasAnyAttribute(method.AttributeLists, "AllowAnonymous"))
                    {
                        continue;
                    }

                    var methodHasAuthorize = classHasAuthorize || HasAnyAttribute(method.AttributeLists, "Authorize");
                    if (!methodHasAuthorize)
                    {
                        continue;
                    }

                    var methodHasRateLimit = classHasRateLimit
                                             || HasAnyAttribute(
                                                 method.AttributeLists,
                                                 "EnableRateLimiting",
                                                 "DisableRateLimiting");
                    if (methodHasRateLimit)
                    {
                        continue;
                    }

                    var line = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    violations.Add($"{ToBackendRelativePath(backendRoot, file)}:{line}");
                }
            }
        }

        violations.Should().BeEmpty(
            "authorized endpoints must declare rate limiting metadata to prevent flood/cost abuse regressions.");
    }

    /// <summary>
    /// Xác nhận partition key của auth rate-limit ưu tiên user claim trước khi fallback về IP.
    /// </summary>
    [Fact]
    public void AuthRateLimitPartitioning_ShouldPreferUserClaimBeforeIpFallback()
    {
        var backendRoot = FindBackendRoot();
        var partitioningFile = Path.Combine(
            backendRoot,
            "src",
            "TarotNow.Api",
            "Startup",
            "ApiServiceCollectionExtensions.RateLimit.Partitioning.cs");
        var content = File.ReadAllText(partitioningFile);

        content.Should().Contain("ClaimTypes.NameIdentifier");
        content.Should().Contain("return $\"user:{userId}\";");
        content.Should().Contain("return $\"ip:{ResolveClientIp(httpContext)}\";");
    }

    /// <summary>
    /// Xác nhận các finally block giải phóng distributed lock dùng token an toàn thay vì token request đã cancel.
    /// </summary>
    [Fact]
    public void LockCleanup_ShouldUseCancellationTokenNoneInFinallyBlocks()
    {
        var backendRoot = FindBackendRoot();
        var targetFiles = new[]
        {
            Path.Combine(
                backendRoot,
                "src",
                "TarotNow.Application",
                "Features",
                "CheckIn",
                "Commands",
                "DailyCheckIn",
                "DailyCheckInCommandHandler.cs"),
            Path.Combine(
                backendRoot,
                "src",
                "TarotNow.Application",
                "Features",
                "Reading",
                "Commands",
                "StreamReading",
                "StreamReadingCommandHandler.RequestCreation.cs"),
            Path.Combine(
                backendRoot,
                "src",
                "TarotNow.Infrastructure",
                "Persistence",
                "Repositories",
                "RefreshTokenRepository.Rotate.cs")
        };

        var missing = new List<string>();
        foreach (var file in targetFiles)
        {
            var content = File.ReadAllText(file);
            if (!content.Contains("ReleaseLockAsync", StringComparison.Ordinal)
                || !content.Contains("CancellationToken.None", StringComparison.Ordinal))
            {
                missing.Add(ToBackendRelativePath(backendRoot, file));
            }
        }

        missing.Should().BeEmpty(
            "lock cleanup in finally blocks must ignore request cancellation and use CancellationToken.None.");
    }

    /// <summary>
    /// Kiểm tra một method có thẻ `<summary>` trong XML documentation hay không.
    /// Luồng này dùng Roslyn trivia để đọc comment tài liệu tại compile-time.
    /// </summary>
    private static bool HasSummaryDocumentation(MethodDeclarationSyntax method)
    {
        var docs = method.GetLeadingTrivia()
            .Select(trivia => trivia.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>();

        // Duyệt từng khối doc comment để tìm phần tử summary.
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

    private static bool HasHttpAttribute(SyntaxList<AttributeListSyntax> attributeLists)
    {
        return attributeLists
            .SelectMany(list => list.Attributes)
            .Any(attribute => attribute.Name.ToString().StartsWith("Http", StringComparison.Ordinal));
    }

    private static bool HasAnyAttribute(
        SyntaxList<AttributeListSyntax> attributeLists,
        params string[] attributeNames)
    {
        return attributeLists
            .SelectMany(list => list.Attributes)
            .Any(attribute =>
            {
                var name = attribute.Name.ToString();
                return attributeNames.Any(candidate =>
                    name.Equals(candidate, StringComparison.Ordinal)
                    || name.EndsWith(candidate, StringComparison.Ordinal)
                    || name.EndsWith($"{candidate}Attribute", StringComparison.Ordinal));
            });
    }

    /// <summary>
    /// Tìm thư mục gốc Backend từ thư mục output test hiện tại.
    /// Luồng đi ngược cây thư mục đến khi thấy đồng thời solution, src và tests.
    /// </summary>
    private static string FindBackendRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        // Đi ngược lên parent directories để định vị root độc lập môi trường chạy.
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

    /// <summary>
    /// Chuẩn hóa đường dẫn về dạng tương đối từ Backend root.
    /// Luồng này giúp output violation nhất quán và dễ đọc trên CI.
    /// </summary>
    private static string ToBackendRelativePath(string backendRoot, string fullPath)
    {
        return Path.GetRelativePath(backendRoot, fullPath).Replace('\\', '/');
    }
}
