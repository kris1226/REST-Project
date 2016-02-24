using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AgentDataServices;
using iAgentDataTool.Models.SmartAgentModels;
using System.Diagnostics;
using ConsoleTables.Core;

namespace Data.Tests
{
    [TestFixture]
    public class CriteriaDataTests
    {
        [Test]
        public async Task CriteriaSerachTest()
        {
            IEnumerable<CriteriaSets> criteria = null;
            var agentDataSvc = new SmartAgentDataSvc();
            var devDataSource = "SmartAgentDev";
            var prodDataSource = "SmartAgentProd";

            var criteriaSetName = "Conway";

            try
            {
                criteria = await agentDataSvc.FindCriteria(criteriaSetName, prodDataSource);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
            if (criteria.Any())
            {
                ConsoleTable.From<CriteriaSets>(criteria).Write();
            }
            Console.WriteLine("No records found");
            Assert.IsNotEmpty(criteria);
        }
    }
}
