using AutoInquiry;
using iAgentDataTool.Helpers;
using iAgentDataTool.ScriptHelpers;
using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.Unity;
using iAgentDataTool.Repositories.SmartAgentRepos;
using AgentDataServices;

namespace ScriptingTests
{
    public class TufsViaNetworkHealthProviderTest 
    {
        [Fact]
        public async Task Create_Tuffs_Script_Test() 
        {
            var db = new
            {
                smartAgentDev = "SmartAgentDev",
                smartAgentProd = "SmartAgentProd",
                remixDev = "RemixDb"
            };

            var agentDataSvc = new SmartAgentDataSvc();

            int count = 1;

            var tuffs = new {
                DeviceId = "TufsNetowr00", 
                WebstieDescription = "TufsNetowrkHealth 00", 
                scripts = new List<Script>(),
                websitekey = new Guid("e38b2dac-a5e7-e511-8d27-000c29729dff")
            };

            var TuffsScript = new List<Script>();

            var loginScript = Script.CreateScript(
                string.Concat(tuffs.WebstieDescription, count, ": Loginscript, login error check"),
                TufsNetworkHealth.Login_Script(new InternetExplorer()).ToString(),
                string.Concat(tuffs.DeviceId, count),
                "Login",
                tuffs.websitekey);
            TuffsScript.Add(loginScript);

            var goToAuthPage = Script.CreateScript(
                string.Concat(tuffs.WebstieDescription, HelperMethods.Increment(count), ": Go to authorization page"),
                TufsNetworkHealth.Goto_Referrals_Auhorizations_PatientSearch_Page().ToString(),
                string.Concat(tuffs.DeviceId, HelperMethods.Increment(count)),
                "PatientSearch",
                tuffs.websitekey);
            TuffsScript.Add(goToAuthPage);

            foreach (var script in TuffsScript)
            {
                var returnValueCount = 0;
                var deviceId = string.Concat("TufsNetowr00", returnValueCount);
                var scriptKey = await agentDataSvc.AddScript(script, db.remixDev);

                if (scriptKey != null)
                {
                    var returnValue = ScriptReturnValue
                        .Build()
                        .WithDeviceId(script.DeviceId)
                        .WithScriptKey(scriptKey)
                        .WithNotEqualKey(new Guid("00000000-0000-0000-0000-000000000000"))
                        .WithEqualKey(new Guid("00000000-0000-0000-0000-000000000000"))
                        .WithReturnValue("SUCCESS")
                        .WithValueOperation("EQ")
                        .Build();

                    await agentDataSvc.AddReturnValues(returnValue, db.remixDev);
                    Console.WriteLine(scriptKey);
                }
                else
                {
                    Console.WriteLine("error adding record", scriptKey);
                }
            }


        }
    }
}
