using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace AnalyzerPlayground.Sample;

/// <summary>
/// if 文のスペースチェックをする Analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class IfWhiteSpaceSyntaxAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "RA0002";

    private const string Category = "CodingFormat";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId,
        title: "if 文のスペースを 1 個ずつで、改行しないでください",
        messageFormat: "if 文のスペースを 1 個ずつで、改行しないでください",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // 呪文のようなもの
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // フックする構文の種類を指定（if 構文）
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.IfStatement);
    }

    /// <summary>
    /// if 文のスペースチェック
    /// </summary>
    /// <param name="context"></param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IfStatementSyntax ifStatementSyntax)
            return;

        var ifKeyword = ifStatementSyntax.IfKeyword;

        // スペースが一個ずつなら報告しない
        if (IsOneWhitespaceTrivia(ifKeyword.TrailingTrivia)
            && IsOneWhitespaceTrivia(ifStatementSyntax.CloseParenToken.TrailingTrivia)) return;

        // 報告する
        var textSpan = new TextSpan(ifKeyword.SpanStart, ifStatementSyntax.Statement.SpanStart - ifKeyword.SpanStart);
        var diagnostic = Diagnostic.Create(Rule,
            ifStatementSyntax.SyntaxTree.GetLocation(textSpan));

        context.ReportDiagnostic(diagnostic);
    }

    private bool IsOneWhitespaceTrivia(in SyntaxTriviaList triviaList)
    {
        return triviaList.Count == 1
               && triviaList[0].IsKind(SyntaxKind.WhitespaceTrivia)
               && triviaList[0].Span.Length == 1;
    }

    /*
    private void AnalyzeIfWhitespaceSample(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IfStatementSyntax ifStatementSyntax)
            return;

        // if のあと、かっこの前の文字列の取得
        var triviaList = ifStatementSyntax.IfKeyword.TrailingTrivia;

        // ホワイトスペースのみの文字列で、長さが 1 でなければ...
        if (triviaList.Count != 1
            || !triviaList[0].IsKind(SyntaxKind.WhitespaceTrivia)
            || triviaList[0].Span.Length != 1) {
            // この if 文のソースコードの箇所に対して、報告
            context.ReportDiagnostic(Diagnostic.Create(Rule, ifStatementSyntax.GetLocation()));
        }
    }
    */
}