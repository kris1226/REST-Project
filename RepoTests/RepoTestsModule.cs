using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTests
{
    public class RepoTestsModule : NinjectModule
    {
        private IDbConnection _db;

        public RepoTestsModule(IDbConnection db)
        {
            this._db = db;
        }
        public override void Load()
        {
            Bind<IAsyncRepository<CriteriaSets>>().To<CriteriaSetsRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<CriteriaDetails>>().To<CriteriaDetialsRepository>().WithConstructorArgument("db", _db);
        }
    }
}
