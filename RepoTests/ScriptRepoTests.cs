using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject;



namespace RepoTests
{
    [TestClass]
    public class ScriptRepoTests
    {
        [TestMethod]
        public Task FindScriptWithWebsiteKey()
        {
            var websiteKey = new Guid("");
        }
    }
}
