using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;

namespace TarotNow.ArchitectureTests;

// Bộ test kiến trúc kiểm soát ranh giới phụ thuộc giữa các layer.
public class ArchitectureBoundariesTests
{
    /// <summary>
    /// Xác nhận hướng tham chiếu project tuân thủ Clean Architecture.
    /// Luồng đọc ProjectReference của từng csproj và so sánh với dependency direction cho phép.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Application layer không thêm phụ thuộc cấm vào hạ tầng/web framework.
    /// Luồng quét pattern `using` bị cấm trong toàn bộ source Application.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Application không dùng lại mô hình DomainException cũ.
    /// Luồng này enforce chuẩn hóa lỗi business về Application.BusinessRuleException.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Domain layer không còn class DomainException legacy.
    /// Luồng này ngăn tái xuất hiện dual exception model ở Domain.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Domain layer không thêm phụ thuộc cấm từ application/infrastructure/framework.
    /// Luồng này giữ Domain thuần và độc lập framework.
    /// </summary>
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

    /// <summary>
    /// Xác nhận Domain không thêm annotation persistence ngoài allowlist.
    /// Luồng này enforce mapping ORM phải nằm ở Infrastructure, không nằm trong Domain model.
    /// </summary>
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

    /// <summary>
    /// Xác nhận API không thêm phụ thuộc trực tiếp DB/repository ngoài allowlist.
    /// Luồng này giữ controller/hub ở boundary presentation thay vì truy cập hạ tầng trực tiếp.
    /// </summary>
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

    /// <summary>
    /// Xác nhận API không dùng trực tiếp concrete OpenAiProvider ngoài allowlist.
    /// Luồng này chuẩn bị cho mục tiêu phụ thuộc abstraction provider.
    /// </summary>
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

    /// <summary>
    /// Xác nhận API layer không phụ thuộc trực tiếp namespace Domain.
    /// Luồng này bảo vệ boundary API -> Application thay vì API -> Domain.
    /// </summary>
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

    /// <summary>
    /// Xác nhận API layer không log AI telemetry trực tiếp.
    /// Luồng này enforce telemetry được xử lý ở Application handler thay vì controller/hub.
    /// </summary>
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

    /// <summary>
    /// Tìm danh sách file chứa bất kỳ pattern cấm nào.
    /// Luồng quét toàn bộ `*.cs` trong root và trả path tương đối cho báo cáo.
    /// </summary>
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

    /// <summary>
    /// Đọc danh sách ProjectReference từ file csproj.
    /// Luồng chuẩn hóa path để so sánh deterministic giữa các môi trường OS.
    /// </summary>
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

    /// <summary>
    /// Chuẩn hóa dấu phân cách đường dẫn sang `/`.
    /// Luồng này tránh sai khác do khác hệ điều hành khi assert.
    /// </summary>
    private static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }

    /// <summary>
    /// Tìm thư mục gốc Backend từ thư mục output của test.
    /// Luồng đi ngược parent folders đến khi thấy đủ solution/src/tests.
    /// </summary>
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

    /// <summary>
    /// Chuyển full path sang đường dẫn tương đối theo Backend root.
    /// Luồng này chuẩn hóa output violation dễ đọc và so sánh.
    /// </summary>
    private static string ToBackendRelativePath(string backendRoot, string fullPath)
    {
        return Path.GetRelativePath(backendRoot, fullPath).Replace('\\', '/');
    }
}
