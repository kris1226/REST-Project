using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.Repositories.SmartAgentRepos;

namespace RepoTests
{
    [TestClass]
    public class CriteriaRepoTests
    {
        private readonly string _devAppConfigName = "SmartAgentDev";
        private readonly string _prodAppConfigName = "SmartAgentProd";
        private Action<object> _WriteToConsole = (value) => Console.WriteLine(value);

        //Creates single criteira record for a given inital scriptkey
        [TestMethod]
        public async Task Create_Single_CriteraSet_Record_Test()
        {
            var clientScriptCriteria = Criteria.CreateCriteria
            (
                    criteriaSetName: "Steward - Morton Hospital -- Tufs via Emdeon Submit",
                    criteriaSetKey: Guid.NewGuid(),
                    scriptKey: new Guid("853a9d42-5ff0-e511-8d27-000c29729dff"),
                    iprkey: "PCTUFTSHEALTH01",
                    clientKey: new Guid("e901a612-710e-4858-ba6c-5f15e7eb924e"),
                    clientLocationKey: new Guid("3fed8eca-2a5e-47f3-8db0-c1cfca108c9e"),
                    updatedBy: "kris.lindsey",
                    deviceId: "TufsEmdeon"
           );
         
            Func<Criteria, string, Task<IEnumerable<Criteria>>> CreateRecords = async (newRecord, dbConfig) =>
            {
                using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[dbConfig].ConnectionString))
                {
                    IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepo>();
                    return await repo.CreateCriteraRecord(newRecord);
                }
            };

            var result = await CreateRecords(clientScriptCriteria, _devAppConfigName);

            foreach (var item in result)
            {
                if (item != null)
                {
                    Console.WriteLine(item.CriteriaSetKey + " " + item.DeviceId + " " + item.CompareValue);
                    var prodResults = await CreateRecords(clientScriptCriteria, _prodAppConfigName);
                    Console.WriteLine(item.CriteriaSetKey + " " + item.DeviceId + " " + item.CompareValue);
                }                
            }            
        }
        [TestMethod]
        public async Task GetCriteria_Detials_With_CriteriaSetTerm()
        {
            //var criteriaSetName = "";
            Func<string, string, Task<IEnumerable<CriteriaDetails>>> Find = async (term, config) =>
            {
                using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[config].ConnectionString))
                {
                    IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                    return await repo.FindByName(term);
                }
            };


            var criteriaDetials = await Find("Liberty", _devAppConfigName);

            if (criteriaDetials != null)
            {
                foreach (var criteria in criteriaDetials)
                {
                    _WriteToConsole(criteria);
                    Console.WriteLine("****************==================================***************");
                }
            }

        }
        [TestMethod]
        public async Task GetCriteriaDetials_With_CriteriaSetKey()
        {
            IEnumerable<CriteriaDetails> criteriaDetials;
            var criteriaSetKey = new Guid("6a12dc76-ce8f-e111-90b9-000c29729dff");
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                criteriaDetials= await repo.FindWithGuidAsync(criteriaSetKey);            
            }

            Console.WriteLine("****************=============================================================================***************");
            if (criteriaDetials.Any())
            {
                foreach (var item in criteriaDetials)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("No records found");
            }
           
            Console.WriteLine("****************=============================================================================***************");
        }
        [TestMethod]
        public async Task GetCriteriaSets_With_CriteriaSetTerm()
        {

            Func<string, string, Task<IEnumerable<CriteriaSets>>> FindCriteria = async (term, config) =>
            {
                using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[config].ConnectionString))
                {
                    IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                    return await repo.FindByName(term);
                }
            };

            var criteriaSets = await FindCriteria("Palomar", _prodAppConfigName);



            if (criteriaSets.Any())
            {
                criteriaSets
                    .ToList()
                    .ForEach(criteira => Console.WriteLine(criteira));
            }
            else
            {
                Console.WriteLine("No records found");
            }
            Console.WriteLine("****************======================================================================================================================================");
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
        public async Task MoveDevCriteraSetsToProd_Test() 
        {
            IEnumerable<CriteriaSets> criteriaSetsDev = null;
            IEnumerable<CriteriaSets> criteriaSetsProd = null;
            IEnumerable<CriteriaSets> nullTest = new List<CriteriaSets>();

            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(devDb));
                IAsyncRepository<CriteriaSets> repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                criteriaSetsDev = await repo.FindByName("Steward - Nashoba Valley Medical Center: Evicor Submit");
            }
            using (IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                IAsyncRepository<CriteriaSets> repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                await repo.AddMultipleToProd(criteriaSetsDev);
                criteriaSetsProd = await repo.FindByName("Steward - Nashoba Valley Medical Center: Evicor Submit");
            }

            foreach (CriteriaSets criteria in criteriaSetsProd)
            {
                Console.WriteLine(criteria);
            }
            Assert.AreEqual(criteriaSetsDev, criteriaSetsProd);
        }

        [TestMethod]
        public async Task MoveCriteriaDetails_To_Prod()
        {
            IEnumerable<CriteriaDetails> cds = null;
            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(devDb));
                IAsyncRepository<CriteriaDetails> repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                cds = await repo.FindByName("Steward - Nashoba Valley Medical Center: Evicor Submit");
            }
            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(devDb));
                IAsyncRepository<CriteriaDetails> repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                var result = await repo.AddAsync(cds);
            }
            foreach (var cd in cds)
            {
                Console.WriteLine(cd);
            }
        }
        //Test to move one set of criteria from development to production
        [TestMethod]
        public async Task Move_Single_Set_Of_Criteria_To_Production_From_Development()
        {
            var criteriaSetKey = new Guid("44529C9F-1727-4E58-B006-9149C5EB92A4");
            CriteriaSets criteriaSet = null;

            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(devDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                Assert.IsNotNull(criteriaSet = await repo.FindCriteriaSetRecord(criteriaSetKey));
            }

            CriteriaSets pordCriteriaSet = null;
            // move found dev record to production
            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(devDb));
                var repo = kernel.Get<ISmartAgentRepository>();
               // Assert.IsNotNull(pordCriteriaSet = await repo.AddCriteriaSetRecord(criteriaSet));
            }
            Console.WriteLine("Record Added successfully!: {0}" , pordCriteriaSet);
        }

        //Get the four criteriaDetail recrods from single criteria Set Key
        [TestMethod]
        public async Task Get_CriteriaDetailRecords_WithCriteriaSetKey()
        {
            var criteriaSetKey = new Guid("7AF690C7-F469-4CC4-A8DC-1C2C3331B421");
            IEnumerable<CriteriaDetails> criteriaDetialRecords = null;

            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(devDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                Assert.IsNotNull(criteriaDetialRecords = await repo.FindCriteriaDetailRecords(criteriaSetKey));
            }

            // move found dev record to production
            //IEnumerable<CriteriaDetails> prodDetialRecords = null;
            using (IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(devDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddCriteriaDetails(criteriaDetialRecords);
            }


            foreach (var item in criteriaDetialRecords)
            {
                Console.WriteLine("Found criteria detail records: {0}", item);
            }
            
        }
    }
}
