using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Repositories.Common;
using System.Data.SqlClient;
using System.Configuration;
using SmartAgentTool.Web.Controllers;

namespace SmartAgentTool.Web
{
    public static class Bootstrapper
    {
        public static string DevConfiguration { get { return "SmartAgentDev"; } }
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[DevConfiguration].ConnectionString);
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();  
            //container.RegisterType<IAsyncRepository<ClientMaster>, ClientMasterRepositoryAsync>(new InjectionConstructor(db));
           // container.RegisterType<IController, ClientsController>("Clients");

            return container;
        }

        
    }
}