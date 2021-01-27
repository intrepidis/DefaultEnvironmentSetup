using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace EnvironmentSettingsBuilder
{
    public class EnvironmentSetJockey
    {
        private readonly string[] imports = new[]
        {
            "System",
            "DefaultEnvironmentSetup.Tools",
            "Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables",
        };

        private readonly string[] dependencies = new[]
        {
            "System.dll",
            "DefaultEnvironmentSetup.dll",
            "Cmn.AppSetup.ConfigurationManager.Model.dll",
        };

        public void ConstructEnvironmentSet()
        {
            string targetName = "EnvironmentSetup";
            string typeName = "DefaultSetup";
            string baseType = "BaseDefaultSetup";

            var maker = new EnvironmentSetMaker(imports, targetName, typeName, baseType);
            CodeCompileUnit compileUnit = maker.MakeCode();
            AssemblyCompiler.Build(compileUnit, targetName, dependencies);
        }
    }
}
