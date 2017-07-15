﻿using Elmah.Contrib.WebApi;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartupAttribute(typeof(Machete.Api.Startup))]
namespace Machete.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Allow all origins
            app.UseCors(CorsOptions.AllowAll);
            // Wire token validation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["IdentityProvider"], 

                // For access to the introspection endpoint
                ClientId = ConfigurationManager.AppSettings["IdentityClientId"],
                ClientSecret = ConfigurationManager.AppSettings["IdentityClientSecret"],

                RequiredScopes = new[] { "api" }
            });

            // Wire Web API
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            // catch all route mapped to ErrorController so 404 errors
            // can be logged in elmah
            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
            config.DependencyResolver = new UnityResolver(UnityConfig.Get());
            //UnityConfig.ConfigureUnity(config);
            //config.Filters.Add(new AuthorizeAttribute());
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            app.UseWebApi(config);
        }
    }
}