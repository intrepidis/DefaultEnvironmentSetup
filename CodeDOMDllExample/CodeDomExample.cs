using System;
using System.CodeDom;

namespace CodeDOMExample
{
    class CodeDomExample
    {
        public static CodeCompileUnit MakeCompileUnit()
        {
            var cu = new CodeCompileUnit();
            cu.Namespaces.Add(MakeNamespace());
            return cu;
        }

        private static CodeNamespace MakeNamespace()
        {
            var ns = new CodeNamespace("Samples");
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Types.Add(MakeClass());
            return ns;
        }

        private static CodeTypeDeclaration MakeClass()
        {
            var c = new CodeTypeDeclaration("Class1");
            c.Members.Add(MakeMethod());
            return c;
        }

        private static CodeEntryPointMethod MakeMethod()
        {
            // Declare a new code entry point method.
            var method = new CodeEntryPointMethod();

            // Create a type reference for the System.Console class.
            var csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.WriteLine statement.
            var cs1 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Hello World!"));

            // Add the WriteLine call to the statement collection.
            method.Statements.Add(cs1);

            // Build another Console.WriteLine statement.
            var cs2 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Press the Enter key to continue."));

            // Add the WriteLine call to the statement collection.
            method.Statements.Add(cs2);

            // Build a call to System.Console.ReadLine.
            var csReadLine = new CodeMethodInvokeExpression(csSystemConsoleType, "ReadLine");

            // Add the ReadLine statement.
            method.Statements.Add(csReadLine);

            return method;
        }
    }
}