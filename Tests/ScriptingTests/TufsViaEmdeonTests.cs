using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.Unity;
using iAgentDataTool.Repositories.SmartAgentRepos;
using AgentDataServices;
using AutoInquiry;
using Xunit;
using iAgentDataTool.Helpers;
using iAgentDataTool.ScriptHelpers;
using iAgentDataTool.Models.Common;
using Submits;
using ScriptDataHelpers;

namespace ScriptingTests
{
    public class TufsViaEmdeonTests
    {
        [Fact]
        public async Task Add_To_Database_test()
        {
            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd",
                remixDev = "RemixDb"
            };

            var smartAgentSvc = new SmartAgentDataSvc();

            var tufsViaEmdeonScript = new List<Script>() {
                TufsHealthPlanViaEmdeon.Login_Script(),
                TufsHealthPlanViaEmdeon.GotoRefferalsPage(2),
                TufsHealthPlanViaEmdeon.PauseScript(3),
                TufsHealthPlanViaEmdeon.PauseErrorScript(4),
                TufsHealthPlanViaEmdeon.LogOutScript(5)
            };

            foreach (var script in tufsViaEmdeonScript)
            {
                var key = await smartAgentSvc.AddScript(script, db.smartAgentDev);
                var returnValue = ReturnValue.CreateReturnValueTemplate(key, script.DeviceId);
                await smartAgentSvc.AddReturnValues(returnValue, db.smartAgentDev);
            }
        }
        [Fact]
        public async Task Add_MMG_CollectionItems_Test()
        {
            var agentDataSvc = new SmartAgentDataSvc();

            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };

            var tuffsCollectionItems = SmartAgentCollItems.Login("tufs", 1, new Guid("925d4391-088e-45f2-b18b-d30c9cb76a1a"))
                        .Concat(SmartAgentCollItems.PatDemoGraphics("tufs", 2, new Guid("1fa9c867-b11e-4cda-bd32-476be43d9b42")));

            foreach (var script in tuffsCollectionItems)
            {
                await agentDataSvc.AddCollectionItems(script, db.smartAgentDev);
            }
        }
    }
}
