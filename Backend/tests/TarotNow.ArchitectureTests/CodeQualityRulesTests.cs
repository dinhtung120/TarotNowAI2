using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TarotNow.ArchitectureTests;

// Bộ test kiến trúc kiểm soát ngân sách chất lượng mã nguồn.
public class CodeQualityRulesTests
{
    // Ngưỡng tối đa logical lines cho mỗi file nguồn.
    private const int MaxLogicalFileLines = 180;
    // Ngưỡng tối đa logical lines cho mỗi method/local function.
    private const int MaxMethodLines = 70;
    // Ngưỡng tối đa tham số bắt buộc cho mỗi method.
    private const int MaxMethodParameters = 5;
    // Ngưỡng tối đa cyclomatic complexity.
    private const int MaxMethodCyclomaticComplexity = 15;
    // Ngưỡng tối đa cognitive complexity.
    private const int MaxMethodCognitiveComplexity = 15;

    /// <summary>
    /// Xác nhận từng file nguồn không vượt ngân sách logical lines.
    /// Luồng này giúp phát hiện file phình to, khó bảo trì.
    /// </summary>
    [Fact]
    public void SourceFiles_ShouldStayWithinLogicalLineBudget()
    {
        var metrics = AnalyzeSourceFiles();

        var violations = metrics
            .Where(metric => metric.LogicalLines > MaxLogicalFileLines)
            .Select(metric => $"{metric.RelativePath} ({metric.LogicalLines} logical lines)")
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        AssertNoViolations(violations, $"Each source file must stay <= {MaxLogicalFileLines} logical lines.");
    }

    /// <summary>
    /// Xác nhận từng method/local function không vượt ngân sách số dòng.
    /// Luồng này buộc hàm giữ độ ngắn gọn và dễ đọc.
    /// </summary>
    [Fact]
    public void Methods_ShouldStayWithinLineBudget()
    {
        var metrics = AnalyzeSourceFiles();

        var violations = metrics
            .SelectMany(metric => metric.Methods.Select(method => (metric.RelativePath, method)))
            .Where(item => item.method.LogicalLines > MaxMethodLines)
            .Select(item =>
                $"{item.RelativePath}:{item.method.StartLine} {item.method.Name} ({item.method.LogicalLines} lines)")
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        AssertNoViolations(violations, $"Each method/function must stay <= {MaxMethodLines} logical lines.");
    }

    /// <summary>
    /// Xác nhận method không có quá nhiều tham số bắt buộc.
    /// Luồng này phát hiện dấu hiệu cần refactor sang request object/value object.
    /// </summary>
    [Fact]
    public void Methods_ShouldNotHaveTooManyParameters()
    {
        var metrics = AnalyzeSourceFiles();

        var violations = metrics
            .SelectMany(metric => metric.Methods.Select(method => (metric.RelativePath, method)))
            .Where(item => item.method.ParameterCount > MaxMethodParameters)
            .Select(item =>
                $"{item.RelativePath}:{item.method.StartLine} {item.method.Name} ({item.method.ParameterCount} parameters)")
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        AssertNoViolations(violations, $"Each method/function must stay <= {MaxMethodParameters} parameters.");
    }

    /// <summary>
    /// Xác nhận method không vượt ngưỡng cyclomatic complexity.
    /// Luồng này giảm rủi ro nhánh logic quá nhiều gây khó test.
    /// </summary>
    [Fact]
    public void Methods_ShouldStayWithinCyclomaticComplexityBudget()
    {
        var metrics = AnalyzeSourceFiles();

        var violations = metrics
            .SelectMany(metric => metric.Methods.Select(method => (metric.RelativePath, method)))
            .Where(item => item.method.CyclomaticComplexity > MaxMethodCyclomaticComplexity)
            .Select(item =>
                $"{item.RelativePath}:{item.method.StartLine} {item.method.Name} (CC={item.method.CyclomaticComplexity})")
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        AssertNoViolations(violations, $"Each method/function must stay <= {MaxMethodCyclomaticComplexity} cyclomatic complexity.");
    }

    /// <summary>
    /// Xác nhận method không vượt ngưỡng cognitive complexity.
    /// Luồng này kiểm soát độ khó hiểu của logic lồng nhau.
    /// </summary>
    [Fact]
    public void Methods_ShouldStayWithinCognitiveComplexityBudget()
    {
        var metrics = AnalyzeSourceFiles();

        var violations = metrics
            .SelectMany(metric => metric.Methods.Select(method => (metric.RelativePath, method)))
            .Where(item => item.method.CognitiveComplexity > MaxMethodCognitiveComplexity)
            .Select(item =>
                $"{item.RelativePath}:{item.method.StartLine} {item.method.Name} (CogC={item.method.CognitiveComplexity})")
            .OrderBy(item => item, StringComparer.Ordinal)
            .ToArray();

        AssertNoViolations(violations, $"Each method/function must stay <= {MaxMethodCognitiveComplexity} cognitive complexity.");
    }

    /// <summary>
    /// Phân tích metrics cho toàn bộ file source C# trong `src`.
    /// Luồng bỏ qua file sinh tự động và migration theo policy kiểm tra.
    /// </summary>
    private static IReadOnlyList<FileMetrics> AnalyzeSourceFiles()
    {
        var backendRoot = FindBackendRoot();
        var sourceRoot = Path.Combine(backendRoot, "src");

        return Directory
            .GetFiles(sourceRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !ShouldSkip(path))
            .Select(path => AnalyzeSingleFile(path, backendRoot))
            .ToArray();
    }

    /// <summary>
    /// Phân tích metrics cho một file C# đơn lẻ.
    /// Luồng parse Roslyn tree, tính logical lines và metrics cho từng method/local function.
    /// </summary>
    private static FileMetrics AnalyzeSingleFile(string filePath, string backendRoot)
    {
        var text = File.ReadAllText(filePath);
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var root = syntaxTree.GetRoot();
        var sourceText = syntaxTree.GetText();
        var lineHasCode = BuildLogicalLineMap(root, sourceText, text);

        var methods = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(method => method.Body is not null || method.ExpressionBody is not null)
            .Select(method => CreateMethodMetrics(method, lineHasCode))
            .Concat(root
                .DescendantNodes()
                .OfType<LocalFunctionStatementSyntax>()
                .Where(function => function.Body is not null || function.ExpressionBody is not null)
                .Select(function => CreateMethodMetrics(function, lineHasCode)))
            .ToArray();

        var relativePath = ToBackendRelativePath(backendRoot, filePath);
        var logicalLines = lineHasCode.Count(hasCode => hasCode);
        return new FileMetrics(relativePath, logicalLines, methods);
    }

    /// <summary>
    /// Tạo MethodMetrics cho MethodDeclarationSyntax.
    /// Luồng gom đầy đủ thông tin tên, vị trí, tham số và độ phức tạp.
    /// </summary>
    private static MethodMetrics CreateMethodMetrics(
        MethodDeclarationSyntax method,
        IReadOnlyList<bool> lineHasCode)
    {
        var lineSpan = method.GetLocation().GetLineSpan();
        var logicalLines = CountMethodBodyLogicalLines(method.Body, method.ExpressionBody, lineHasCode);

        return new MethodMetrics(
            Name: method.Identifier.Text,
            StartLine: lineSpan.StartLinePosition.Line + 1,
            ParameterCount: CountRequiredParameters(method.ParameterList.Parameters),
            LogicalLines: logicalLines,
            CyclomaticComplexity: CalculateCyclomaticComplexity(method),
            CognitiveComplexity: CalculateCognitiveComplexity(method));
    }

    /// <summary>
    /// Tạo MethodMetrics cho LocalFunctionStatementSyntax.
    /// Luồng dùng cùng thước đo với method thường để đánh giá nhất quán.
    /// </summary>
    private static MethodMetrics CreateMethodMetrics(
        LocalFunctionStatementSyntax function,
        IReadOnlyList<bool> lineHasCode)
    {
        var lineSpan = function.GetLocation().GetLineSpan();
        var logicalLines = CountMethodBodyLogicalLines(function.Body, function.ExpressionBody, lineHasCode);

        return new MethodMetrics(
            Name: $"local::{function.Identifier.Text}",
            StartLine: lineSpan.StartLinePosition.Line + 1,
            ParameterCount: CountRequiredParameters(function.ParameterList.Parameters),
            LogicalLines: logicalLines,
            CyclomaticComplexity: CalculateCyclomaticComplexity(function),
            CognitiveComplexity: CalculateCognitiveComplexity(function));
    }

    /// <summary>
    /// Xây dựng bản đồ logical line có code cho toàn file.
    /// Luồng này loại trừ phần ký tự thuộc comment trước khi đếm.
    /// </summary>
    private static bool[] BuildLogicalLineMap(SyntaxNode root, SourceText sourceText, string rawText)
    {
        var commentCharacters = BuildCommentCharacterMap(root, rawText.Length);
        var lineHasCode = new bool[sourceText.Lines.Count];

        for (var lineIndex = 0; lineIndex < sourceText.Lines.Count; lineIndex++)
        {
            var lineSpan = sourceText.Lines[lineIndex].Span;
            for (var position = lineSpan.Start; position < lineSpan.End; position++)
            {
                if (position < commentCharacters.Length && commentCharacters[position])
                {
                    // Bỏ qua ký tự comment để chỉ đếm dòng có code thực.
                    continue;
                }

                if (!char.IsWhiteSpace(rawText[position]))
                {
                    lineHasCode[lineIndex] = true;
                    break;
                }
            }
        }

        return lineHasCode;
    }

    /// <summary>
    /// Đánh dấu vị trí ký tự thuộc comment trong source.
    /// Luồng này hỗ trợ loại bỏ comment khi tính logical lines.
    /// </summary>
    private static bool[] BuildCommentCharacterMap(SyntaxNode root, int sourceLength)
    {
        var commentCharacters = new bool[sourceLength];

        foreach (var trivia in root.DescendantTrivia(descendIntoTrivia: true))
        {
            if (!IsCommentTrivia(trivia.Kind()))
            {
                continue;
            }

            var span = trivia.Span;
            var end = Math.Min(span.End, sourceLength);
            for (var index = span.Start; index < end; index++)
            {
                commentCharacters[index] = true;
            }
        }

        return commentCharacters;
    }

    /// <summary>
    /// Kiểm tra trivia hiện tại có phải comment/doc comment không.
    /// Luồng này chuẩn hóa các loại comment cần loại khỏi thống kê code.
    /// </summary>
    private static bool IsCommentTrivia(SyntaxKind kind)
    {
        return kind is SyntaxKind.SingleLineCommentTrivia
            or SyntaxKind.MultiLineCommentTrivia
            or SyntaxKind.SingleLineDocumentationCommentTrivia
            or SyntaxKind.MultiLineDocumentationCommentTrivia
            or SyntaxKind.DocumentationCommentExteriorTrivia;
    }

    /// <summary>
    /// Đếm số dòng logical code trong một khoảng line index.
    /// Luồng xử lý an toàn biên đầu-cuối để tránh lỗi vượt mảng.
    /// </summary>
    private static int CountLogicalLinesInRange(IReadOnlyList<bool> lineHasCode, int startLine, int endLine)
    {
        if (startLine < 0 || endLine < 0 || startLine >= lineHasCode.Count)
        {
            return 0;
        }

        var normalizedEnd = Math.Min(endLine, lineHasCode.Count - 1);
        var count = 0;

        for (var index = startLine; index <= normalizedEnd; index++)
        {
            if (lineHasCode[index])
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Đếm logical lines cho thân method hoặc expression-bodied member.
    /// Luồng với expression body trả 1 dòng để phản ánh method có thực thi.
    /// </summary>
    private static int CountMethodBodyLogicalLines(
        BlockSyntax? body,
        ArrowExpressionClauseSyntax? expressionBody,
        IReadOnlyList<bool> lineHasCode)
    {
        if (body is not null)
        {
            var lineSpan = body.GetLocation().GetLineSpan();
            return CountLogicalLinesInRange(lineHasCode, lineSpan.StartLinePosition.Line, lineSpan.EndLinePosition.Line);
        }

        return expressionBody is null ? 0 : 1;
    }

    /// <summary>
    /// Đếm số tham số bắt buộc (không có default value).
    /// Luồng này dùng để enforce ngân sách độ phức tạp chữ ký hàm.
    /// </summary>
    private static int CountRequiredParameters(SeparatedSyntaxList<ParameterSyntax> parameters)
    {
        return parameters.Count(parameter => parameter.Default is null);
    }

    /// <summary>
    /// Tính cyclomatic complexity bằng cách cộng các điểm rẽ nhánh.
    /// Luồng đếm cả branch statement, switch labels và toán tử logic ngắn mạch.
    /// </summary>
    private static int CalculateCyclomaticComplexity(SyntaxNode node)
    {
        var complexity = 1;

        foreach (var descendant in node.DescendantNodes())
        {
            if (descendant is IfStatementSyntax
                or ForStatementSyntax
                or ForEachStatementSyntax
                or ForEachVariableStatementSyntax
                or WhileStatementSyntax
                or DoStatementSyntax
                or ConditionalExpressionSyntax
                or CatchClauseSyntax
                or SwitchExpressionArmSyntax)
            {
                complexity++;
            }

            if (descendant is BinaryExpressionSyntax binaryExpression
                && (binaryExpression.IsKind(SyntaxKind.LogicalAndExpression)
                    || binaryExpression.IsKind(SyntaxKind.LogicalOrExpression)))
            {
                complexity++;
            }

            if (descendant is SwitchSectionSyntax switchSection)
            {
                complexity += switchSection.Labels.Count;
            }
        }

        return complexity;
    }

    /// <summary>
    /// Tính cognitive complexity dựa trên flow node và độ lồng.
    /// Luồng cộng thêm điểm khi có nesting để phản ánh độ khó đọc thực tế.
    /// </summary>
    private static int CalculateCognitiveComplexity(SyntaxNode node)
    {
        var complexity = 0;

        foreach (var descendant in node.DescendantNodes())
        {
            if (IsCognitiveFlowNode(descendant))
            {
                var nesting = descendant.Ancestors().Count(IsCognitiveNestingNode);
                complexity += 1 + nesting;
            }

            if (descendant is BinaryExpressionSyntax binaryExpression
                && (binaryExpression.IsKind(SyntaxKind.LogicalAndExpression)
                    || binaryExpression.IsKind(SyntaxKind.LogicalOrExpression)))
            {
                complexity++;
            }
        }

        return complexity;
    }

    /// <summary>
    /// Xác định node được xem là nhánh điều khiển cho cognitive complexity.
    /// Luồng này gom các cấu trúc điều kiện/lặp/chuyển nhánh chính.
    /// </summary>
    private static bool IsCognitiveFlowNode(SyntaxNode node)
    {
        return node is IfStatementSyntax
            or SwitchSectionSyntax
            or ForStatementSyntax
            or ForEachStatementSyntax
            or ForEachVariableStatementSyntax
            or WhileStatementSyntax
            or DoStatementSyntax
            or CatchClauseSyntax
            or ConditionalExpressionSyntax
            or SwitchExpressionArmSyntax;
    }

    /// <summary>
    /// Xác định node góp phần tạo mức lồng khi tính cognitive complexity.
    /// Luồng này tách riêng với flow node để tính điểm nesting chính xác hơn.
    /// </summary>
    private static bool IsCognitiveNestingNode(SyntaxNode node)
    {
        return node is IfStatementSyntax
            or SwitchStatementSyntax
            or SwitchExpressionSyntax
            or ForStatementSyntax
            or ForEachStatementSyntax
            or ForEachVariableStatementSyntax
            or WhileStatementSyntax
            or DoStatementSyntax
            or CatchClauseSyntax
            or ConditionalExpressionSyntax;
    }

    /// <summary>
    /// Xác định file có nên bỏ qua khỏi phân tích hay không.
    /// Luồng bỏ qua migration/bin/obj và file generated để giảm nhiễu kết quả.
    /// </summary>
    private static bool ShouldSkip(string path)
    {
        if (path.Contains($"{Path.DirectorySeparatorChar}Migrations{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            return true;
        }

        if (path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            return true;
        }

        if (path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
        {
            return true;
        }

        var fileName = Path.GetFileName(path);
        return fileName.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith(".generated.cs", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Tìm thư mục gốc Backend từ thư mục output test hiện tại.
    /// Luồng duyệt ngược parent directories đến khi thấy đủ solution/src/tests.
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
    /// Luồng này giúp report vi phạm ngắn gọn và ổn định trên CI.
    /// </summary>
    private static string ToBackendRelativePath(string backendRoot, string fullPath)
    {
        return Path.GetRelativePath(backendRoot, fullPath).Replace('\\', '/');
    }

    /// <summary>
    /// Assert helper: fail test khi có vi phạm và in báo cáo chi tiết.
    /// Luồng trả sớm nếu không có vi phạm để giữ output test sạch.
    /// </summary>
    private static void AssertNoViolations(IReadOnlyList<string> violations, string rule)
    {
        if (violations.Count == 0)
        {
            // Không có vi phạm thì không cần tạo báo cáo thất bại.
            return;
        }

        var report = string.Join(Environment.NewLine, violations);
        Assert.Fail($"{rule}{Environment.NewLine}{report}");
    }

    private sealed record FileMetrics(
        string RelativePath,
        int LogicalLines,
        IReadOnlyList<MethodMetrics> Methods);

    private sealed record MethodMetrics(
        string Name,
        int StartLine,
        int ParameterCount,
        int LogicalLines,
        int CyclomaticComplexity,
        int CognitiveComplexity);
}
