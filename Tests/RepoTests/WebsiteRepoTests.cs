using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.SmartAgentRepos;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Remix;
using ConsoleTables.Core;

namespace RepoTests
{
    [TestFixture]
    public class WebsiteRepoTests
    {
        [Test]
        public async Task Get_All_SmartAgent_Websites_Test()
        {
            var devAppConfigName = "SmartAgentDev";
            var prodAppConfigName = "SmartAgentProd";

            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(db));

            var websiteRepo = kernel.Get<IAsyncRepository<WebsiteMaster>>();
            var websites = await websiteRepo.GetAllAsync();
            Assert.NotNull(websites);

            websites.ToList()
                    .ForEach(website => Console.WriteLine(website.ToString()));        
        }
        [Test]
        public async Task Get_All_Remix_Websites_Test()
        {
            var dbConfig = new
            {
                devConfig = "SmartAgentDev",
                prodConfig = "SmartAgentProd",
                remixConfig = "RemixDb"
            };


            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[dbConfig.remixConfig].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(db));

            var websiteRepo = kernel.Get<IAsyncRepository<WebsiteMaster>>();
            var websites = await websiteRepo.GetAllAsync();
            Assert.NotNull(websites);

            websites.ToList()
                    .ForEach(website => Console.WriteLine(website.ToString()));
        }
        [Test]
        public async Task Update_Website_URL()
        {
           // var devAppConfigName = "SmartAgentDev";
            var productionDatabase = "SmartAgentProd";

            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[productionDatabase].ConnectionString);
            IKernel kernel = new StandardKernel(new RepoTestsModule(db));

            var websiteRepo = kernel.Get<IAsyncRepository<WebsiteMaster>>();
            var website = WebsiteMaster.CreateWebsiteMaster(
                "Allied Physicians Submit",
                "https://portal.nmm.cc/Portal",
                "CFC",
                new Guid("e6b9299a-f903-4120-a481-3e5952fb98bc"),
                122
             );

           
            await websiteRepo.UpdateAsync(website);
            var websites = await websiteRepo.GetAllAsync();

            var unitedHealthCare = websites.Where(site => site.WebsiteDomain == website.WebsiteDomain).FirstOrDefault();

            Console.WriteLine(unitedHealthCare);
         
        }
        [Test]
        public async Task Create_Website()
        {
            Func<WebsiteMaster, string, Task<Guid>> CreateRecord = async (newRecord, dbConfig) =>
            {
                using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[dbConfig].ConnectionString))
                {
                    IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepo>();
                    return await repo.CreateWebsiteMasterRecord(newRecord);
                }
            };

            var smartAgentDevelopmentDb = "SmartAgentDev";
            var smartAgentProdDb = "SmartAgentProd";

            var websitesToAdd = new List<WebsiteMaster>();

            var website = WebsiteMaster.CreateWebsiteMaster(
                websiteDesription: "Maverick Medical Group Submit",
                websiteDoman: "https://aerial.carecoordination.medecision.com/pmg/physician/LoginDefault.aspx",
                deviceId: "MMG",
                websiteKey: Guid.NewGuid(),
                portalId: 3
            );

            websitesToAdd.Add(website);


            var result = await CreateRecord(website, smartAgentDevelopmentDb);
            Console.WriteLine(result);

            var prodResult = await CreateRecord(website, smartAgentProdDb);
            Console.WriteLine(prodResult);

        }
        [Test]
        public async Task Create_WebsiteMaster_With_Portal()
        {

        }
        [Test]
        public async Task Get_Portal_Records()
        {
            var devRemixSource = "RemixDb";
            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[devRemixSource].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(db));

            var portalsRepo = kernel.Get<IAsyncRepository<Portals>>();
            var portals = await portalsRepo.GetAllAsync();
            Assert.NotNull(portals);

            ConsoleTable
                .From<Portals>(portals)
                .Write();            
        }
        [Test]
        public async Task Create_Portal_Record()
        {
            var devRemixSource = "RemixDb";
            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[devRemixSource].ConnectionString);

            Func<string, string, Task<IEnumerable<Portals>>> GetPortalRecord = async (name, dbConfig) =>
            {
                using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[dbConfig].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var portalsRepo = kernel.Get<IAsyncRepository<Portals>>();
                    return await portalsRepo.FindByName(name);
                }
            };

        }
    }
}
