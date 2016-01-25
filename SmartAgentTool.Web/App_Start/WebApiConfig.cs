using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using SmartAgentTool.Web.App_Start;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.Common;
using iAgentDataTool.AsyncRepositories.Common;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using iAgentDataTool.Repositories.SmartAgentRepos;

namespace SmartAgentTool.Web
{
    public static class WebApiConfig
    {
        public static string DevConfiguration { get { return "SmartAgentDev"; } }
        public static void Register(HttpConfiguration config)
        {
            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[DevConfiguration].ConnectionString);

            // Web API configuration and services
            config.Formatters.Add(new BrowserJsonFormatter());

            var container = new UnityContainer();
            
            container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<ClientMaster>, ClientMasterRepositoryAsync>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<WebsiteMaster>, WebsiteMasterAsyncRepository>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<CriteriaDetails>, CriteriaDetialsRepository>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<CriteriaSets>, CriteriaSetsRepository>(new InjectionConstructor(db));
            container.RegisterType<ISmartAgentRepo, CreateSmartAgentUserRepo>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<FacilityMaster>, FacilityMasterAsyncRepository>(new InjectionConstructor(db));
            container.RegisterType<IAsyncRepository<FacilityDetail>, FacilityDetialsAsyncRepository>(new InjectionConstructor(db));
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
