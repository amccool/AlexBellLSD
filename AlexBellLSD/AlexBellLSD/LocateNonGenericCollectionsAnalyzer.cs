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
    public class LocateNonGenericCollectionsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RB0111";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Design";

        private static DiagnosticDescriptor Rule = 
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, 
                DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(Rule); 
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            //context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.TypeKeyword);
            context.RegisterCompilationStartAction(AnalyzeArrayList);



        }

        private static void AnalyzeArrayList(CompilationStartAnalysisContext compilationContext)
        {
            var arrayListType = compilationContext.Compilation.GetTypeByMetadataName("System.Collections.ArrayList");

            compilationContext.RegisterSyntaxNodeAction(syntaxContext =>
            {
                var variableTypeInfo = syntaxContext.SemanticModel.GetTypeInfo(syntaxContext.Node).Type as INamedTypeSymbol;

                if (variableTypeInfo == null)
                    return;

                if (variableTypeInfo.Equals(arrayListType))
                {
                    var desc = new DiagnosticDescriptor("RB001", "ArrayListWarning",
                        "Change {0} from ArrayList to Generic collection", "Design",
                        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "desc");
                    
                        syntaxContext.ReportDiagnostic(Diagnostic.Create(Rule
                        , syntaxContext.Node.GetLocation()));
                }
            }, SyntaxKind.ObjectCreationExpression);



            compilationContext.RegisterSyntaxNodeAction(syntaxContext =>
            {
                var variableTypeInfo = syntaxContext.SemanticModel.GetTypeInfo(syntaxContext.Node).Type as INamedTypeSymbol;

                if (variableTypeInfo == null)
                    return;

                if (variableTypeInfo.Equals(arrayListType))
                {
                    var desc = new DiagnosticDescriptor("RB001", "ArrayListWarning",
                        "Change {0} from ArrayList to Generic collection", "Design",
                        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "desc");

                    syntaxContext.ReportDiagnostic(Diagnostic.Create(Rule
                    , syntaxContext.Node.GetLocation()));
                }
            }, SyntaxKind.PropertyDeclaration);


            compilationContext.RegisterSyntaxNodeAction(syntaxContext =>
            {
                var variableTypeInfo = syntaxContext.SemanticModel.GetTypeInfo(syntaxContext.Node).Type as INamedTypeSymbol;

                if (variableTypeInfo == null)
                    return;

                if (variableTypeInfo.Equals(arrayListType))
                {
                    var desc = new DiagnosticDescriptor("RB001", "ArrayListWarning",
                        "Change {0} from ArrayList to Generic collection", "Design",
                        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "desc");

                    syntaxContext.ReportDiagnostic(Diagnostic.Create(Rule
                    , syntaxContext.Node.GetLocation()));
                }
            }, SyntaxKind.FieldDeclaration);



        }



        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            //var arrayListType = compilationContext.Compilation.GetTypeByMetadataName("System.Collections.ArrayList");

            var typeInfo = context.SemanticModel.GetTypeInfo(context.Node);

            //if (typeInfo.Equals(arrayListType))
            //typeInfo.Type.
            if (typeInfo.Type.Kind == SymbolKind.ArrayType )
            {

                var diagnostic = Diagnostic.Create(Rule, context.Node.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
