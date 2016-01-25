using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using iAgentDataTool.Repositories.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TodoApp
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentProd"].ConnectionString);
                kernel.Bind<IAsyncRepository<ClientMaster>>().To<ClientMasterRepositoryAsync>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<FacilityMaster>>().To<FacilityMasterAsyncRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<FacilityDetail>>().To<FacilityDetialsAsyncRepository>().WithConstructorArgument("db", smartAgentDb);

                kernel.Bind<IAsyncRepository<CriteriaSets>>().To<CriteriaSetsRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<IAsyncRepository<CriteriaDetails>>().To<CriteriaDetialsRepository>().WithConstructorArgument("db", smartAgentDb);
            
                kernel.Bind<IAsyncRepository<ScriptMaster>>().To<ScriptMasterRepository>().WithConstructorArgument("db", smartAgentDb);
                kernel.Bind<ScriptRetrunValuesRepository>().ToSelf().WithConstructorArgument("db", smartAgentDb);
                return kernel;
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}