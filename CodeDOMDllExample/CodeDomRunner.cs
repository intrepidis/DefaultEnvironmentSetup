using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace CodeDOMExample
{
    public class CodeDomRunner
    {
        private static readonly string nl = Environment.NewLine;

        public void Go()
        {
            string targetName = "TestGraph";
            var provider = CodeDomProvider.CreateProvider(nameof(Microsoft.CSharp));
            string sourceFileName = Path.ChangeExtension(targetName, provider.FileExtension);
            Generate(provider, sourceFileName);
            Compile(provider, sourceFileName, targetName);
        }

        private string Generate(CodeDomProvider provider, string sourceFileName)
        {
            CodeCompileUnit compileUnit = CodeDomExample.MakeCompileUnit();

            // Create an IndentedTextWriter, constructed with a StreamWriter to the source file.
            var tw = new IndentedTextWriter(new StreamWriter(sourceFileName, false), "    ");

            // Generate source code using the code generator.
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());

            // Close the output file.
            tw.Close();

            // Return the generated source file content.
            StreamReader sr = new StreamReader(sourceFileName);
            string fileContent = sr.ReadToEnd();
            return fileContent;
        }

        private string Compile(CodeDomProvider provider, string sourceFileName, string targetName)
        {
            // Configure a CompilerParameters that links System.dll and produces the specified DLL file.
            string[] referenceAssemblies = { "System.dll" };
            var cp = new CompilerParameters(referenceAssemblies, $"{targetName}.dll", false);

            // Generate a DLL rather than an executable file.
            cp.GenerateExecutable = false;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFileName);

            string resultMessage;

            if (cr.Errors.Count == 0)
            {
                resultMessage = $"Successfully built \"{sourceFileName}\" source into assembly \"{cr.PathToAssembly}\".";
            }
            else
            {
                // Display compilation errors.
                resultMessage = $"Errors encountered while building \"{sourceFileName}\" into \"{cr.PathToAssembly}\": {nl}";
                foreach (CompilerError ce in cr.Errors)
                {
                    resultMessage += ce.ToString() + nl;
                }
            }

            return resultMessage;
        }
    }
}
