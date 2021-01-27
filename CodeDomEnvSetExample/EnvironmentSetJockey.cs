using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace CodeDOMExample
{
    public class EnvironmentSetJockey
    {
        private readonly string[] imports = new[]
        {
            "System",
            "DefaultEnvironmentSetup.Model",
            "Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables",
        };

        private readonly string[] dependencies = new[]
        {
            "System.dll",
            "DefaultEnvironmentSetup.Model.dll",
            "Cmn.AppSetup.ConfigurationManager.Model.dll",
        };

        public void ConstructEnvironmentSet()
        {
            string targetName = "DefaultEnvironmentSetup";
            string typeName = "DefaultSetup";
            string baseType = "BaseDefaultSetup";

            var maker = new EnvironmentSetMaker(imports, targetName, typeName, baseType);
            CodeCompileUnit compileUnit = maker.MakeCode();
            AssemblyCompiler.Build(compileUnit, targetName, dependencies);
        }
    }
}
