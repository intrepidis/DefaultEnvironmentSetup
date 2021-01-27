using DefaultEnvironmentSetup.Model;
using System;
using System.CodeDom;
using System.Linq;

namespace CodeDOMExample
{
    public class EnvironmentSetMaker
    {
        private readonly string[] imports;
        private readonly string targetName;
        private readonly string typeName;
        private readonly string baseType;

        public EnvironmentSetMaker(string[] imports, string targetName, string typeName, string baseType)
        {
            this.imports = imports;
            this.targetName = targetName;
            this.typeName = typeName;
            this.baseType = baseType;
        }

        public CodeCompileUnit MakeCode()
        {
            CodeTypeMember member = MakeConstructor();
            CodeTypeDeclaration type = MakeType(typeName, baseType, member);
            CodeNamespace ns = MakeNamespace(targetName, imports, type);

            var cu = new CodeCompileUnit();
            cu.Namespaces.Add(ns);
            return cu;
        }

        private static CodeNamespace MakeNamespace(string @namespace, string[] imports, CodeTypeDeclaration type)
        {
            var ns = new CodeNamespace(@namespace);
            ns.Imports.AddRange(imports.Select(i => new CodeNamespaceImport(i)).ToArray());
            ns.Types.Add(type);
            return ns;
        }

        private static CodeTypeDeclaration MakeType(string typeName, string baseType, CodeTypeMember member)
        {
            var c = new CodeTypeDeclaration(typeName);
            c.BaseTypes.Add(baseType);
            c.Members.Add(member);
            return c;
        }

        private static CodeConstructor MakeConstructor()
        {
            // Declare a new code entry point method.
            var ctor = new CodeConstructor { Attributes = MemberAttributes.Public };

            // Create a type reference for this class.
            var @this = new CodeThisReferenceExpression();
            
            // Set a member property value.
            var p1 = new CodePropertyReferenceExpression(@this, "CmnInstallations");

            //ServicingServer = _(() => $@"{AppServer.Value}");
            //this.ServicingServer = this._((Func<string>)(() => this.AppServer.Value ?? ""));
            //ServicingServer = _(be);
            //string be()
            //{
            //    return string.Format("{0}", AppServer.Value);
            //}
            var e1 = new CodePrimitiveExpression(@"$@""{AppServer.Value}""");
            //var l1 = new CodeMethodReferenceExpression(@this, $"() => {e1}");
            var l1 = new CodeSnippetExpression(@"() => CmnInstallations.Value + @""\FmaBsl\LogScour\"" + CmnInstallations.Value");
            var v1 = new CodeMethodInvokeExpression(@this, BaseDefaultSetup.DelegatedValueProviderBuildFunctionName, l1);
            //var funcString = new CodeTypeReference("System.Func", new CodeTypeReference("System.String"));
            //var m1f = new CodeMemberMethod { ReturnType = new CodeTypeReference("System.String") };
            //var returnExpression = @"$@""{AppServer.Value}""";
            //m1f.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(returnExpression)));
            //ctor.
            //var m1e = new CodeMethodReferenceExpression();
            //var v1 = new CodeMethodInvokeExpression(@this, BaseDefaultSetup.DelegatedValueProviderBuildFunctionName, m1e);

            var a1 = new CodeAssignStatement(p1, v1);

            ctor.Statements.Add(a1);


            // Create a type reference for the System.Console class.
            var csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.WriteLine statement.
            var cs1 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Hello World!"));

            // Add the WriteLine call to the statement collection.
            ctor.Statements.Add(cs1);

            // Build another Console.WriteLine statement.
            var cs2 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Press the Enter key to continue."));

            // Add the WriteLine call to the statement collection.
            ctor.Statements.Add(cs2);

            // Build a call to System.Console.ReadLine.
            var csReadLine = new CodeMethodInvokeExpression(csSystemConsoleType, "ReadLine");

            // Add the ReadLine statement.
            ctor.Statements.Add(csReadLine);

            return ctor;
        }
    }
}