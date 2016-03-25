using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.Common;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using iAgentDataTool.Repositories;
using iAgentDataTool.AsyncRepositories.Common;
using iAgentDataTool.Repositories.SmartAgentRepos;

namespace SmartAgent.Web.App_Start
{
    public class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentProd"].ConnectionString);

                kernel.Bind<ISmartAgentRepo>().To<CreateSmartAgentUserRepo>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IUpwAsyncRepository>().To<UpwAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<Upw>>().To<UpwAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<ClientMaster>>().To<ClientMasterRepositoryAsync>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<ClientLocations>>().To<ClientLocationsAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<WebsiteMaster>>().To<WebsiteMasterAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<FacilityMaster>>().To<FacilityMasterAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<FacilityDetail>>().To<FacilityDetialsAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<PayerWebsiteMappingValue>>().To<PayerWebsiteMappingValuesAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<CriteriaSets>>().To<CriteriaSetsRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<CriteriaDetails>>().To<CriteriaDetialsRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<ScriptMaster>>().To<ScriptMasterRepository>().WithConstructorArgument("db", smartAgentDb);


                return kernel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}