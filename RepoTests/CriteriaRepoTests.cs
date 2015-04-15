using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject;

namespace RepoTests
{
    [TestClass]
    public class CriteriaRepoTests
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
        [TestMethod]
        public async Task GetCriteria_Detials_With_CriteriaSetTerm()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                var criteriaDetials = await repo.FindByName("Pensacola");

                foreach (var item in criteriaDetials)
                {
                    Console.WriteLine(item);
                }
            }
        }
        [TestMethod]
        public async Task GetCriteriaDetials_With_CriteriaSetKey()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                var criteriaSetKey = new Guid("2cc3d1d3-3588-e311-81c8-000c29729dff");
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                var criteriaDetials = await repo.FindWithGuidAsync(criteriaSetKey);

                foreach (var item in criteriaDetials)
                {
                    Console.WriteLine(item);
                }
            }
        }
        [TestMethod]
        public async Task GetCriteriaSets_With_CriteriaSetTerm()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                var criteriaSets = await repo.FindByName("Pensacola");

                foreach (var item in criteriaSets)
                {
                    Console.WriteLine(item);
                }
            }
        }
        [TestMethod]
        public async Task GetCriteriaSets_With_CriteriaSetKey()
        {
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                var criteriaSetKey = new Guid("2CC3D1D3-3588-E311-81C8-000C29729DFF");
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                var criteriaSets = await repo.FindWithGuidAsync(criteriaSetKey);

                foreach (var item in criteriaSets)
                {
                    Console.WriteLine(item);
                }
            }
        }
        [TestMethod]
        public async Task MoveCriteraToProd() { 

}
    }
}
