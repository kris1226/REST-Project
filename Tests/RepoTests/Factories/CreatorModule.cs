﻿using iAgentDataTool.AsyncRepositories.Common;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using iAgentDataTool.Repositories.Common;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.Repositories.SmartAgentRepos;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCreator;

namespace RepoTests
{
    public class CreatorModule : NinjectModule
    {
        private IDbConnection _db;

        public CreatorModule(IDbConnection db)
        {
            this._db = db;
        }
        public override void Load()
        {
            Bind<IClientCreator>().To<SmartAgentClientCreator1>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<WebsiteMaster>>().To<WebsiteMasterAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<ISmartAgentRepository>().To<SmartAgentRepo>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<ClientMaster>>().To<ClientMasterRepositoryAsync>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<ClientLocations>>().To<ClientLocationsAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<PayerWebsiteMappingValue>>().To<PayerWebsiteMappingValuesAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<FacilityMaster>>().To<FacilityMasterAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<FacilityDetail>>().To<FacilityDetialsAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<CriteriaSets>>().To<CriteriaSetsRepository>().WithConstructorArgument("db", _db);
            Bind<IAsyncRepository<CriteriaDetails>>().To<CriteriaDetialsRepository>().WithConstructorArgument("db", _db);
        }
    }
}
