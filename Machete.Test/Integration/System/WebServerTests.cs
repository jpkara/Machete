using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

namespace Machete.Test.Integration.System
{
    [TestClass]
    public class WebServerTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        //private sharedUI ui;
        private FluentRecordBase frb;
        //private static IMapper map;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            WebServer.StartIis();
        }

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_WebServer_Starts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:4213");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [ClassCleanup]
        public static void ClassCleanup() { WebServer.StopIis(); }
    }
}
