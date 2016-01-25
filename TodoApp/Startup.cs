using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.SelfHost;
using System.Web.Http;
using TodoApp.App_Start;
using WebApiContrib.IoC.Ninject;

namespace TodoApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string uri = @"http://localhost:8080";
            var config = new HttpSelfHostConfiguration(uri);

   

            // Web API configuration and services
            config.Formatters.Add(new BrowserJsonFormatter());


            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
            );

            // Web API routes
            config.MapHttpAttributeRoutes();
            // config.EnsureInitialized();
            config.DependencyResolver = new NinjectResolver(NinjectConfig.CreateKernel());

            app.UseWebApi(config);
        }
    }
}