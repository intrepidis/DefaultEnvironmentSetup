using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

// This example demonstrates building a Hello World program graph
// using System.CodeDom elements. It calls code generator and
// code compiler methods to build the program using CSharp, VB, or
// JScript.  A Windows Forms interface is included. Note: Code
// must be compiled and linked with the Microsoft.JScript assembly.
namespace CodeDOMExample
{
    class CodeDomExample
    {
        // Build a Hello World program graph using System.CodeDom types.
        public static CodeCompileUnit BuildHelloWorldGraph()
        {
            // Create a new CodeCompileUnit to contain the program graph.
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare a new namespace called Samples.
            CodeNamespace samples = new CodeNamespace("Samples");

            // Add the new namespace to the compile unit.
            compileUnit.Namespaces.Add(samples);

            // Add the new namespace import for the System namespace.
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // Declare a new type called Class1.
            CodeTypeDeclaration class1 = new CodeTypeDeclaration("Class1");

            // Add the new type to the namespace type collection.
            samples.Types.Add(class1);

            // Declare a new code entry point method.
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            // Create a type reference for the System.Console class.
            CodeTypeReferenceExpression csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.WriteLine statement.
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Hello World!"));

            // Add the WriteLine call to the statement collection.
            start.Statements.Add(cs1);

            // Build another Console.WriteLine statement.
            CodeMethodInvokeExpression cs2 = new CodeMethodInvokeExpression(csSystemConsoleType, "WriteLine", new CodePrimitiveExpression("Press the Enter key to continue."));

            // Add the WriteLine call to the statement collection.
            start.Statements.Add(cs2);

            // Build a call to System.Console.ReadLine.
            CodeMethodInvokeExpression csReadLine = new CodeMethodInvokeExpression(csSystemConsoleType, "ReadLine");

            // Add the ReadLine statement.
            start.Statements.Add(csReadLine);

            // Add the code entry point method to the Members collection of the type.
            class1.Members.Add(start);

            return compileUnit;
        }

        public static void GenerateCode(CodeDomProvider provider, CodeCompileUnit compileUnit, string sourceFile)
        {
            // Create an IndentedTextWriter, constructed with a StreamWriter to the source file.
            IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");

            // Generate source code using the code generator.
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());

            // Close the output file.
            tw.Close();
        }

        public static CompilerResults CompileCode(CodeDomProvider provider, string sourceFile, string exeFile)
        {
            // Configure a CompilerParameters that links System.dll and produces the specified executable file.
            string[] referenceAssemblies = { "System.dll" };
            CompilerParameters cp = new CompilerParameters(referenceAssemblies, exeFile, false);

            // Generate an executable rather than a DLL file.
            cp.GenerateExecutable = true;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);

            // Return the results of compilation.
            return cr;
        }
    }
}