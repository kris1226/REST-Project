using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;

namespace RepoTests
{
    [TestClass]
    public class CriteriaSetsRepoTests
    {
        [TestMethod]
        public async Task Create_CriteraSet_Record_Test()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString)) 
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaSets>>();

                var criteriaSet = new CriteriaSets();
                criteriaSet.CriteriaSetName = "Test insert Cigna Direct";
                criteriaSet.ScriptKey = new Guid("DF18A7F1-27E6-E111-BB46-000C29729DFF");
                criteriaSet.DeviceId = "CignaTest";

                await repo.AddAsync(criteriaSet);
                var record = await repo.FindByName("Test insert Cigna Direct");
                foreach (var item in record)
                {
                    Console.WriteLine(item.ToString());
                }
            }       
        }
        public async Task GetCriteria_Detials_With_CriteriaSetTerm()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                var criteriaDetials = await repo.FindByName("Genesis");

                foreach (var item in criteriaDetials)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }
}
