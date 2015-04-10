using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.Common;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Controllers;

namespace WebTool.Tests
{
    public class ApiControllersModule : NinjectModule
    {
        IDbConnection _db;

        public ApiControllersModule(IDbConnection db)
        {
            this._db = db;
        }
        public override void Load()
        {
            Bind<IAsyncRepository<FacilityMaster>>().To<FacilityMasterAsyncRepository>().WithConstructorArgument("db", _db);
            Bind<FacilitiesController>().ToSelf();
        }
    }
}
