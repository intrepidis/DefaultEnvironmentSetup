using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cmn.AppSetup.ConfigurationManager.Model.EnvironmentVariables;
//using EnvironmentSettings.EVT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestHarness
{
    class OldTests
    {
        //public static void Go()
		//{
		//	var testClass = new OldTests();
		//	testClass.EvtAppServer();
		//	testClass.EvtWebServer();
		//}

		//private void EvtAppServer()
		//{
		//	var env = new Test_OriginationAppServer_EnvironmentVariables();

		//	((UserSuppliedValueProvider)env.AppServer).SetValueByUser("TestAppServer");
		//	((UserSuppliedValueProvider)env.WebServer).SetValueByUser("TestWebServer");

		//	string s = env.PortalSettings.SiteVisa_AllowedRedirectTargets.Value;
		//	Assert.AreEqual(s, string.Empty);
		//}

		//private void EvtWebServer()
		//{
		//	var env = new Test_OriginationWebServer_EnvironmentVariables();

		//	((UserSuppliedValueProvider)env.AppServer).SetValueByUser("TestAppServer");
		//	((UserSuppliedValueProvider)env.WebServer).SetValueByUser("TestWebServer");

		//	string s = env.AppServer.Value;

		//	var email = env.Email;
		//	s = email.GroupName;
		//	s = email.EmailServer.Value;
		//	s = email.EmailsTo.Value;
		//	s = email.EmailsFrom.Value;
		//	s = email.EmailFrom;
		//	s = email.ErrorEmailsSmtpServer;
		//	s = email.ErrorEmailsTo;
		//	s = email.ErrorEmailsFrom;
		//	s = email.BusinessEmailsTo;
		//	s = email.BusinessEmailsFrom;

		//	s = env.PortalSettings.SiteVisa_AllowedRedirectTargets.Value;
		//	Assert.AreEqual(s, "http://localhost;http://TestWebServer;http://testwebserver");
		//}
	}
}
