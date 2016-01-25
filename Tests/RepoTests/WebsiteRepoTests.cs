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
            //var prodAppConfigName = "SmartAgentProd";

            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[devAppConfigName].ConnectionString);
            IKernel kernel = new StandardKernel(new RepoTestsModule(db));

            var websiteRepo = kernel.Get<IAsyncRepository<WebsiteMaster>>();
            var websites = await websiteRepo.GetAllAsync();
            Assert.NotNull(websites);
            foreach (var result in websites)
            {
                Console.WriteLine(result);
            }            
        }
        [Test]
        public async Task Update_Website_URL()
        {
           // var devAppConfigName = "SmartAgentDev";
            var productionDatabase = "SmartAgentProd";

            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[productionDatabase].ConnectionString);
            IKernel kernel = new StandardKernel(new RepoTestsModule(db));

            var websiteRepo = kernel.Get<IAsyncRepository<WebsiteMaster>>();
            var website = new WebsiteMaster();

            website.WebsiteDescription = "Brand New Day Submit";
            website.WebsiteKey = Guid.NewGuid();
            website.WebsiteDomain = "https://aerial.carecoordination.medecision.com/ucipa/physician/LoginDefault.aspx";
           
            await websiteRepo.UpdateAsync(website);
            var websites = await websiteRepo.GetAllAsync();

            var unitedHealthCare = websites.Where(site => site.WebsiteDomain == website.WebsiteDomain).FirstOrDefault();

            Console.WriteLine(unitedHealthCare);
         
        }
        [Test]
        public async Task Create_Website_URL()
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
            var website = new WebsiteMaster();
            var websitesToAdd = new List<WebsiteMaster>();

            website.WebsiteDescription = "Onesource BCBS Flordia Submit";
            website.WebsiteKey = Guid.NewGuid();
            website.WebsiteDomain = "https://onesource.passporthealth.com/_members/";
            websitesToAdd.Add(website);


            var result = await CreateRecord(website, devAppConfigName);
            Console.WriteLine(result);

            var prodResult = await CreateRecord(website, productionDatabase);
            Console.WriteLine(prodResult);

        }
    }
}
