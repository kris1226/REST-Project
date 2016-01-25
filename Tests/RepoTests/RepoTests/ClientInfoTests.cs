using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject;
using Microsoft.Practices.Unity;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.Repositories.SmartAgentRepos;
using NUnit.Framework;
using iAgentDataTool.Models.Common;
using System.Data;


namespace RepoTests
{
    [TestFixture]
    public class ClientInfoTests
    {
        private readonly string _devAppConfigName = "SmartAgentDev";
        private readonly string _prodAppConfigName = "SmartAgentProd";
        [Test]
        public async Task Get_Client_Master_Record_Test()
        {
            var clientName = "Swedish America";

            Func<string, string, Task<IEnumerable<ClientMaster>>> FindClient = async (name, source) =>
                {
                    var container = new UnityContainer();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                    {
                        container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                        var repo = container.Resolve<ISmartAgentRepository>();
                        return await repo.FindClientMasterRecord(name);
                    }
                };

            var result = await FindClient(clientName, _prodAppConfigName);
            foreach (var item in result)
            {
                Console.WriteLine(item.ClientName + " " + item.ClientKey + " " + item.HowToDeliver);
            }
            Assert.IsNotNull(result.Any());
        }
        [Test]
        public async Task Find_ClientLocation_Test()
        {
            var clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            var clientLocationName = "Ameri";

            Func<string, string, Task<IEnumerable<ClientLocations>>> FindClientLocation = async (name, source) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.FindClientLocationRecords(name);
                }
            };

            var result = await FindClientLocation(clientLocationName, _prodAppConfigName);
            foreach (var item in result)
            {
                Console.WriteLine(item.ClientLocationKey + " " + item.ClientKey + " " + item.ClientLocationName + " " + item.ClientId + " " + item.TpId + " " + item.FacilityId);
            }
            Assert.IsNotNull(result.Any());
        }
        public async Task Find_FacilityMaster_Test()
        {
            var clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            var facilityName = "Emory St Josephs";

            Func<string, string, Task<IEnumerable<FacilityMaster>>> FindFacility = async (name, source) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.FindFacilityMasterRecords(facilityName);
                }
            };


            var result = await FindFacility(facilityName, _prodAppConfigName);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Assert.IsNotNull(result.Any());
        }

        [Test]
        public async Task Add_ClientLocation_Test()
        {
            //var cl2 = new ClientLocations();
            //cl2.ClientLocationName = "Norton Kosair Children Medical Center - Brownsboro";
            //cl2.ClientKey = new Guid("f3ed1f27-4023-4c31-a1ec-75498bad2da9");
            //cl2.ClientLocationKey = new Guid("79f149f3-716d-4a44-b67b-ce72fa56725a");
            //cl2.DeviceId = "NortonKY";
            //cl2.ClientId = "114173";
            //cl2.FacilityId = "NHSG-KCHB";
            //cl2.TpId = "178916";

            //var cl = new ClientLocations();
            //cl.ClientLocationName = "Norton Diagnostic Center- Fern Creek";
            //cl.ClientKey = new Guid("f3ed1f27-4023-4c31-a1ec-75498bad2da9");
            //cl.ClientLocationKey = new Guid("e1c67e84-31f6-4d7f-af10-3d6114391bb6");
            //cl.DeviceId = "NortonFern";
            //cl.ClientId = "105979";
            //cl.FacilityId = "NHSG-NS";
            //cl.TpId = "178916";

            //var cl3 = new ClientLocations();
            //cl3.ClientLocationName = "Norton Specialists";
            //cl3.ClientKey = new Guid("f3ed1f27-4023-4c31-a1ec-75498bad2da9");
            //cl3.ClientLocationKey = new Guid("f6ee6759-da08-4768-8d35-d1d54e749d5e");
            //cl3.DeviceId = "NortonLouis";
            //cl3.ClientId = "114993";
            //cl3.FacilityId = "NHSG-NS";
            //cl3.TpId = "178916";

            var cl4 = ClientLocations.CreateClientLocation(
                "Norton Women's and Kosair Children's Hospital",
                new Guid("f3ed1f27-4023-4c31-a1ec-75498bad2da9"),
                new Guid("d16d3868-e814-44f6-a6ff-08daf5e77874"),
                "102745",
                "178916",
                "NHSG-KCH"
             );

            //var cl5 = new ClientLocations();
            //cl5.ClientLocationName = "Norton Women's and Kosair Children's Hospital(old Suburban)";
            //cl5.ClientKey = new Guid("f3ed1f27-4023-4c31-a1ec-75498bad2da9");
            //cl5.ClientLocationKey = new Guid("d16d3868-e814-44f6-a6ff-08daf5e77874");
            //cl5.DeviceId = "NortonLPCC";
            //cl5.ClientId = "102745";
            //cl5.FacilityId = "NHSG-KCH";
            //cl5.TpId = "178916";

            var cls = new List<ClientLocations>();

           // cls.Add(cl);
          //  cls.Add(cl2);
            //cls.Add(cl3);
            cls.Add(cl4);
           // cls.Add(cl5);


            var container = new UnityContainer();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                var repo = container.Resolve<ISmartAgentRepository>();
                await repo.AddClientLocationRecords(cls);
            }

            //Action<IEnumerable<ClientLocations>, string> AddClientLocation = async (locations, source) =>
            //{
            //    var container = new UnityContainer();
            //    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
            //    {
            //        container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
            //        var repo = container.Resolve<ISmartAgentRepository>();
            //        await repo.AddClientLocationRecords(locations);
            //    }
            //};

            //AddClientLocation(cls, _devAppConfigName);
            Console.WriteLine("complete..");

        }
    }
}
