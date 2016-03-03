using iAgentDataTool.Helpers.Interfaces;
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

namespace RepoTests
{
    [TestFixture]
    public class WebsiteRepoTests
    {
        [Test]
        public async Task Get_All_Websites_Test()
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

            var devAppConfigName = "SmartAgentDev";
            var productionDatabase = "SmartAgentProd";

            var websitesToAdd = new List<WebsiteMaster>();

            var website = WebsiteMaster.CreateWebsiteMaster(
                "Horizan NJ Health via NaviNet",
                "https://navinet.navimedix.com/Main.asp",
                "NJHealth",
                Guid.NewGuid(),
                3
            );

            websitesToAdd.Add(website);


            var result = await CreateRecord(website, devAppConfigName);
            Console.WriteLine(result);

            var prodResult = await CreateRecord(website, productionDatabase);
            Console.WriteLine(prodResult);

        }
    }
}
