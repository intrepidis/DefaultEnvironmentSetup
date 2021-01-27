using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DefaultEnvironmentSetup.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.CSharp.Formatting;

namespace EnvironmentSettingsBuilder
{
    public class EnvironmentBuilder
    {
        private readonly string[] imports;
        private readonly string targetName;
        private readonly string typeName;
        private readonly string baseType;
        private SyntaxGenerator gen;

        public EnvironmentBuilder(string[] imports, string targetName, string typeName, string baseType)
        {
            this.imports = imports;
            this.targetName = targetName;
            this.typeName = typeName;
            this.baseType = baseType;
        }

        public string MakeCode()
        {
            var _ = typeof(CSharpFormattingOptions);
            gen = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);

            var source =
                MakeImports(
                    MakeNamespace(
                        MakeType(
                            MakeConstructor())));

            string code =
                gen.CompilationUnit(source)
                .NormalizeWhitespace()
                .ToFullString();

            return code;
        }

        private SyntaxNode[] MakeImports(SyntaxNode ns)
        {
            return imports
                .Select(use => gen.NamespaceImportDeclaration(use))
                .Concat(new[] { ns })
                .ToArray();
        }

        private SyntaxNode MakeNamespace(SyntaxNode type)
        {
            var ns = gen.NamespaceDeclaration(targetName, type);
            return ns;
        }

        private SyntaxNode MakeType(SyntaxNode member)
        {
            var @base = SyntaxFactory.ParseTypeName(baseType);
            var type = gen.ClassDeclaration(typeName, baseType: @base, members: new[] { member });
            return type;
        }

        private SyntaxNode MakeConstructor()
        {
            //RemotePersistentRoot = _(() => CmnInstallationDrive.Value + "\RemoteData");
            var expre = gen.ValueReturningLambdaExpression(
                gen.AddExpression(
                    gen.AddExpression(
                        gen.MemberAccessExpression(gen.IdentifierName("CmnInstallations"), "Value"),
                        gen.LiteralExpression(@"\FmaBsl\LogScour\")),
                    gen.MemberAccessExpression(gen.IdentifierName("CmnInstallations"), "Value")));

            var assignment =
                gen.AssignmentStatement(
                    gen.IdentifierName("CmnInstallations"),
                    gen.InvocationExpression(
                        gen.MemberAccessExpression(gen.ThisExpression(), BaseDefaultSetup.DelegatedValueProviderBuildFunctionName),
                        expre));

            var assignments = new[] { assignment };

            var ctor = gen.ConstructorDeclaration(accessibility: Accessibility.Public, statements: assignments);
            return ctor;
        }
    }
}