using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Submits;
using iAgentDataTool.Helpers;
using iAgentDataTool.ScriptHelpers;
using iAgentDataTool.Repositories.SmartAgentRepos;
using AgentDataServices;
using iAgentDataTool.Models.Common;
using ScriptDataHelpers;
using Submits.cs;
using ConsoleTables.Core;
using Xunit;
using Xunit.Abstractions;

namespace ScriptingTests
{
    public class MMGEntryTest
    {
        private readonly ITestOutputHelper output;
        public MMGEntryTest(ITestOutputHelper output)
        {
            this.output = output;
        }        
        [Fact]
        public async Task Create_MMG_Test()
        {
            var agentDataSvc = new SmartAgentDataSvc();

            var db = new  {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };

            var MMGScripts = new List<Script>() {
                MMG.Login_Script(),
                MMG.GotoRefferalsPage(2),
                MMG.LogOutScript(3)
            };

            foreach (var script in MMGScripts)
            {
                var key = await agentDataSvc.AddScript(script, db.smartAgentDev);
                var returnValue = ReturnValue.CreateReturnValueTemplate(key, script.DeviceId);
                await agentDataSvc.AddReturnValues(returnValue, db.smartAgentDev);
            }

        }
        [Fact]
        public async Task Update_MMG_ScriptCode_Test()
        {
            var agentDataSvc = new SmartAgentDataSvc();
            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };

            var MMGScripts = new List<Script>() {
                MMG.PauseScript(4),
                MMG.PauseErrorScript(5)
            };

            foreach (var script in MMGScripts)
            {
                var key = await agentDataSvc.AddScript(script, db.smartAgentDev);
                var returnValue = ReturnValue.CreateReturnValueTemplate(key, script.DeviceId);
                await agentDataSvc.AddReturnValues(returnValue, db.smartAgentDev);
            }
        }
        [Fact]
        public async Task Add_MMG_CollectionItems_Test()
        {
            var agentDataSvc = new SmartAgentDataSvc();
            var collectionItems = agentDataSvc.GetSmartAgentCollectionItemsMap();

            var db = new {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };

            var loginCollectionItems = MMGColItems.LoginCollectionItems(new Guid("853a9d42-5ff0-e511-8d27-000c29729dff"))
                .Concat(MMGColItems.PatientDemoGraphicsCollectionItems(new Guid("863a9d42-5ff0-e511-8d27-000c29729dff")));

            foreach (var script in loginCollectionItems)
            {
                await agentDataSvc.AddCollectionItems(script, db.smartAgentDev);
            }
        }
        [Fact]
        public async Task Move_ScriptMaster_to_Production()
        {
            var agentDataSvc = new SmartAgentDataSvc();
            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };
            var mmgScriptMaster = await agentDataSvc.GetScriptMasterRecords(new Guid("86ee2f58-33f9-4dbb-8db2-a43c89da5dfc"),
                db.smartAgentDev);

            var scripts = mmgScriptMaster
                .Select(s => Script.CreateScript(
                    s.ScriptDesc, 
                    s.ScriptCode, 
                    s.DeviceId, 
                    s.Category, 
                    s.WebsiteKey))
                .ToList();

            if (scripts.Any())
            {
                foreach (var script in scripts)
                {
                    var key = await agentDataSvc.AddScript(script, db.smartAgentProd);
                    output.WriteLine("{0}", script.Code);
                }   
            }          
        }
        [Fact]
        public async Task Move_return_values_to_prod()
        {
            var agentDataSvc = new SmartAgentDataSvc();
            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd"
            };
            var returnValues = await agentDataSvc.GetScriptReturnValues(
                new Guid("86ee2f58-33f9-4dbb-8db2-a43c89da5dfc"),
                db.smartAgentDev);

            if (returnValues.Any())
            {
                foreach (var returnValue in returnValues)
                {
                    await agentDataSvc.AddReturnValues(returnValue, db.smartAgentProd);               
                    output.WriteLine("{0}, {1}", returnValue.DeviceId, returnValue.ScriptKey);
                }   
            }          
        }
    }
}
