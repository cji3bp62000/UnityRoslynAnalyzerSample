using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using UnityEngine;

namespace AnalyzerPlayground.Sample;

/// <summary>
/// マテリアルの破棄を行っているかをチェックする Analyzer
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MaterialLeakSemanticAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "RA0001";

    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId,
        title: "Material の破棄を行っていません",
        messageFormat: "Material の破棄を行っていません",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // 代入構文のフック　（代入構文を起点に、）
        context.RegisterOperationAction(AnalyzeOperation, OperationKind.SimpleAssignment);
    }

    /// <summary>
    /// マテリアルの破棄を行っているかをチェックする関数
    /// </summary>
    /// <param name="context"></param>
    private void AnalyzeOperation(OperationAnalysisContext context)
    {
        if (context.Operation is not ISimpleAssignmentOperation simpleAssignmentOperation ||
            context.Operation.Syntax is not AssignmentExpressionSyntax assignmentSyntax)
            return;

        var opContainingClass = context.ContainingSymbol.ContainingType;

        // 代入構文の左辺と右辺を取得
        var left = simpleAssignmentOperation.Target;
        var right = simpleAssignmentOperation.Value;

        // 左辺がクラスの変数か、チェック
        if (left is not IFieldReferenceOperation fieldRefOp || !SymbolEqualityComparer.Default.Equals(opContainingClass, fieldRefOp.Field.ContainingType))
            return;

        // 右辺が new Material() の構文か、チェック
        if (right is not IObjectCreationOperation { Constructor.ContainingType.Name: nameof(Material) })
            return;

        // OnDisable か OnDestroy で Destroy しているか、チェック
        bool isMaterialDestroyed = false;
        var checkMethodItr = opContainingClass.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(symbol => symbol.Name is nameof(Behaviour.OnDisable) or nameof(Object.OnDestroy));

        foreach (var checkMethod in checkMethodItr) {
            // 関数実行構文を取得
            var methodBody = checkMethod.DeclaringSyntaxReferences[0].GetSyntax().DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocation in methodBody) {
                // Destroy か DestroyImmediate の関数で、中身が Material のフィールド名と一致しているか
                if (invocation.Expression is IdentifierNameSyntax { Identifier: { ValueText: nameof(Object.Destroy) or nameof(Object.DestroyImmediate) } } &&
                    invocation.ArgumentList.Arguments[0].Expression is IdentifierNameSyntax identifierNameSyntax && identifierNameSyntax.Identifier.ValueText == fieldRefOp.Field.Name) {
                    isMaterialDestroyed = true;
                    break;
                }
            }
        }

        if (isMaterialDestroyed) return;

        var diagnostic = Diagnostic.Create(Rule, assignmentSyntax.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }
}