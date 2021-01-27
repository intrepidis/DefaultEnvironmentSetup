using System;
using System.Collections.Generic;
using System.IO;

namespace EnvironmentSettingsBuilder
{
    public class EnvironmentBuilderJockey
    {
        private readonly string[] imports = new[] // Imports (using assemblies)
        {
            "System",
            "DefaultEnvironmentSetup.Tools",
            "Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables",
        };

        private const string exampleIniFileContent = @"
[Config]
TypeName=DefaultSetup
BaseType=BaseDefaultSetup

[UserSupplied]
DriveLetter=E:

[Resolved]
Installations=%DriveLetter%\Client
LocalData=%Installations%\LocalData
RemotePersistentRoot=%CmnInstallations%\RemoteData\%CmnInstallations%\Junk""With\\$rap
";

        public void ConstructEnvironmentSet()
        {
            string targetName = "DefaultEnvironmentSetup"; // Namespace (the target assembly)
            string typeName = "DefaultSetup"; // Class name (the type name)
            string baseType = "BaseDefaultSetup"; // Parent class (name of the base type)

            var data = new Dictionary<string, string>();
            data["DriveLetter"] = "E:";
            data["Installations"] = @"%DriveLetter%\Client";
            data["LocalData"] = @"%Installations%\LocalData";
            data["RemotePersistentRoot"] = @"%CmnInstallations%\RemoteData\%CmnInstallations%\Junk""With\\$rap";

            var builder = new EnvironmentBuilder(imports, targetName, typeName, baseType);
            File.WriteAllText(typeName + ".cs", builder.MakeCode());
        }
    }
}
