using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Ninject;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.AsyncRepositories.Common;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.SmartAgentRepos;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using RepoTests.Factories;
using ConsoleTables.Core;
using NUnit.Framework;
using AgentDataServices;
using ScriptDataHelpers;
using iAgentDataTool.ScriptHelpers;



namespace RepoTests {
    [TestFixture]
    public class ScriptRepoTests {

        private Action<object> write = w => Console.WriteLine(w);
        private readonly string _devSmartAgent = "SmartAgentDev";
        private readonly string _prodSmartAgentDb = "SmartAgentProd";
        private IAsyncRepository<ScriptMaster> _scriptRepo;

        //  private readonly string _devIAgent = "RemixDb";
        [SetUp]
        public void Setup()
        {
            var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString);
            var kernel = new StandardKernel(new ScriptCreationModule(smartAgentDb));
            _scriptRepo = kernel.Get<IAsyncRepository<ScriptMaster>>();
        }

        //[Test]
        //public async void Update_Script_Records()
        //{
        //    var newCode = "SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=A FORM=ACTION:* ATTR=TXT:Member\nTAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:* ATTR=NAME:*tbMemberID CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=ACTION:* ATTR=NAME:*SearchMemberCtrl$btSearch\n";
        //    var websiteKey = new Guid("89bd591b-8cbf-e111-b39a-000c29729dff");
        //    var scriptKey = new Guid("8294dbd1-acbf-e111-b39a-000c29729dff");
        //    var scriptDesc = "KePro Submit 002: Member search";
        //    var category = "PatientSearch";
        //    var deviceId = "002";


        //var keproScript001 = ScriptMaster.Build()
        //                .WithScriptKey(scriptKey)
        //                .WithScriptDesc(scriptDesc)
        //                .WithWebsiteKey(websiteKey)
        //                .WithScriptCode(newCode)
        //                .WithNumberOfIterations(0)
        //                .WithCategory(category)
        //                .WithDeviceId(deviceId)
        //                .Build();

        //    Action<ScriptMaster> UpdateScriptCode = async (script) => {
        //        await _scriptRepo.UpdateAsync(script);
        //    };
        //    try
        //    {
        //        await UpdateScriptCode(keproScript001);
        //    }
        //    catch (Exception) {
                                   
        //    }                             
        //    finally {
        //        Console.WriteLine("process complete");
        //    }
        //}

        [Test]
        public async Task Get_First_ScriptKey_From_Client_Script_SetUp()
        {

            var clientName = "Columbia University - Trustees of Columbia ";
            var insuranceName = "Emblem Submit";
            var clientkey = new Guid("6648E492-D332-443B-8273-77992C36CD3E");
            var clientLocationKey = new Guid("4E517A91-153F-40E7-92B5-37BFE5969DA8");
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



            Func<Criteria, string, Task<Guid>> GetFirstScriptKey = async (criteria, config) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[config].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.GetFirstScriptKey(criteria);
                }
            };

            var initialScriptKey = await GetFirstScriptKey(c, _devSmartAgent);
            write(scriptKey);
        }
        [Test]
        public async Task Get_Scripts_Test()
        {
            Action<object> write = value => Console.WriteLine(value);
            var container = new UnityContainer();
            var websiteKey = new Guid("D6C6AD62-0E24-E511-96C2-000C29729DFF");
            IEnumerable<ScriptMaster> scripts = null;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString))
            {
                container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
                var repo = container.Resolve<IScriptCreation>();
                scripts = await repo.GetScripts(websiteKey);
            }
            foreach (var item in scripts)
            {
                write(item);
            }
        }
        [Test]
        public async Task Create_Client_Master_Test()
        {
            Func<ClientMaster, string, Task<Guid>> CreateClientMasterRecord = async (cm, connectionString) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.AddClientMasterRecord(cm);
                }
            };
            //ClientMaster client  = null;
            var record = new ClientMaster(clientName: "clientname", howToDeliver: "ecnauth", clientKey: new Guid(""));

            var clientKey = await CreateClientMasterRecord(record, _devSmartAgent);
            write(clientKey);
        }

        [Test]
        public async Task Create_Client_Test()
        {
            var db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var container = new UnityContainer();
            container.RegisterType<ICreatSmartAgentClient, CreateSmartAgentUserRepo>(new InjectionConstructor(db));
            container.RegisterType<IDbConnection, SqlConnection>();
            var repo = container.Resolve<ICreatSmartAgentClient>();

            var newClient = new SmartAgentClient();
            newClient.ClientLocationName = "Good Samaritan Hospital Lebanon";
            newClient.ClientKey = new Guid("3bc45110-cad0-4f2a-bd2d-efccda2732d4");
            newClient.ClientLocationKey = new Guid("cb097cc3-2f93-4f3a-867c-192b0fd1c3db");
            newClient.DeviceId = "Samaritan";
            newClient.ClientId = "120838";
            newClient.TpId = "196387";
            newClient.FacilityId = "1";
            newClient.OrderMap = "0150700001507000";
            newClient.FacilityKey = Guid.NewGuid();
            try
            {
                await repo.CreateSmartAgentClient(newClient);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Test]
        public async Task Create_ClientLocation_Test()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var container = new UnityContainer();
            container.RegisterType<ICreatSmartAgentClient, CreateSmartAgentUserRepo>(new InjectionConstructor(db));
            container.RegisterType<IDbConnection, SqlConnection>();
            var repo = container.Resolve<ICreatSmartAgentClient>();

            SmartAgentClient newClient = new SmartAgentClient();
            newClient.ClientLocationName = "Norton Kosair Children Hospital";
            newClient.ClientKey = new Guid("F3ED1F27-4023-4C31-A1EC-75498BAD2DA9");
            newClient.ClientLocationKey = new Guid("79F149F3-716D-4A44-B67B-CE72FA56725A");
            newClient.DeviceId = "Norton";
            newClient.ClientId = "102765";
            newClient.TpId = "178916";
            newClient.FacilityId = "NHSG-KCH";

            var key = await repo.CreateClientLocation(newClient);
            Console.WriteLine(key);
        }
        [Test]
        public async Task Create_Website_Test()
        {
            Guid websitekey = Guid.NewGuid();
            var container = new UnityContainer();

            var websiteMaster = WebsiteMaster.CreateWebsiteMaster(
                "Emdeon Script",
                "http://portal.nmm.cc/nmm/en/index.jsp",                
                "CFC",
                Guid.NewGuid(),
                117
            );

            Func<WebsiteMaster, string, Task<Guid>> CreateWebsiteRecord = async (website, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepo, CreateSmartAgentUserRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepo>();
                    return await repo.CreateWebsiteMasterRecord(website);
                }
            };

            Func<string, string, Task<WebsiteMaster>> FindWebsiteRecord = async (searchTerm, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepo, CreateSmartAgentUserRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepo>();
                    return await repo.FindWebsiteRecord(searchTerm);
                }
            };

            var dataSource = _devSmartAgent;
            var prodDataSource = _prodSmartAgentDb;

            var result = await FindWebsiteRecord(websiteMaster.WebsiteDescription, dataSource);

            if (result == null)
            {
                websitekey = await CreateWebsiteRecord(websiteMaster, dataSource);
                write(websitekey);
            }
            write(result.WebsiteDescription + " " + result.WebsiteKey);

            var result2 = await FindWebsiteRecord(websiteMaster.WebsiteDescription, prodDataSource);
            if (result2 == null)
            {
                websitekey = await CreateWebsiteRecord(websiteMaster, prodDataSource);
                write(websitekey);
            }
            write(result.WebsiteDescription + " " + result.WebsiteKey);

        }

        [Test]
        public void HorizonNaviNetScript()
        {
            var code = new StringBuilder()
                .Append(NaviNet.LoginScript(IEVersion.GetIEVersion()[10]))            
                .Append(NaviNet.HorizonNJ.GotoHorizonSubmitPage());

            //var authoirzationPage = NaviNet.HorizonNJ.GotoHorizonSubmitPage(horizonLoginScript).ToString();
            Console.WriteLine(code);
           
        }
        [Test]
        public async Task Create_NetworkSubmit_Test()
        {
            var NewtowekSubmitScript = new List<Script>();
            var scriptVariablesMap = StaticHelpers.GetScriptVairableMap();
            var container = new UnityContainer();
            var websiteDescription = "Network health submit ";
            var websiteKey = new Guid("6af63ad0-66cf-4b64-9042-38f061ce5cbd");
            var deviceId = "NJHorizon";

        }
        [Test]
        public async Task Create_Script_Record_Test()
        {
            var version = IEVersion.GetIEVersion();
            Action<object> writeOut = value => Console.WriteLine(value);
            var scriptVariablesMap = StaticHelpers.GetScriptVairableMap();
        
            var scripts = new List<Script>();
            var websiteDescription = "Horizon NJ Health via NaviNet Submit ";
            var websiteKey = new Guid("6af63ad0-66cf-4b64-9042-38f061ce5cbd");
            var deviceId = "NJHorizon";

            var loginScript = Script.CreateScript
            (
                websiteDescription + "001: Login, onlogin error check",
                NaviNet.LoginScript(version[9]).ToString(),
                string.Concat(deviceId, "_001"),
                "Login",
                 websiteKey
            );
            scripts.Add(loginScript);

            //var script2 = Script.CreateScript
            //(
            //    websiteDescription + "002: goto Horizon, submit page",
            //    NaviNet.HorizonNJ.GotoHorizonSubmitPage().ToString(),
            //    string.Concat(deviceId, "_002"),
            //    "PatientSearch",
            //     websiteKey
            //);
            //scripts.Add(script2);

            var script5 = Script.CreateScript(
                websiteDescription + "004: Pause for Submit",
                NaviNet.Pause().ToString(),
                string.Concat(deviceId, "_004"),
                "Extraction",
                websiteKey
            );
            scripts.Add(script5);

            //var script6 = Script.CreateScript
            // (
            //     websiteDescription + "004: Pause error",
            //     NaviNet.PauseOnError().ToString(),
            //     deviceId + "_007",
            //     "Extraction",
            //     websiteKey
            // );
            //scripts.Add(script6);

            Func<Script, string, Task<Guid>> AddScriptMasterRecord = async (sm, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<IScriptCreation>();
                    return await repo.CreateScritp(sm);
                }
            };

            //Func<ScriptReturnValue, string, Task<IEnumerable<ScriptReturnValue>>> AddReturnValues = async (rv, connectionString) =>
            //{
            //    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            //    {
            //        var container = new UnityContainer();
            //        container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
            //        var repo = container.Resolve<IScriptCreation>();
            //        return await repo.CreateReturnValues(rv);
            //    }
            //};

            //foreach (var script in scripts)
            //{
            //    var result = await AddScriptMasterRecord(script, _devSmartAgent);
            //    write(result);

            //    if (result != null)
            //    {
            //        var returnValue = new ScriptReturnValue();

            //        returnValue.ScriptKey = result;
            //        returnValue.DeviceId = script.DeviceId;
            //        returnValue.EqualScripKey = new Guid("00000000-0000-0000-0000-000000000000");
            //        returnValue.NotEquelScriptKey = new Guid("00000000-0000-0000-0000-000000000000");
            //        returnValue.MappingValue = null;
            //        returnValue.ReturnValue = "SUCCESS";

            //        var resultScripts = await AddReturnValues(returnValue, _devSmartAgent);

            //        resultScripts.ToList().ForEach(s => write(s.ScriptKey + " " + deviceId));

            //    }
            //    else
            //    {
            //        Console.WriteLine("error adding record", result);
            //    }
            //}
        }
        //[Test]
        //public async Task Create_Collection_Item_Test()
        //{
        //    var container = new UnityContainer();
        //    var collectionItems = new List<ScriptCollectionItem>();
        //    var data = new SmartAgentDataSvc();
        //    var collectionItemsMap = data.GetCollectionItemsMap();
        //    Func<ScriptCollectionItem, Task<ScriptCollectionItem>> CreateCollectionItems = async (c) =>
        //    {
        //        using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString))
        //        {
        //            container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));

        //            var repo = container.Resolve<IScriptCreation>();
        //            return await repo.CreateCollectionItems(c);
        //        }
        //    };

        //    var deviceId = "NYMedicaid";

        //    var websiteDomain = new ScriptCollectionItem();
        //    websiteDomain.FieldKey = collectionItemsMap["WebsiteDomain"];
        //    websiteDomain.ScriptKey = new Guid("0e035220-49da-e511-8d27-000c29729dff");
        //    websiteDomain.OverrideLabel = "Website Domain";
        //    websiteDomain.DeviceId = deviceId + "_001";
        //    collectionItems.Add(websiteDomain);

        //    var username = new ScriptCollectionItem();
        //    username.FieldKey = collectionItemsMap["Username"];
        //    username.ScriptKey = new Guid("0e035220-49da-e511-8d27-000c29729dff");
        //    username.OverrideLabel = "Username";
        //    username.DeviceId = deviceId + "_001";
        //    collectionItems.Add(username);

        //    var websitePassword = new ScriptCollectionItem();
        //    websitePassword.FieldKey = collectionItemsMap["websitePassword"];
        //    websitePassword.ScriptKey = new Guid("0e035220-49da-e511-8d27-000c29729dff");
        //    websitePassword.OverrideLabel = "website Password";
        //    websitePassword.DeviceId = deviceId + "_001";
        //    collectionItems.Add(websitePassword);

        //    var memberId = new ScriptCollectionItem();
        //    memberId.FieldKey = collectionItemsMap["MemberID"];
        //    memberId.ScriptKey = new Guid("12035220-49da-e511-8d27-000c29729dff");
        //    memberId.OverrideLabel = "MemberId";
        //    memberId.DeviceId = deviceId + "_005";
        //    collectionItems.Add(memberId);

        //    //var patLastName = new ScriptCollectionItem();
        //    //patLastName.FieldKey = collectionItem["PatientLastName"];
        //    //patLastName.ScriptKey = new Guid("5cd3e1ec-abd5-e511-8d27-000c29729dff");
        //    //patLastName.OverrideLabel = "PatLName";
        //    //patLastName.DeviceId = deviceId + "_002";
        //    //collectionItems.Add(patLastName);

        //    var serviceDate = new ScriptCollectionItem();
        //    serviceDate.FieldKey = collectionItemsMap["ServiceDate"];
        //    serviceDate.ScriptKey = new Guid("10035220-49da-e511-8d27-000c29729dff");
        //    serviceDate.OverrideLabel = "ServiceDate";
        //    serviceDate.DeviceId = deviceId + "_003";
        //    collectionItems.Add(serviceDate);

        //    var serviceDate2 = new ScriptCollectionItem();
        //    serviceDate2.FieldKey = collectionItemsMap["ServiceDate"];
        //    serviceDate2.ScriptKey = new Guid("10035220-49da-e511-8d27-000c29729dff");
        //    serviceDate2.OverrideLabel = "ServiceDate";
        //    serviceDate2.DeviceId = deviceId + "_003";
        //    collectionItems.Add(serviceDate2);


        //    //var pateintDateOfBirth = new ScriptCollectionItem();
        //    //pateintDateOfBirth.FieldKey = collectionItem["PatientDOB"];
        //    //pateintDateOfBirth.ScriptKey = new Guid("5dd3e1ec-abd5-e511-8d27-000c29729dff");
        //    //pateintDateOfBirth.OverrideLabel = "PatDOB";
        //    //pateintDateOfBirth.DeviceId = deviceId + "_003";
        //    //collectionItems.Add(pateintDateOfBirth);

        //    //var serviceDate = new ScriptCollectionItem();
        //    //serviceDate.FieldKey = collectionItem["PatientDOB"];
        //    //serviceDate.ScriptKey = new Guid("5cd3e1ec-abd5-e511-8d27-000c29729dff");
        //    //serviceDate.OverrideLabel = "Service Date";
        //    //serviceDate.DeviceId = deviceId + "_003";
        //    //collectionItems.Add(serviceDate);

        //    collectionItems.ForEach(async record =>
        //    {
        //        var result = await CreateCollectionItems(record);
        //        write(result.OverrideLabel + " " + result.ScriptKey);
        //    });

        //    //foreach (var item in collectionItems)
        //    //{
        //    //    var result = await CreateCollectionItems(item);
        //    //    write(result.OverrideLabel + " " + result.ScriptKey);
        //    //}
        //}
        [Test]
        public async Task Add_ExtractionMap_Test()
        {
            var container = new UnityContainer();
            var em = new WebsiteExtractionMap();

            em.WebsiteKey = new Guid("86ee2f58-33f9-4dbb-8db2-a43c89da5dfc");
            em.DataName = "Status";
            em.DocumentLocation = "table";
            em.LocationType = "Containsl";
            em.LocationValue = "Status";
            em.FormatFunction = null;
            em.ValueFunction = null;
            em.Priority = 1;

            Func<WebsiteExtractionMap, string, Task<WebsiteExtractionMap>> AddRecord = async (newRecord, configName) =>
                {
                    using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings[configName].ConnectionString))
                    {
                        container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
                        var repo = container.Resolve<IScriptCreation>();
                        return await repo.CreateExtractionMap(em);
                    }
                };

            var newExtractionRecord = await AddRecord(em, _devSmartAgent);
            Console.WriteLine(newExtractionRecord.WebsiteKey.ToString());

            var prodRecord = await AddRecord(em, _prodSmartAgentDb);
            Console.WriteLine(prodRecord.WebsiteKey.ToString());
        }
        [Test]
        public async Task Add_ClientScripts_Test()
        {
            var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodSmartAgentDb].ConnectionString);
            var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);
            var container = new UnityContainer();
            var websiteKey = new Guid("9b82289b-6f72-e511-96c2-000c29729dff");

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(devDb));
            var devRepo = container.Resolve<ISmartAgentRepository>();



            var scripts = await devRepo.FindScripts(websiteKey);

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(prodDb));
            var prodRepo = container.Resolve<ISmartAgentRepository>();

            await prodRepo.AddScripts(scripts);

        }
        [Test]
        public async Task MoveReturnValuesToProdTest()
        {
            var devDataSource = "SmartAgentDev";
            var prodDataSource = "SmartAgentProd";
            var websiteKey = new Guid("e6b9299a-f903-4120-a481-3e5952fb98bc");

            Func<Guid, string, Task<IEnumerable<ScriptReturnValue>>> FindRecords = async (key, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var devRepo = container.Resolve<ISmartAgentRepository>();
                    return await devRepo.FindScriptReturnValues(key);
                }
            };

            Func<IEnumerable<ScriptReturnValue>, Task> MoveScriptsToProd = async (scriptReturnValues) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[prodDataSource].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var prodRepo = container.Resolve<ISmartAgentRepository>();
                    await prodRepo.AddScriptReturnValues(scriptReturnValues, websiteKey);
                }
            };


            var returnValues = await FindRecords(websiteKey, devDataSource);
            Assert.IsNotEmpty(returnValues);
            ConsoleTable.From<ScriptReturnValue>(returnValues).Write();

            await MoveScriptsToProd(returnValues);

            var prodReturnValues = await FindRecords(websiteKey, prodDataSource);
            Assert.IsNotEmpty(prodReturnValues);
            ConsoleTable.From<ScriptReturnValue>(prodReturnValues).Write();


        }
        [Test]
        public async Task MoveCollectionItemsToProd()
        {
            var devDataSource = "SmartAgentDev";
            var prodDataSource = "SmartAgentProd";
            var websiteKey = new Guid("e6b9299a-f903-4120-a481-3e5952fb98bc");

            Func<Guid, string, Task<IEnumerable<ScriptCollectionItem>>> FindRecords = async (key, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var devRepo = container.Resolve<ISmartAgentRepository>();
                    return await devRepo.FindScriptCollectionItems(key);
                }
            };

            Func<IEnumerable<ScriptReturnValue>, Task> MoveScriptsToProd = async (scriptReturnValues) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[prodDataSource].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var prodRepo = container.Resolve<ISmartAgentRepository>();
                    await prodRepo.AddScriptReturnValues(scriptReturnValues, websiteKey);
                }
            };

            var collectionItems = await FindRecords(websiteKey, devDataSource);
        }
        [Test]
        public async Task MoveScriptMasterRecordsToProd()
        {
            var devDataSource = "SmartAgentDev";
            var prodDataSource = "SmartAgentProd";
            var websiteKey = new Guid("e6b9299a-f903-4120-a481-3e5952fb98bc");

            Func<Guid, string, Task<IEnumerable<ScriptMaster>>> FindRecords = async (key, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var devRepo = container.Resolve<ISmartAgentRepository>();
                    return await devRepo.FindScriptMaster(key);
                }
            };

            Func<IEnumerable<ScriptMaster>, Task> MoveScriptsToProd = async (scriptMaster) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentProd"].ConnectionString))
                {
                    var container = new UnityContainer();
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var prodRepo = container.Resolve<ISmartAgentRepository>();
                    await prodRepo.AddScripts(scriptMaster);
                }
            };

            IEnumerable<ScriptMaster> scripts = await FindRecords(websiteKey, devDataSource);

            ConsoleTable.From<ScriptMaster>(scripts).Write();
            Assert.IsNotNull(scripts);

            await MoveScriptsToProd(scripts);

            IEnumerable<ScriptMaster> productionScripts = await FindRecords(websiteKey, prodDataSource);
            Assert.IsNotNull(productionScripts);
            ConsoleTable.From<ScriptMaster>(scripts).Write();
        }
        [Test]
        public async Task Update_Website_Url_Test()
        {
            IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodSmartAgentDb].ConnectionString);
            IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var uhc = WebsiteMaster.CreateWebsiteMaster(
                "Community Family Care",
                "https://www.capcms.com/capconnect/login.aspx",               
                "CFC",
                Guid.NewGuid(),
                 101
            );

            var container = new UnityContainer();
            container.RegisterType<IAsyncRepository<WebsiteMaster>, WebsiteMasterAsyncRepository>(new InjectionConstructor(devDb));
            var repo = container.Resolve<IAsyncRepository<WebsiteMaster>>();
            await repo.UpdateAsync(uhc);
            Console.WriteLine();
        }

        public async Task Create_Client_Mapping_Master_And_Values_Test()
        {
            var clientkey = new Guid("6C076431-F47B-46ED-A162-6B8BEA7F59B6");

            Func<Guid, string, Task<int>> CreateClientMappingValues = async (clientKey, source) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.CreateClientMappings(clientkey);
                }
            };
            var rowsAffected = await CreateClientMappingValues(clientkey, _devSmartAgent);
            if (rowsAffected > 0)
            {
                Console.WriteLine("Rows Affected: {0}", rowsAffected);
            }
        }
        public async Task Create_ClientMaster_Test()
        {
            var clientMaster = new ClientMaster(clientName: "ClientName", clientKey: new Guid("DF31493C-9654-4F86-9D04-F404B417167C"), howToDeliver: "ECNAUTH2");

            Func<ClientMaster, string, Task<Guid>> CreateClientMappingValues = async (record, source) =>
            {
                var container = new UnityContainer();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString))
                {
                    container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<ISmartAgentRepository>();
                    return await repo.AddClientMasterRecord(record);
                }
            };
            var clientKey = await CreateClientMappingValues(clientMaster, _devSmartAgent);
            if (clientKey != null)
            {
                Console.WriteLine("Rows Affected: {0}", clientKey);
            }
        }

    }
}
