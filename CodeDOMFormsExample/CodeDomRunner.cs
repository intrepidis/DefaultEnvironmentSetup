using System;
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
            CodeDomExample.GenerateCode(provider, CodeDomExample.BuildHelloWorldGraph(), sourceFileName);

            // Return the generated source file content.
            StreamReader sr = new StreamReader(sourceFileName);
            string fileContent = sr.ReadToEnd();
            return fileContent;
        }

        private string Compile(CodeDomProvider provider, string sourceFileName, string targetName)
        {
            // Compile the source file into an executable output file.
            CompilerResults cr = CodeDomExample.CompileCode(provider, sourceFileName, $"{targetName}.exe");

            string resultMessage;

            if (cr.Errors.Count == 0)
            {
                resultMessage = $"Successfully built \"{sourceFileName}\" source into executable \"{cr.PathToAssembly}\".";
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
