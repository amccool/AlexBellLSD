using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AlexBellLSD
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NoRethrowAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RB0111";
        internal static readonly LocalizableString Title = "NoRethrowAnalyzer";
        internal static readonly LocalizableString MessageFormat = "Try catch block with no rethrow";
        internal const string Category = "Design";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeCatchClause, SyntaxKind.CatchClause);
        }

        private void AnalyzeCatchClause(SyntaxNodeAnalysisContext obj)
        {
            var catchClauseSyntaxNode = (CatchClauseSyntax) obj.Node;

            var catchClauseBlock = catchClauseSyntaxNode.Block;

            var statementsInCatchClauseBlock = catchClauseBlock.Statements;

            foreach (var statement in statementsInCatchClauseBlock)
            {
                if (statement is ThrowStatementSyntax)
                {
                    return;
                }
            }

            var diagnostic = Diagnostic.Create(Rule, catchClauseSyntaxNode.GetLocation());
            obj.ReportDiagnostic(diagnostic);
        }
    }
}