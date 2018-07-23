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
        private static IISExpress web;
        private static IISExpress api;
        private static IISExpress idp;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            web = new IISExpress("Machete.Web", "4213");
            api = new IISExpress("Api", "63374");
            idp = new IISExpress("Identity", "63273");
            web.StartIis();
            api.StartIis();
            idp.StartIis();
        }

        [TestInitialize]
        public void TestInitialize() { }

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_MacheteWeb_Starts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:4213");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_API_Starts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:63374");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/api/configs");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_IDP_Starts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44379");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/id/.well-known/openid-configuration");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            web.StopIis();
            api.StopIis();
            idp.StopIis();
        }
    }
}
