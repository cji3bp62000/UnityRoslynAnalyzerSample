using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace AnalyzerPlayground.Sample;

/// <summary>
/// [readonly] コメントをつけているローカル変数を、宣言以降に代入しているかをチェックする Analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ReadOnlyLocalVariableSyntaxAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "RA0003";

    private const string Category = "CodingFormat";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId,
        title: "readonly なローカル変数 {0} に代入しています",
        messageFormat: "readonly なローカル変数 {0} に代入しています",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    private static readonly string ReadonlyTag = "[readonly]";

    public override void Initialize(AnalysisContext context)
    {
        // 呪文のようなもの
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // フックする構文の種類を指定（if 構文）
        context.RegisterOperationAction(AnalyzeOperation, OperationKind.VariableDeclaration);
    }

    /// <summary>
    /// if 文のスペースチェック
    /// </summary>
    /// <param name="context"></param>
    private void AnalyzeOperation(OperationAnalysisContext context)
    {
        if (context.Operation is not IVariableDeclarationOperation {Syntax: VariableDeclarationSyntax variableDeclarationSyntax} op)
            return;
        if (context.ContainingSymbol is not IMethodSymbol method) return;

        var hasReadonlyTagComment = variableDeclarationSyntax.Type.GetLeadingTrivia()
            .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            .Any(trivia => trivia.ToString().AsSpan(2).Trim(' ').SequenceEqual(ReadonlyTag.AsSpan()));

        if (!hasReadonlyTagComment) return;

        // 代入チェック
        var localSymbols = op.GetDeclaredVariables();

        var assignmentSyntaxes = method.DeclaringSyntaxReferences
            .SelectMany(s =>s.SyntaxTree.GetRoot().DescendantNodes().OfType<AssignmentExpressionSyntax>());

        foreach (var syntax in assignmentSyntaxes) {
            if (op.SemanticModel.GetSymbolInfo(syntax.Left).Symbol is not ILocalSymbol localSymbol) continue;
            if (!localSymbols.Contains(localSymbol)) continue;

            // 報告する
            var diagnostic = Diagnostic.Create(Rule,
                syntax.GetLocation(), localSymbol.Name);

            context.ReportDiagnostic(diagnostic);

        }
    }
}