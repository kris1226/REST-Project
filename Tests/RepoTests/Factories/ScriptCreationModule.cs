using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Repositories.SmartAgentRepos;
using Ninject.Modules;
using System.Data;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories;

namespace RepoTests.Factories {
    public class ScriptCreationModule : NinjectModule {
        private readonly IDbConnection _db;

        public ScriptCreationModule(IDbConnection db) {
            _db = db;
        }
        public override void Load()
        {
            Bind<IAsyncRepository<ScriptMaster>>()
                .To<ScriptMasterRepository>()
                .WithConstructorArgument("db", _db);

            Bind<IScriptCreation>()
                .To<ScriptCreationRepo>()
                .WithConstructorArgument("db", _db);
        }
    }
}
