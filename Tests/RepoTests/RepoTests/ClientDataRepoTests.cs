using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Ninject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.Repositories.SmartAgentRepos;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using ConsoleTables.Core;
using TheCreator;

namespace RepoTests
{
    [TestClass]
    public class ClientDataRepoTests
    {
        private Guid _clientKey = new Guid("38d61357-8643-42ec-9003-3b9da4db390c");
        private string _criteriaSetName = "Palomar Medical Center – Mental Health Services";
        private Guid _facilityKey = new Guid("3E15E013-4A15-44A2-83F4-D8B2ED630397");
        private readonly string _devAppConfigName = "SmartAgentDev";
        private readonly string _prodAppConfigName = "SmartAgentProd";
        private Action<string, object> writeToConsole = (desc, value) => Console.WriteLine(desc, value);

        [TestMethod]
        public async Task Create_Client_Master_Test()
        {            
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            var clientKey = Guid.NewGuid();
            var clientLocationKey = Guid.NewGuid();
            var clientName = "Test1";
            var howToDeliver = "OSVC";

            var newClient = new ClientMaster(clientName: clientName, clientKey: clientKey, howToDeliver: howToDeliver);
            var testlocation = ClientLocations.CreateClientLocation("Test1", clientKey, clientLocationKey, "clientId", "tpid", "facilityID");
            testlocation.LastUserId = "kris.lindsey";

            var clients = new List<ClientMaster>();
            clients.Add(newClient);

            var creator = new SmartAgentClientCreator(smartAgentDb);
            var result = await creator.Create<ClientMaster>(clients);

            if (result != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                var clientlocations = new List<ClientLocations>();
                clientlocations.Add(testlocation);
                var clientlocation = await creator.Create<ClientLocations>(clientlocations);

                if (clientlocation != null)
                {
                    var mappingValues = PayerWebsiteMappingValue.CreateWebisteMappingValue(clientKey, clientlocation);                 
                }
            }

            Console.Write("*******************");
            Console.WriteLine(clientKey);
            Assert.AreEqual(newClient.ClientKey, result);

        }
        public async Task Check_For_Data_Test()
        {
            var clientname = "First Health Carolinas";
            var clientKey = new Guid("7789e2ff-bfc7-4b20-9603-4e46bb64cff1");
            var howToDeliver = "OSSVC";

            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            var repo = kernel.Get<IAsyncRepository<ClientMaster>>();

            var newClient = new ClientMaster(clientName: clientname, clientKey: clientKey, howToDeliver: howToDeliver);

            // Client Master record
         //   var result = await repo.FindByName(clientname);
            var value = await repo.FindWithGuidAsync(clientKey);
            if (value.Any())
            {
                foreach (var item in value)
                {
                    Console.WriteLine(item);
                }
              
            }
            else
            {
                Console.WriteLine("No clients found");
            }                     
        }
        [TestMethod]
        public async Task Create_Client_Test()
        {
            // Get connection prepare for data entry
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);

            // Instantiate Repositories needed for data entry
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));

            // Start client creation process.
            var clientName = "Community Health Systems (CHS)";
            var clientKey = new Guid("5ee74d8b-f1f9-4464-a1e3-aa7c4335d12d");           
            var howToDeliver = "OCSVC";

            if (clientKey == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                clientKey = Guid.NewGuid();
            }

            var client = new ClientMaster(clientName: clientName, clientKey: clientKey, howToDeliver: howToDeliver);

            var clientLocationName = "CHS - Deaconess Med Ctr";
            var clientId = "109781";
            string facilityId = "VAC";
            var tpId = "178995";
            var lastUserId = "kris.lindsey";
        
            IEnumerable<ClientMaster> clients = new List<ClientMaster>();

            var clientMasterRepo = kernel.Get<IAsyncRepository<ClientMaster>>();
            var clientLocationRepo = kernel.Get<IAsyncRepository<ClientLocations>>();
             
            //Check to see if client already exists in data
            clients = await clientMasterRepo.FindByName(client.ClientName);

            // If one exits add location data
            if (clients.Any())
            {
                var locations = new List<ClientLocations>();
                var clientLocationKey = Guid.NewGuid();

                var clientLocation = ClientLocations.CreateClientLocation
                (
                    clientLocationName,
                    clientKey,
                    clientLocationKey,
                    clientId,
                    tpId,
                    facilityId
                );

                clientLocation.LastUserId = lastUserId;

                locations.Add(clientLocation);
                var clientLocationEntryResult = await clientLocationRepo.AddAsync(locations);

                if (clientLocationEntryResult == clientLocation.ClientLocationKey)
                {
                    var facilityRepo = kernel.Get<IAsyncRepository<FacilityMaster>>();

                    var newFacility = FacilityMaster.CreateFaciltiy
                    (
                        clientLocation.ClientLocationName,
                        Guid.NewGuid(),
                        "0144300301443003",
                        clientLocation.ClientKey,
                        clientLocation.ClientLocationKey
                    );

                    var facilites = new List<FacilityMaster>();
                    
                    facilites.Add(newFacility);
                    var result = await facilityRepo.AddAsync(facilites);

                    var mappingValues = new List<PayerWebsiteMappingValue>();

                    var newMappingValue = PayerWebsiteMappingValue.CreateWebisteMappingValue
                        (
                            clientKey,
                            clientLocationKey
                        );

                    mappingValues.Add(newMappingValue);

                    var pwmvRepo = kernel.Get<IAsyncRepository<PayerWebsiteMappingValue>>();
                    var mvresult = await pwmvRepo.AddAsync(mappingValues);

                    if (!string.IsNullOrWhiteSpace(clientLocationName))
                    {
                        var criteriaRepo = new CreateSmartAgentUserRepo(smartAgentDb);
                        var criteriaSearchRepo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                        //var criteriaSetName = "";

                        var insuranceName = "Emblem Submit";
                        var clientkey = new Guid("6648E492-D332-443B-8273-77992C36CD3E");
                        var criteriaSetname = clientName + insuranceName;
                        var scriptKey = new Guid("CD2D1C15-D197-E211-B890-000C29729DFF");
                        var criteriaSetKey = Guid.NewGuid();
                        var iprkey = "PCEMBLEM01";

                        var c = Criteria.CreateCriteria
                            (
                                criteriaSetname,
                                criteriaSetKey,
                                scriptKey,
                                iprkey,
                                clientkey,
                                clientLocationKey,
                                "kris.lindsey"
                            );

                        var critera = await criteriaSearchRepo.FindByName(clientLocationName);

                        if (critera.Any())
                        {
                            Console.WriteLine("Found records! nothing to do hear :)");
                            return;
                        }

                        else
                        {
                            var results = await criteriaRepo.CreateCriteraRecords(c);
                            Console.WriteLine("criteria created: ", result);
                        }
                    }
                    
                    
                }               
                
            }
            //If not create client master record and then procedd to enter client data.
            else
            {

                var cs = new List<ClientMaster>();
                cs.Add(client);
                var result = await clientMasterRepo.AddAsync(cs);
                if (result == clientKey)
                {

                }
            }

            foreach (var item in clients)
            {
                writeToConsole(item.ClientName + " ", item.ClientKey);
            }
        }
        [TestMethod]
        public async Task Create_ClientLocation_Test()
        {
            var clientLocationName = "Martin Memorial Medical Center - North Campus";
            var clientId = "120722";
            string facilityId = null;
            var tpId = "196320";
            var lastUserId = "kris.lindsey";

            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            IEnumerable<ClientMaster> clients = null;
            var clientMasterRepo = kernel.Get<IAsyncRepository<ClientMaster>>();
            var clientLocationRepo = kernel.Get<IAsyncRepository<ClientLocations>>();

       
            clients = await clientMasterRepo.FindByName("Martin");

            foreach (var item in clients)
            {
                writeToConsole(item.ClientName + " ", item.ClientKey);
            }         
            if (clients.Any())
            {
                var locations = new List<ClientLocations>();
                var client = clients.FirstOrDefault();
                var clientKey = client.ClientKey;
                var clientLocation = ClientLocations.CreateClientLocation(
                    clientLocationName,
                    clientKey,
                    Guid.NewGuid(),
                    clientId,
                    tpId,
                    facilityId
                    );
                var deviceId = clientLocationName.Split(' ').FirstOrDefault();

                clientLocation.ClientKey = clientKey;
                clientLocation.ClientLocationKey = Guid.NewGuid();
                clientLocation.ClientLocationName = clientLocationName;
                clientLocation.DeviceId = deviceId;
                clientLocation.ClientId = clientId;
                clientLocation.TpId = tpId;
                clientLocation.FacilityId = facilityId;
                clientLocation.LastUserId = lastUserId;

                locations.Add(clientLocation);

                Guid locationKey;

                smartAgentDb.Open();
                locationKey = await clientLocationRepo.AddAsync(locations);
                smartAgentDb.Close();

                writeToConsole(clientLocationName + " ", locationKey);
            }
         
        }
        [TestMethod]
        public async Task Create_Faciltiy_Master_Test()
        {
            var facilityName = "Martin Memorial Medical Center - North Campus";
            var orderMap = "0149600001496A000";

            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));

            var clientLocationRepo = kernel.Get<IAsyncRepository<ClientLocations>>();

            var result = await clientLocationRepo.FindByName(facilityName);
            if (result.Any())
            {
                var facilites = new List<FacilityMaster>();
                var location = result.SingleOrDefault();

                var facility = FacilityMaster.CreateFaciltiy(
                    facilityName,
                    Guid.NewGuid(),
                    orderMap,
                    location.ClientKey,
                    location.ClientLocationKey
                 );

                facilites.Add(facility);
                var facilityRepo = kernel.Get<IAsyncRepository<FacilityMaster>>();
      
                var facilityKey = await facilityRepo.AddAsync(facilites);
                writeToConsole("Created new facility! Key: ", facilityKey.ToString());

            }
        }
        [TestMethod]
        public async Task Create_FaciltiyDetails_Test()
        {
            var facilityName = "Martin Memorial Medical Center - North Campus";
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            var facilityRepo = kernel.Get<IAsyncRepository<FacilityMaster>>();
            var result = await facilityRepo.FindByName(facilityName);

            if (result.Any())
            {
                var details = new List<FacilityDetail>();
                var facilityData = result.FirstOrDefault();
                var facilityDetial = new FacilityDetail();

                facilityDetial.FacilityKey = facilityData.FacilityKey;
                facilityDetial.UpdatedBy = "kris.lindsey";
                details.Add(facilityDetial);

                var facilityDetialRepo = kernel.Get<IAsyncRepository<FacilityDetail>>();
                var checkResults = await facilityDetialRepo.FindWithGuidAsync(facilityData.FacilityKey);

                if (checkResults.Any())
                {
                    writeToConsole("Already have facility detial records for facility: ", facilityData);
                }
                else
                {
                    var facilityKey = await facilityDetialRepo.AddAsync(details);
                    Console.WriteLine("facility Detials added with facility key: ", facilityKey);
                }
            }
        }
        [TestMethod]
        public async Task Create_PayerWebsiteMappingValues_Test()
        {
            var clientLocationName = "Sid Peterson Memorial Hospital";
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            var clientLocationsRepo = kernel.Get<IAsyncRepository<ClientLocations>>();
            var clientLocations = await clientLocationsRepo.FindByName(clientLocationName);

            if (clientLocations.Any())
            {
                var pwmvRepo = kernel.Get<IAsyncRepository<PayerWebsiteMappingValue>>();
                var location = clientLocations.FirstOrDefault();
                var check = await pwmvRepo.FindWith2GuidsAsync(location.ClientKey, location.ClientLocationKey);

                if (check.Any())
                {
                    Console.WriteLine("Already have website mapping values for client location!!");
                }
                else
                {
                    var values = new List<PayerWebsiteMappingValue>();
                    var websiteMappingValues = PayerWebsiteMappingValue.CreateWebisteMappingValue
                        (
                            location.ClientKey,
                            location.ClientLocationKey
                        );

                    values.Add(websiteMappingValues);

                    var mappingValues = await pwmvRepo.AddAsync(values);
                    Console.WriteLine(mappingValues);
                }                                          
            }
        }
        [TestMethod]
        public async Task Create_Criteria_Test()
        {
            var clientLocationName = "UT Southwestern Medical Center Professional";
            IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));

            var criteriaRepo = new CreateSmartAgentUserRepo(smartAgentDb);
            var criteriaSearchRepo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
            var clientLocationRepo = kernel.Get<IAsyncRepository<ClientLocations>>();

            var clientLocation = await clientLocationRepo.FindByName(clientLocationName);

            if(clientLocation.Any()) 
            {
                var l = clientLocation.FirstOrDefault();
                var clientkey = new Guid("2a5f3da7-0f1d-466b-9df4-55b5b677f106");
                var clientLocationKey = new Guid("f68c2bc6-95db-4f73-a453-5ef66e808930");
                var criteriaSetname = "UT Southwestern Medical Center Professional ";
                Guid scriptKey = Guid.Empty;
                var criteriaSetKey = Guid.NewGuid();
                var iprkey = "";

                var c = Criteria.CreateCriteria
                    (
                        criteriaSetname,
                        criteriaSetKey,
                        scriptKey,
                        iprkey,
                        clientkey,
                        clientLocationKey,
                        "kris.lindsey"
                    );

                
                var critera = await criteriaSearchRepo.FindByName(clientLocationName);

                if (critera.Any())
                {
                    Console.WriteLine("Found records! nothing to do hear :)");
                    return;
                }

                else
                {
                    var result = await criteriaRepo.CreateCriteraRecords(c);
                    Console.WriteLine("criteria created: ", result);
                }
            }
        }

        [TestMethod]
        public async Task Get_Client_Master_Records()
        {
            IEnumerable<ClientMaster> clientData = null;
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<ClientMaster>>();
                clientData = await repo.GetAllAsync();
            }

            clientData.ToList().ForEach(client => Console.WriteLine(client.ToString()));


        }
        [TestMethod]
        public async Task Get_Single_Client_Master_Record_Test()
        {
            IEnumerable<ClientMaster> clientData = null;
            var clientKey = new Guid("820825d2-71e7-41a4-a945-6bf7ddc42fa3");
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<ClientMaster>>();
                clientData = await repo.FindWithGuidAsync(clientKey);
            }
            Console.WriteLine("****************=========================================================***************");
            foreach (var client in clientData)
            {
                Console.WriteLine(client);
            }
            Console.WriteLine("****************===========================================================***************");
        }
        [TestMethod]
        public async Task Get_ClientLocations_Test()
        {
            IEnumerable<ClientLocations> clientLocationData = null;
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<ClientLocations>>();
                clientLocationData = await repo.GetAllAsync();
            }

            foreach (var location in clientLocationData)
            {
                Console.WriteLine(location);
            }
        }
        [TestMethod]
        public async Task Get_Facility_Data_Test()
        {
            IEnumerable<FacilityMaster> facilityData = null;
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<FacilityMaster>>();
                facilityData = await repo.GetAllAsync();
            }

            foreach (var location in facilityData)
            {
                Console.WriteLine(location);
            }
        }
        [TestMethod]
       //Test starts hear to move all client data to production from development.
        // Summary:
        //     Testing for moving development data from development to production
        //

        public async Task Move_Client_Master_Records_To_Production_Test()
        {
            // Guid clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            ClientMaster devclient = null;

            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                devclient = await repo.FindClientMasterRecord(_clientKey);
            }
            writeToConsole("Found Client: {0}", devclient);

            //Move record to production
            //Might want to return the clientkey to prove the successful add.
            ClientMaster prodClient = null;

            using (IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                var result = await repo.AddClientMasterRecord(devclient);
                prodClient = await repo.FindClientMasterRecord(_clientKey);
            }
            writeToConsole("Client successfully added to production", prodClient);
        }
        [TestMethod]
        public async Task Move_ClientMappingMaster_Records_To_Production_Test()
        {
            //Guid clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            IEnumerable<ClientMappingMaster> devRecords = null;

            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindClientMappingMasterRecords(_clientKey);
            }

            writeToConsole("Found Client *** : {0}", devRecords);

            //Move record to production
            //Might want to return the clientkey to prove the successful add.

            IEnumerable<ClientMappingMaster> prodClient = null;
            using (IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddClientMappingMasterRecords(_clientKey);
                prodClient = await repo.FindClientMappingMasterRecords(_clientKey);
            }
            foreach (var item in prodClient)
            {
                writeToConsole("Client successfully added to production: {0}", item);
            }
        }
        [TestMethod]
        public async Task Move_ClientMappingValues_To_Production_Test()
        {
            //var clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            IEnumerable<ClientMappingValue> devRecords = null;

            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindClientMappingValues(_clientKey);
            }

            //Move record to production
            //Might want to return the clientkey to prove the successful add.

            IEnumerable<ClientMappingValue> prodClient = null;
            using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddClientMappingValues(_clientKey);
                prodClient = await repo.FindClientMappingValues(_clientKey);
            }
            foreach (var item in prodClient)
            {
                writeToConsole("Client successfully added to production: {0}", item);
            }
        }
        [TestMethod]
        public async Task Move_Client_Locations_To_Production_Test()
        {
            //var clientKey = new Guid("2A5F3DA7-0F1D-466B-9DF4-55B5B677F106");
            IEnumerable<ClientLocations> devRecords = null;

            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindClientLocationRecords(_clientKey);
            }

            //Move record to production
            //Might want to return the clientkey to prove the successful add.

            IEnumerable<ClientLocations> prodClient = null;
            using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddClientLocationRecords(devRecords);
                prodClient = await repo.FindClientLocationRecords(_clientKey);
            }

            foreach (var item in prodClient)
            {
                Console.WriteLine("Client successfully added to production: {0}", item);
            }
        }
        [TestMethod]
        public async Task Move_Facility_Data_to_Production_Test()
        {
            //var clientKey = new Guid("2A5F3DA7-0F1D-466B-9DF4-55B5B677F106");
            IEnumerable<FacilityMaster> devRecords = null;

            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindFacilityMasterRecords(_clientKey);
            }

            //Move record to production
            //Might want to return the clientkey to prove the successful add.

            IEnumerable<FacilityMaster> prodClient = null;
            using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(prodDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddFacilityMasterRecord(devRecords);
                prodClient = await repo.FindFacilityMasterRecords(_clientKey);
            }

            foreach (var item in prodClient)
            {
                Console.WriteLine("Client successfully added to production: {0}", item);
            }
        }
        [TestMethod]
        public async Task Move_Facility_Detials_Records_To_Prod_Test()
        {
            var facilityKeys = new List<Guid>();
            IEnumerable<FacilityDetail> facilityDetails = null;

            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                facilityDetails = await repo.FindFacilityDetailsRecords(_facilityKey);
            }

            //Move record to production
            //Might want to return the clientkey to prove the successful add.

            IEnumerable<FacilityDetail> prodClient = null;
            using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(prodDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddFacilityDetailsRecords(facilityDetails);

                prodClient = await repo.FindFacilityDetailsRecords(_facilityKey);
            }

            foreach (var item in prodClient)
            {
                Console.WriteLine("Client successfully added to production: {0}", item);
            }
        }
        public async Task Move_Payer_Website_Mapping_Values_To_Production_Test()
        {
            Func<Guid, string, Task<IEnumerable<PayerWebsiteMappingValue>>> FindPayerWebsiteMappingValues = async (clientKey, source) =>
            {
                using (var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(devDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    return await repo.FindPayerWebsiteMappingValues(clientKey);
                }
            };

            Func<IEnumerable<PayerWebsiteMappingValue>, string, Task<int>> AddPayerWebsiteMappingValues = async (payerWebsiteMappingValues, source) =>
            {
                using (var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(devDb));
                    var repo = kernel.Get<ISmartAgentRepo>();
                    var result = await repo.AddPayerWebsiteMappingValue(payerWebsiteMappingValues);
                    return result;
                }
            };
            var devRecords = await FindPayerWebsiteMappingValues(_clientKey, _devAppConfigName);
            var moveToProd = AddPayerWebsiteMappingValues(devRecords, _prodAppConfigName);

        }
        // Move criteriaSets to production from development.
        public async Task Move_CriteriaSets_From_Dev_To_Prod_Test()
        {
            Action<string, object> WriteToConsole = (text, obj) => Console.WriteLine(obj);
            
            Func<string, Task<IEnumerable<CriteriaSets>>> FindCriteriaSets = async (conn) =>
            {
                using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    return await repo.FindCriteriaSetRecords(_criteriaSetName);
                }
            };

            Func<string, IEnumerable<CriteriaSets>, Task<IEnumerable<CriteriaSets>>> AddCrtiera = async (conn, records) =>
            {
                using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(prodDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    await repo.AddCriterias(records);
                    return await repo.FindCriteriaSetRecords(_criteriaSetName);
                }
            };

            var devRecords = await FindCriteriaSets(_devAppConfigName);
            //Move record to production
            //Might want to return the clientkey to prove the successful add.
            var prodRecords = await AddCrtiera(_prodAppConfigName, devRecords);

            foreach (var item in prodRecords)
            {
                WriteToConsole("Criteria successfully added to production: {0}", item);
            }
        }
        //Move Criteria Detials Records to Production
        [TestMethod]
        public async Task Move_CriteriaDetails_From_Dev_To_Prod_Test()
        {
            //Find records in development database server.

            Func<string, string, Task<IEnumerable<CriteriaSets>>> FindCriteriaSets = async (criteriaSetTerm, connString) =>
            {
                using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[connString].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    return await repo.FindCriteriaSetRecords(criteriaSetTerm);
                }
            };


            Func<string, IEnumerable<CriteriaSets>, Task<IEnumerable<CriteriaDetails>>> FindCriteriaDetails = async (conn, records) =>
            {
                using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    return await repo.FindWithCriteriaSetKeys(records.Select(element => element.CriteriaSetKey));
                }
            };

            Func<string, IEnumerable<CriteriaDetails>, Task<IEnumerable<CriteriaDetails>>> AddCriteriaDetails = async (conn, records) =>
            {
                using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(prodDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    await repo.AddCriteriaDetails(records);
                    return await repo.FindWithCriteriaSetKeys(records.Select(element => element.CriteriaSetKey));
                }
            };

            Func<string, IEnumerable<CriteriaSets>, string, Task<IEnumerable<CriteriaSets>>> AddCrtiera = async (conn, records, criteriaSetName) =>
            {
                using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(prodDb));
                    var repo = kernel.Get<ISmartAgentRepository>();
                    await repo.AddCriterias(records);
                    return await repo.FindCriteriaSetRecords(criteriaSetName);
                }
            };

            var criteriaName = "Pomerado Palomar Hospital – Outpatient Pavilion:";

            var devCriteriaSets = await FindCriteriaSets(criteriaName,
                _devAppConfigName);

            if (devCriteriaSets.Any())
            {
                var prodCriteriaRecords = await AddCrtiera(_prodAppConfigName, devCriteriaSets, criteriaName);
                // Find dev criteria detials records
                var criteriaDetialsDev = await FindCriteriaDetails(_devAppConfigName, devCriteriaSets);

                //Move record to production
                //Might want to return the clientkey to prove the successful add.

                var prodClient = await AddCriteriaDetails(_prodAppConfigName, criteriaDetialsDev);
                if (prodClient.Any())
                {
                    ConsoleTable.From<CriteriaDetails>(prodClient).Write();
                }
                else
                {
                    Console.WriteLine("No records added");
                }
                
            }

        }
        [TestMethod]
        public async Task Move_Client_To_Prod_Test()
        {

            // Guid clientKey = new Guid("db38cb6a-29fc-452c-befe-a3acf2648b61");
            ClientMaster devclient = null;

            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                devclient = await repo.FindClientMasterRecord(_clientKey);
            }
            writeToConsole("Found Client: {0}", devclient);

            //Move record to production
            //Might want to return the clientkey to prove the successful add.
            ClientMaster prodClient = null;

            using (IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                var result = await repo.AddClientMasterRecord(devclient);
                prodClient = await repo.FindClientMasterRecord(_clientKey);
            }
            writeToConsole("Client successfully added to production", prodClient);


        }
        // Adds all previously known script criteria to database
        public async Task Add_Criteria_To_Dev_Test()
        {
            //Find records in development database server.
            var clientName = "Bond Clinic: ";
            var insuranceName = "BCBS CA Submit";
            var clientkey = new Guid("52d4ec2e-f608-4520-a853-d06ae860b019");
            var clientLocationKey = new Guid("13ebf4f0-57a1-4a7f-bb40-85fdb1940fb8");
            var criteriaSetname = clientName + insuranceName;

            var scriptKey = new Guid("2DC3E283-E070-E511-96C2-000C29729DFF");
            var criteriaSetKey = Guid.NewGuid();

            var iprkey = "PCBSCA01";

            var c = Criteria.CreateCriteria
                (
                    criteriaSetname,
                    criteriaSetKey,
                    scriptKey,
                    iprkey,
                    clientkey,
                    clientLocationKey,
                    "kris.lindsey"
                );
            Func<string, Criteria, Task<IEnumerable<Criteria>>> AddCritera = async (connString, critera) =>
            {
                using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[connString].ConnectionString))
                {
                    var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                    var repo = kernel.Get<ISmartAgentRepo>();
                    var results = await repo.CreateCriteraRecords(critera);
                    return results;
                }
            };

           var values = await AddCritera(_devAppConfigName, c);
           var prod = await AddCritera(_prodAppConfigName, c);

           var counter = 0;
           foreach (var item in values)
           {
               counter += 1;
               writeToConsole(counter + " Criteria ", item);
           }
    
        }
        [TestMethod]
        public async Task Add_Payer_Website_MappingValue_Records_To_Production_Test()
        {
            // var clientKey = new Guid("2A5F3DA7-0F1D-466B-9DF4-55B5B677F106");
            IEnumerable<PayerWebsiteMappingValue> devRecords = null;

            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindPayerWebsiteMappingValues(_clientKey);
            }

            IEnumerable<PayerWebsiteMappingValue> prodClient = null;
            using (var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(prodDb));
                ISmartAgentRepository repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddPayerWebsiteMappingValues(devRecords);
                prodClient = await repo.FindPayerWebsiteMappingValues(_clientKey);
            }
            foreach (var item in devRecords)
            {
                writeToConsole("Client successfully added to production: {0}", item);
            }
        }
        public async Task Add_Criteria_Details_Test()
        {
            ICollection<CriteriaDetails> criteriaDetails = new List<CriteriaDetails>();
            CriteriaDetails criteriaDetial = new CriteriaDetails();

            criteriaDetial.ClientKey = "52d4ec2e-f608-4520-a853-d06ae860b019";
            criteriaDetial.ClientLocationKey = "13ebf4f0-57a1-4a7f-bb40-85fdb1940fb8";
            criteriaDetial.LastUserId = "kris.lindsey";
            criteriaDetial.CriteriaSetKey = Guid.NewGuid();
            criteriaDetial.DeviceId = "BCBS_CA";
            criteriaDetial.IprKey = "PCBSCA01";

            criteriaDetails.Add(criteriaDetial);

            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<CriteriaDetails>>();
                var result = await repo.AddAsync(criteriaDetails);
            }

            foreach (var client in criteriaDetails)
            {
                Console.WriteLine(client);
            }
        }
        [TestMethod]
        //Find multiple CriteriaSet Records and add them to production
        public async Task Find_Muliple_CriteriaSet_Records_With_CriteriaSetKeys_Add_To_Production_Test()
        {
            var criteriaSetKeys = new List<Guid>();

            criteriaSetKeys.Add(new Guid("929015EB-2571-4E78-9B6B-6A00751414F0"));
            criteriaSetKeys.Add(new Guid("679536EC-9D0C-4806-A286-690FE2AE1EFF"));
            criteriaSetKeys.Add(new Guid("0E41C5C2-097B-4F38-BB10-499BCA5A7991"));
            criteriaSetKeys.Add(new Guid("D1DC8822-0F07-4C69-AFBC-6570C709E033"));
            criteriaSetKeys.Add(new Guid("D1A7ABC1-552D-46CC-AF45-629A58411953"));
            criteriaSetKeys.Add(new Guid("6ED860A6-B5F4-4055-9C56-EF9C085D910A"));
            criteriaSetKeys.Add(new Guid("7E94AD5C-09ED-418C-934D-DC906FCAC59B"));

            IEnumerable<CriteriaSets> devRecords = null;
            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindCriteriaSetsWithCriteriaSetKeys(criteriaSetKeys);
            }

            // Add records to production
            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddCriterias(devRecords);
            }

        }
        [TestMethod]
        public async Task Find_CriteriaDetails_Records_With_Muliple_CriteriaSet_Keys_Test()
        {
            var criteriaSetKeys = new List<Guid>();

            criteriaSetKeys.Add(new Guid("929015EB-2571-4E78-9B6B-6A00751414F0"));
            criteriaSetKeys.Add(new Guid("679536EC-9D0C-4806-A286-690FE2AE1EFF"));
            criteriaSetKeys.Add(new Guid("0E41C5C2-097B-4F38-BB10-499BCA5A7991"));
            criteriaSetKeys.Add(new Guid("D1DC8822-0F07-4C69-AFBC-6570C709E033"));
            criteriaSetKeys.Add(new Guid("D1A7ABC1-552D-46CC-AF45-629A58411953"));
            criteriaSetKeys.Add(new Guid("6ED860A6-B5F4-4055-9C56-EF9C085D910A"));
            criteriaSetKeys.Add(new Guid("7E94AD5C-09ED-418C-934D-DC906FCAC59B"));

            IEnumerable<CriteriaDetails> devRecords = null;
            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                devRecords = await repo.FindCriteriaDetailRecords(criteriaSetKeys);
            }

            // Add Records to Production
            using (var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<ISmartAgentRepository>();
                await repo.AddCriteriaDetails(devRecords);
            }

        }
        [TestMethod]
        public async Task Find_Facility_With_Term_Test()
        {
            IEnumerable<FacilityMaster> facilityData = null;
            using (IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                IKernel kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
                var repo = kernel.Get<IAsyncRepository<FacilityMaster>>();
                facilityData = await repo.FindByName("genesis");
            }
            Console.WriteLine("****************=============================================================================***************");
            foreach (var location in facilityData)
            {
                Console.WriteLine(location);
            }
            Console.WriteLine("****************=============================================================================***************");
        }
        // look up memoization to improve implemntation
        // this is a naieve recursive implemntaion.
       [TestMethod]
        public static int Factorial(int n)
        {
            if(n < 0) 
            {
                throw new ArgumentException("Must compute with positve integer");
            }
            else if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }
        [TestMethod]
       public static void FactorialTest()
       {
           Func<Decimal, Decimal> Factorial = null;

           Factorial = (n) =>
           {
               if (n < 0)
               {
                   throw new ArgumentException("Must compute with positve integer");
               }
               else if (n == 0)
               {
                   return 1;
               }
               else
               {
                   return n * Factorial(n - 1);
               }
           };
           Console.WriteLine(Factorial(4));
       }
    }        
}
