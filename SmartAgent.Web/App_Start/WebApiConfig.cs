using Owin;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiContrib.IoC.Ninject;

namespace SmartAgent.Web.App_Start {
    public class WebApiConfig {
        
        public static void Configure(IAppBuilder app) {      
            var config = new HttpConfiguration();

            // Web API configuration and services
            config.Formatters.Add(new BrowserJsonFormatter());

            //Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );

            config.DependencyResolver = new NinjectResolver(NinjectConfig.CreateKernel());
            app.UseWebApi(config);
        }
    }
}