using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using Microsoft.Owin.Security;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;

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

        private static string idpEndpoint;
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // accept invalid certs
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            web = new IISExpress("Machete.Web", "4214");
            api = new IISExpress("Api", "63374");
            idp = new IISExpress("Identity", "63273");
            web.StartIis();
            api.StartIis();
            idp.StartIis();
            idpEndpoint = "https://localhost:44379/id";
        }

        [TestInitialize]
        public void TestInitialize() { }

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_MacheteWeb_Starts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:4214");
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
            //client.BaseAddress = new Uri("");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("https://localhost:44379/id/.well-known/openid-configuration");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //[TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        //public async Task E2E_Client_authN_succeeds()
        //{
        //    var client = new HttpClient();

        //    var disco = await client.GetDiscoveryDocumentAsync(idpEndpoint);
        //    if (disco.IsError) throw new Exception(disco.Error);

        //    var tokenEndpoint = disco.TokenEndpoint;
        //    var keys = disco.KeySet.Keys;

        //    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    {
        //        Address = disco.TokenEndpoint,

        //        ClientId = "tests",
        //        ClientSecret = "foo",
        //        Scope = "api"
        //    });

        //    if (response.IsError) throw new Exception(response.Error);
        //    var token = response.AccessToken;
        //    var jwt = new JwtSecurityToken(token);
        //    Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
        //    Assert.IsNotNull(token);
        //    Assert.AreEqual("Bearer", response.TokenType);
        //}

        [TestMethod, TestCategory(TC.E2E), TestCategory(TC.Controller), TestCategory(TC.TestHarness)]
        public async Task E2E_username_and_password_succeeds()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(idpEndpoint);
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenEndpoint = disco.TokenEndpoint;

            var response = await client.RequestPasswordTokenAsync(
                new PasswordTokenRequest() {
                    Address = disco.TokenEndpoint,
                    UserName = "jadmin",
                    Password = "ChangeMe",
                    ClientId = "tests",
                    ClientSecret = "foo",
                    Scope = "api"
                });

            if (response.IsError) throw new Exception(response.Error);
            var token = response.AccessToken;
            var jwt = new JwtSecurityToken(token);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = await client.GetAsync("http://localhost:63374/api/reports");
            var content = await response2.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            Assert.IsNotNull(content);
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            web.StopIis();
            api.StopIis();
            idp.StopIis();
        }
    }
}
