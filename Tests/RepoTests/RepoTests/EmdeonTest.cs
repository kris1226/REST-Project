using ConsoleTables.Core;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories;
using iAgentDataTool.Repositories.SmartAgentRepos;
using iAgentDataTool.ScriptHelpers.Interfaces;
using Microsoft.Practices.Unity;
using ScriptDataHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RepoTests.RepoTests
{
    public class EmdeonScriptTest
    {
        [Fact]
        public async Task UpdateScript()
        {
            var devSmartAgent = "SmartAgentDev";
            var prodAppConfigName = "SmartAgentProd";
            var container = new UnityContainer();
            var newScript = new StringBuilder()
                .Append(@"SET !TIMEOUT_STEP 4\nURL GOTO %%websiteDomain%%\nWAIT SECONDS=1\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:username CONTENT=%%websiteUsername%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD FORM=NAME:* ATTR=NAME:password CONTENT=%%websitePassword%%\\nTAG POS=1 TYPE=INPUT:BUTTON FORM=NAME:* ATTR=NAME:btnLogIn\n")
                .Append(@"TAG POS=1 TYPE=B FORM=NAME:Login ATTR=TXT:Invalid<SP>User<SP>ID<SP>or<SP>password* EXTRACT=TXT")
                .ToString();

            ScriptMaster script = ScriptMaster
                .Build()
                .WithScriptKey(new Guid("60b5331b-e52c-e211-b35b-000c29729dff"))
                .WithWebsiteKey(new Guid("5608f070-e12c-e211-b35b-000c29729dff"))
                .WithScriptCode(newScript)
                .Build();

            var scriptKey = new Guid("60b5331b-e52c-e211-b35b-000c29729dff");

            Func<ScriptMaster, string, Task> UpdateScriptCode = async (sm, dataSource) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[dataSource].ConnectionString))
                {
                    container.RegisterType<IAsyncRepository<ScriptMaster>, ScriptMasterRepository>(new InjectionConstructor(db));
                    var repo = container.Resolve<IAsyncRepository<ScriptMaster>>();
                    await repo.UpdateAsync(sm);
                }
            };

            Func<Guid, string, Task<Script>> GetScriptCode = async (key, dataSource) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[dataSource].ConnectionString))
                {
                    var repo = new ScriptMasterRepository(db);
                    return await repo.GetScript(key);
                }
            };

            await UpdateScriptCode(script, devSmartAgent);
            Console.WriteLine("complete");
            var updatedScript = await GetScriptCode(script.ScriptKey, devSmartAgent);
            Console.WriteLine(updatedScript);

            Assert.Equal(script.ScriptCode, updatedScript.Code);

        }
    }
}
