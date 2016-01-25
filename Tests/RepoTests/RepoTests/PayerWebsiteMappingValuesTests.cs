using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Helpers.Interfaces;
using Microsoft.Practices.Unity;
using iAgentDataTool.Repositories.Common;
using iAgentDataTool.Models.Common;

namespace RepoTests
{
    [TestFixture]
    public class PayerWebsiteMappingValuesTests
    {
        private readonly string _devAppConfigName = "SmartAgentDev";
        //private readonly string _prodAppConfigName = "SmartAgentProd";

        [Test]
        public async Task UpdateClientLocationKeyTest()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var container = new UnityContainer();

            container.RegisterType<IAsyncRepository<PayerWebsiteMappingValue>, PayerWebsiteMappingValuesAsyncRepository>(new InjectionConstructor(db));
            var repo = container.Resolve<IAsyncRepository<PayerWebsiteMappingValue>>();
            var clientKey = new Guid("F3ED1F27-4023-4C31-A1EC-75498BAD2DA9");
            var newLocKey = new Guid("2bb60a77-3331-4dd1-bdd7-4d4e1fea1edf");
            var oldLocKey = new Guid("1118142A-3415-485F-9C08-5290648A4C05");
            var result = await repo.UpdateLocationKey(clientKey, oldLocKey, newLocKey);
            Console.WriteLine(result);
        }
    }
}
