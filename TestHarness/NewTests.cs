using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables;
using DefaultEnvironmentSettings;
using EnvironmentSettings;
//using EnvironmentSettings.EVT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//TODO: Add tests to check whether any of the strings have UserSuppliedValueProvider, DeploymentValueProvider or RequiredValueProvider in them.

namespace TestHarness
{
    class NewTests
    {
        //private static readonly string nl = Environment.NewLine;

        public static void Go()
        {
            var testClass = new NewTests();
            //testClass.StagingAppServer();
            testClass.DefaultSettings();
        }

        private void DefaultSettings()
        {
            //ConvertSettings(typeof(DefaultSettings), new MyDefaultSettings(), "DefaultSettings2", @"E:\EnvironmentSettings\DefaultEnvironmentSettings\");

            var sets = new MyDefaultSettings();
            ((DeploymentValueProvider)sets.AppServer).SetValue("a1");
            ((DeploymentValueProvider)sets.WebServer).SetValue("w1");
            string bankAccountDataDir = sets.CmnBankAccDetails_BankAccountDataDir.Value;
            string bankAccountDataDir2 = sets.CmnBankAccDetails_BankAccountDataDir + "";
            string PaymentSummaryURL = sets.PaymentSummaryURL.Value;
            string PaymentSummaryURL2 = sets.PaymentSummaryURL + "";
        }

        private static void ConvertSettings(Type inputType, object inputObject, string identifier, string outputPath)
        {
            string[] overrides = new[] {
                "CmnInstallations",
                "LocalCacheRoot",
                "LocalPersistentRoot",
                "RemotePersistentRoot" };

            string className = identifier + "_EnvironmentVariables";

            string[] header = new[] {
                "using System;",
                "using Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables;",
                "",
                "namespace DefaultEnvironmentSettings",
                "{",
                "    public abstract partial class " + className + @" : SettingsProvider",
                "    {",
                "        public " + className + "()",
                "            : base(\"" + identifier + "\")",
                "        {",
                "        }",
                "" };

            string[] footer = new[] { @"    }", "}" };

            string lineFormat0 = "        public%OVERRIDE% IEnvironmentValueProvider {0} => ProvideValue(%VALUE%);";
            string lineFormat1 = lineFormat0.Replace("%VALUE%", "() => @\"{1}\"");
            string lineFormat2 = lineFormat0.Replace("%VALUE%", "() => \"{1}\"");
            string lineFormat3 = lineFormat0.Replace("%VALUE%", string.Empty);

            var lines =
                from p in GetSettings(inputType, inputObject)
                where !"Identifier".Equals(p.Key, StringComparison.Ordinal)
                let hasBackSlashes = p.Value != null && p.Value.Contains("\\")
                let value = hasBackSlashes ? p.Value.Replace("\"", "\"\"") : p.Value ?? string.Empty
                let valueFormat = hasBackSlashes ? lineFormat1 : value.Length == 0 ? lineFormat3 : lineFormat2
                let overrideFormat = valueFormat.Replace("%OVERRIDE%", overrides.Contains(p.Key, StringComparer.Ordinal) ? " override" : string.Empty)
                select string.Format(overrideFormat, p.Key, value);

            string outputFilePath = Path.Combine(outputPath, className + ".cs");
            File.WriteAllLines(outputFilePath, header.Concat(lines).Concat(footer));
        }

        private static IEnumerable<KeyValuePair<string, string>> GetSettings(Type type, object obj)
        {
            foreach (var prop in
                from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                select p)
            {
                string name = prop.Name;
                string value;
                if (prop.PropertyType == typeof(string))
                {
                    value = (string)prop.GetValue(obj, null);
                }
                else if (prop.PropertyType == typeof(IEnvironmentValueProvider))
                {
                    var prov = (IEnvironmentValueProvider)prop.GetValue(obj, null);
                    value = prov == null ? string.Empty : prov.Value;
                }
                else
                {
                    value = string.Format("Unknown type: {0} with value to string: {1}", prop.PropertyType, prop.GetValue(obj, null));
                }
                yield return new KeyValuePair<string, string>(name, value);
            }
        }

        private void StagingAppServer()
        {
            //var env = new EVT_OriginationAppServer_EnvironmentVariables();

            //((UserSuppliedValueProvider)env.AppServer).SetValueByUser("TestAppServer");
            //((UserSuppliedValueProvider)env.WebServer).SetValueByUser("TestWebServer");

            //string s = env.PortalSettings.SiteVisa_AllowedRedirectTargets.Value;
            //Assert.AreEqual(s, "http://localhost;http://TestWebServer;http://testwebserver");
        }
    }

    internal class MyDefaultSettings : DefaultSettings
    {
        public MyDefaultSettings()
        {
            Identifier = "MyNewDefSets";
        }

        public sealed override IEnvironmentValueProvider CmnInstallationDrive => _(() => "E:");
    }
}
