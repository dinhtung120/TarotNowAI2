using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TarotNow.ArchitectureTests;

public class CodeQualityRulesTests
{
    private const int MaxLogicalFileLines = 100;
    private const int MaxMethodLines = 40;
    private const int MaxMethodParameters = 5;
    private const int MaxMethodCyclomaticComplexity = 10;

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
            CyclomaticComplexity: CalculateCyclomaticComplexity(method));
    }

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
            CyclomaticComplexity: CalculateCyclomaticComplexity(function));
    }

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

    private static bool IsCommentTrivia(SyntaxKind kind)
    {
        return kind is SyntaxKind.SingleLineCommentTrivia
            or SyntaxKind.MultiLineCommentTrivia
            or SyntaxKind.SingleLineDocumentationCommentTrivia
            or SyntaxKind.MultiLineDocumentationCommentTrivia
            or SyntaxKind.DocumentationCommentExteriorTrivia;
    }

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

    private static int CountRequiredParameters(SeparatedSyntaxList<ParameterSyntax> parameters)
    {
        return parameters.Count(parameter => parameter.Default is null);
    }

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

    private static void AssertNoViolations(IReadOnlyList<string> violations, string rule)
    {
        if (violations.Count == 0)
        {
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
        int CyclomaticComplexity);
}
