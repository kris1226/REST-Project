[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TodoApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TodoApp.App_Start.NinjectWebCommon), "Stop")]

namespace TodoApp.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    using iAgentDataTool.Repositories.Common;
    using iAgentDataTool.Helpers.Interfaces;
    using iAgentDataTool.Models.Common;
    using Ninject.Web.WebApi;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                //Support Web Api
                //GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString);
            kernel.Bind<IAsyncRepository<ClientMaster>>().To<ClientMasterRepositoryAsync>().WithConstructorArgument("db", smartAgentDb);
            kernel.Bind<IAsyncRepository<FacilityMaster>>().To<FacilityMasterAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
            kernel.Bind<IAsyncRepository<FacilityDetail>>().To<FacilityDetialsAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
        }        
    }
}
