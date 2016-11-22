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
    public class NonGenericCollectionsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RB0099";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Design";

        private static DiagnosticDescriptor PropertyRule = 
            new DiagnosticDescriptor(DiagnosticId, "property", "{0} is a non-generic collection", Category,
                DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private static DiagnosticDescriptor FieldRule = 
            new DiagnosticDescriptor(DiagnosticId, "field", "{0} is a non-generic collection", Category, 
                DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private static DiagnosticDescriptor ClassRule =
            new DiagnosticDescriptor(DiagnosticId, "class", "{0} is a non-generic collection", Category,
                DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(PropertyRule, FieldRule, ClassRule);

        public static List<INamedTypeSymbol> UnwantedCollectionTypes { get; private set; }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(SetupUnWantedCollectionTypes);

            context.RegisterSyntaxNodeAction(AnalyzeFieldNode, SyntaxKind.FieldDeclaration);

            context.RegisterSyntaxNodeAction(AnalyzePropertyNode, SyntaxKind.PropertyDeclaration);

            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.ClassDeclaration);


        }

        private static void SetupUnWantedCollectionTypes(CompilationStartAnalysisContext compilationContext)
        {
            UnwantedCollectionTypes = new List<INamedTypeSymbol>();
            var arrayListType = compilationContext.Compilation.GetTypeByMetadataName("System.Collections.ArrayList");

            UnwantedCollectionTypes.Add(arrayListType);
        }


        private void AnalyzePropertyNode(SyntaxNodeAnalysisContext context)
        {
            var propertyDeclarationSyntax = (PropertyDeclarationSyntax) context.Node;

            var variableTypeInfo = context.SemanticModel.GetTypeInfo(context.Node).Type as INamedTypeSymbol;

            if (variableTypeInfo == null)
                return;

            if (UnwantedCollectionTypes.Contains(variableTypeInfo))
            {
                var variableName = propertyDeclarationSyntax.Identifier.ValueText;
                var diagnostic = Diagnostic.Create(PropertyRule, propertyDeclarationSyntax.GetLocation(), variableName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeFieldNode(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclarationNode = (FieldDeclarationSyntax) context.Node;
            var variableTypeInfo = context.SemanticModel.GetTypeInfo(context.Node).Type as INamedTypeSymbol;

            if (variableTypeInfo == null)
                return;

            if (UnwantedCollectionTypes.Contains(variableTypeInfo))
            {
                var variableName = fieldDeclarationNode.Declaration.Variables.First().Identifier.ValueText;
                var diagnostic = Diagnostic.Create(FieldRule, fieldDeclarationNode.GetLocation(), variableName);
                context.ReportDiagnostic(diagnostic);
            }
        }


        private void AnalyzeClassNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclarationNode = (ClassDeclarationSyntax) context.Node;

            if (classDeclarationNode.BaseList == null)
                return;


            foreach (var baseType in classDeclarationNode.BaseList.Types)
            {
                var variableTypeInfo = context.SemanticModel.GetTypeInfo(baseType).Type as INamedTypeSymbol;

                if (variableTypeInfo == null)
                    return;

                if (UnwantedCollectionTypes.Contains(variableTypeInfo))
                {
                    var variableName = classDeclarationNode.Identifier.ValueText;
                    var diagnostic = Diagnostic.Create(FieldRule, classDeclarationNode.GetLocation(), variableName);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
