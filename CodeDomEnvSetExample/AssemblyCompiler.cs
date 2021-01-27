using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

namespace CodeDOMExample
{
    public class AssemblyCompiler
    {
        private static readonly string nl = Environment.NewLine;
        private const string tab = "    ";

        public static void Build(CodeCompileUnit compileUnit, string targetName, string[] dependencies)
        {
            var provider = CodeDomProvider.CreateProvider(nameof(Microsoft.CSharp));
            string sourceFileName = Path.ChangeExtension(targetName, provider.FileExtension);
            GenerateClassFile(provider, compileUnit, sourceFileName);
            CompileAssemblyDll(provider, sourceFileName, targetName, dependencies);
        }

        private static void GenerateClassFile(CodeDomProvider provider, CodeCompileUnit compileUnit, string sourceFileName)
        {
            // Generate source code using the code generator.
            var tw = new IndentedTextWriter(new StreamWriter(sourceFileName, false), tab);
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            tw.Close();
        }

        private static void CompileAssemblyDll(CodeDomProvider provider, string sourceFileName, string targetName, string[] dependencies)
        {
            // Link with reference assemblies and produce the specified DLL file.
            var cp = new CompilerParameters(dependencies, $"{targetName}.dll", false)
            {
                GenerateExecutable = false,
            };
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFileName);
            if (cr.Errors.Count > 0)
            {
                var errors = cr.Errors.Cast<CompilerError>().Select(err => err.ToString());
                throw new CompilationException(sourceFileName, cr.PathToAssembly, errors.ToArray());
            }
        }
    }
}
