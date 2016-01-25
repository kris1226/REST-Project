using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent;
using Ninject;
using iAgentDataTool.Repositories.SmartAgentRepos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models;
using iAgentDataTool.Repositories.Interfaces;
using iAgentDataTool.AsyncRepositories.Common;
using RepoTests.Factories;



namespace RepoTests {
    [TestFixture]
    public class ScriptRepoTests {

        private Action<object> write = w => Console.WriteLine(w);
        private readonly string _devSmartAgent = "SmartAgentDev";
        private readonly string _prodAppConfigName = "SmartAgentProd";
        private IAsyncRepository<ScriptMaster> _scriptRepo;

        //  private readonly string _devIAgent = "RemixDb";
        [SetUp]
        public void Setup()
        {
            var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString);
            var kernel = new StandardKernel(new ScriptCreationModule(smartAgentDb));
            _scriptRepo = kernel.Get<IAsyncRepository<ScriptMaster>>();
        }

        [Test]
        public async void Update_Script_Records()
        {
            var newCode = "SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=A FORM=ACTION:* ATTR=TXT:Member\nTAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:* ATTR=NAME:*tbMemberID CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=ACTION:* ATTR=NAME:*SearchMemberCtrl$btSearch\n";
            var websiteKey = new Guid("89bd591b-8cbf-e111-b39a-000c29729dff");
            var scriptKey = new Guid("8294dbd1-acbf-e111-b39a-000c29729dff");
            var scriptDesc = "KePro Submit 002: Member search";
            var category = "PatientSearch";
            var deviceId = "002";


            var keproScript001 = ScriptMaster.Build()
                            .WithScriptKey(scriptKey)
                            .WithScriptDesc(scriptDesc)
                            .WithWebsiteKey(websiteKey)
                            .WithScriptCode(newCode)
                            .WithNumberOfIterations(0)
                            .WithCategory(category)
                            .WithDeviceId(deviceId)
                            .Build();

            Action<ScriptMaster> UpdateScriptCode = async (script) => {
                await _scriptRepo.UpdateAsync(script);
            };
            try
            {
                UpdateScriptCode(keproScript001);
            }
            catch (Exception) {
                                   
            }                             
            finally {
                Console.WriteLine("process complete");
            }
        }

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
            var record = ClientMaster.CreateClientMaster(
                "Good Samaritan Hospital",
                new Guid("3bc45110-cad0-4f2a-bd2d-efccda2732d4"),
                "ECNAUTH2"
            );

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

            var w = new WebsiteMaster();
            w.DeviceId = "OptumRad";
            w.WebsiteDescription = "Optum UHC Rad Submit";
            w.WebsiteDomain = "https://www.unitedhealthcareonline.com/b2c/Logout.do?page=signin";
            w.WebsiteKey = websitekey;

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
            var prodDataSource = _prodAppConfigName;

            var result = await FindWebsiteRecord(w.WebsiteDescription, dataSource);

            if (result == null)
            {
                websitekey = await CreateWebsiteRecord(w, dataSource);
                write(websitekey);
            }
            write(result.WebsiteDescription + " " + result.WebsiteKey);

            var result2 = await FindWebsiteRecord(w.WebsiteDescription, prodDataSource);
            if (result2 == null)
            {
                websitekey = await CreateWebsiteRecord(w, prodDataSource);
                write(websitekey);
            }
            write(result.WebsiteDescription + " " + result.WebsiteKey);

        }

        [Test]
        public async Task Create_Script_Record_Test()
        {
            Action<object> writeOut = value => Console.WriteLine(value);

            var container = new UnityContainer();
            var scripts = new List<Script>();


            //IEnumerable<ScriptReturnValue> rtv = null;

            var websiteDescription = "Onesource BCBS Flordia Inquiry ";
            var websiteKey = new Guid("54fd645a-10bd-4197-b111-a764268dcc20");
            var deviceId = "OSFlordia";

            var script1 = Script.CreateScript
            (
                websiteDescription + ":001 Login Script, error check",
                @"SET !TIMEOUT_STEP 5\nURL GOTO=%%websiteDomain%%\nTAG POS=1 TYPE=INPUT:TEXT ATTR=NAME:*LoginUN CONTENT=%%websiteUsername%%\nTAG POS=1 TYPE=INPUT:PASSWORD ATTR=NAME:*LoginPWD CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:* ATTR=NAME:btnSubmit\nTAG POS=1 TYPE=FONT ATTR=TXT:User<SP>Login EXTRACT=TXT\n",
                deviceId + "_001",
                "Login",
                 websiteKey
            );
            scripts.Add(script1);

            var script2 = Script.CreateScript
            (
                websiteDescription + "002: goto referral page",
                @"SET !TIMEOUT_STEP 12\nTAG POS=1 TYPE=A FORM=NAME:frm ATTR=TXT:Referrals<SP>&<SP>Precerts\nTAG POS=1 TYPE=FONT FORM=NAME:frm ATTR=TXT:BCBS<SP>of<SP>Florida\nFRAME NAME=meat\nTAG POS=1 TYPE=SELECT FORM=NAME:* ATTR=NAME:*_NPI CONTENT=%1922032424\n",
                deviceId + "_002",
                "PatientSearch",
                 websiteKey
            );
            scripts.Add(script2);

            var script3 = Script.CreateScript
            (
                websiteDescription + "003: patient demographics",
                @"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*_SMI CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*_SLN CONTENT=%%PatLname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*_SFN CONTENT=%%PatFname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*_SDOB CONTENT=%%PatDOB%%\n",
                deviceId + "_003",
                "PatientSearch",
                 websiteKey
            );
            scripts.Add(script3);


            var script4 = Script.CreateScript(
                websiteDescription + "004: Service start Date",
                @"SET !TIMEOUT_STEP 4\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:PDQMSForm ATTR=NAME:*_BDOS CONTENT=%%ServiceDate%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:PDQMSForm ATTR=NAME:*_EDOS CONTENT=%%ServiceDate%%\n",
                deviceId + "_004",
                "PatientSearch",
                 websiteKey
            );
            scripts.Add(script4);

            var script5 = Script.CreateScript
          (
              websiteDescription + "005: check if question was incorrect",
              @"PAUSESUBMIT|SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n",
              deviceId + "_005",
              "Extraction",
               websiteKey
          );
            scripts.Add(script5);

            var script6 = Script.CreateScript
             (
                 websiteDescription + "006: Check if on UHC page",
                 @"PAUSEERR|SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n",
                 deviceId + "_006",
                 "Extraction",
                  websiteKey
             );
            scripts.Add(script6);

            //var script6a = Script.CreateScript
            // (
            //     websiteDescription + "006: Optum transtion page",
            //     @"SET !TIMEOUT_STEP 5\nSET !ERRORIGNORE YES\nTAG POS=1 TYPE=H3 ATTR=TXT:UnitedHealthcare<SP>Online\nTAG POS=1 TYPE=A ATTR=TXT:Radiology<SP>Notification<SP>&<SP>Authorization<SP>-<SP>Submission<SP>&<SP>Status\nWAIT SECONDS=1\n",
            //     deviceId + "_006",
            //     "Login",
            //      websiteKey
            // );
            //scripts.Add(script6a);

            //var script7 = Script.CreateScript
            // (
            //     websiteDescription + "007: Check for tax id",
            //     @"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=SELECT FORM=NAME:* ATTR=NAME:corpTaxid EXTRACT=TXT\n",
            //     deviceId + "_007",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script7);

            //var script8 = Script.CreateScript
            // (
            //     websiteDescription + "007: CUHC Requesting Provider",
            //     @"SET !TIMEOUT_STEP 3\nSET !TIMEOUT_STEP 15\nWAIT SECONDS=2\nTAG POS=1 TYPE=SELECT ATTR=NAME:provName CONTENT=$[[UHCRadRequestingProvider[[\n",
            //     deviceId + "_008",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script8);

            //var script9 = Script.CreateScript
            // (
            //     websiteDescription + "009: Go to Submit page",
            //     @"SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=INPUT:BUTTON ATTR=NAME:Submit2\nTAB T=2\nTAB CLOSEALLOTHERS\nWAIT SECONDS=7\nTAG POS=1 TYPE=A FORM=NAME:* ATTR=TXT:Submit<SP>Clinical<SP>Request\nONDIALOG POS=1 BUTTON=NO\nTAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:* ATTR=NAME:*imgUnited\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*txtRequestorsName CONTENT=%%UserContactName%%\n",
            //     deviceId + "_009",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script9);

            //var script10 = Script.CreateScript
            // (
            //     websiteDescription + "010: Enter Fax number",
            //     @"SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*Physician$txtFax CONTENT=%%ProviderFax%%\n",
            //     deviceId + "_010",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script10);

            //var script11= Script.CreateScript
            // (
            //     websiteDescription + "011: Check for invalid fax number entry",
            //     @"SET !TIMEOUT_STEP 4\nTAG POS=1 TYPE=LI FORM=NAME:* ATTR=TXT:Please<SP>enter<SP>valid<SP>fax<SP>number<SP>in<SP>format<SP>(XXX)XXX-XXXX<SP>or<SP>XXXXXXXXXX EXTRACT=TXT\n",
            //     deviceId + "_011",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script11);

            //var script12 = Script.CreateScript
            // (
            //     websiteDescription + "012: Patient Demographics",
            //     @"SET !TIMEOUT_STEP 4\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:* ATTR=NAME:*Physician$btnSelectPhysician\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*txtSearchPatientID CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*txtSearchLastname CONTENT=%%PatLname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*txtFirstName CONTENT=%%PatFname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:*SearchDateOfBirth CONTENT=%%PatDOB%%\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:* ATTR=NAME:*Patient$btnSearch\n",
            //     deviceId + "_012",
            //     "PatientSearch",
            //      websiteKey
            // );
            //scripts.Add(script12);

            //var script3 = Script.CreateScript
            //(
            //websiteDescription + "003: Pause For submit",
            //@"PAUSESUBMIT|SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n",
            //deviceId + "_003",
            //"Extraction",
            //websiteKey
            //);
            //scripts.Add(script3);



            //var script14 = Script.CreateScript
            // (
            //     websiteDescription + "004: Error pause",
            //     @"PAUSEERR|SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n",
            //     deviceId + "_004",
            //     "Extraction",
            //      websiteKey
            // );
            //scripts.Add(script14);

            //var script5 = Script.CreateScript
            // (
            //     websiteDescription + "005: Logout",
            //     @"SET !TIMEOUT_STEP 3\nONDIALOG POS=1 BUTTON=NO\nTAG POS=1 TYPE=A FORM=NAME:frm ATTR=TXT:LOGOUT\n",
            //     deviceId + "_005",
            //     "Logout",
            //      websiteKey
            // );
            //scripts.Add(script5);


            Func<Script, string, Task<Guid>> AddScriptMasterRecord = async (sm, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<IScriptCreation>();
                    return await repo.CreateScritp(sm);
                }
            };

            Func<ScriptReturnValue, string, Task<IEnumerable<ScriptReturnValue>>> AddReturnValues = async (rv, connectionString) =>
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
                {
                    container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));
                    var repo = container.Resolve<IScriptCreation>();
                    return await repo.CreateReturnValues(rv);
                }
            };

            foreach (var script in scripts)
            {
                var result = await AddScriptMasterRecord(script, _devSmartAgent);
                write(result);

                if (result != null)
                {
                    var returnValue = new ScriptReturnValue();

                    returnValue.ScriptKey = result;
                    returnValue.DeviceId = script.DeviceId;
                    returnValue.WhenEqualScripKey = new Guid("00000000-0000-0000-0000-000000000000");
                    returnValue.WhenNotEquelScriptKey = new Guid("00000000-0000-0000-0000-000000000000");
                    returnValue.MappingValue = null;
                    returnValue.ReturnValue = "SUCCESS";

                    var resultScripts = await AddReturnValues(returnValue, _devSmartAgent);

                    foreach (var returnScript in resultScripts)
                    {
                        write(returnScript.ScriptKey + " " + returnScript.DeviceId);
                    }
                }
                Console.WriteLine("error adding record", result);
            }
        }
        [Test]
        public async Task Create_Return_Values_Test()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);
            var returnValue = new ScriptReturnValue();
            var container = new UnityContainer();
            container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));

            returnValue.ScriptKey = new Guid("f902e1ec-9f1a-e511-96c2-000c29729dff");
            returnValue.WhenEqualScripKey = new Guid("00000000-0000-0000-0000-000000000000");
            returnValue.WhenNotEquelScriptKey = new Guid("00000000-0000-0000-0000-000000000000");
            returnValue.DeviceId = "INOVHealth003";
            returnValue.MappingValue = null;
            returnValue.ReturnValue = "Invalid username and/or password.";

            var repo = container.Resolve<IScriptCreation>();
            var rv = await repo.CreateReturnValues(returnValue);

            foreach (var item in rv)
            {
                Console.WriteLine(item.ScriptKey);
            }
        }
        [Test]
        public async Task Create_Collection_Item_Test()
        {
            var container = new UnityContainer();
            var collectionItem = new Dictionary<string, Guid>();
            var collectionItems = new List<ScriptCollectionItem>();

            Func<ScriptCollectionItem, Task<ScriptCollectionItem>> CreateCollectionItems = async (c) =>
            {
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString))
                {
                    container.RegisterType<IScriptCreation, ScriptCreationRepo>(new InjectionConstructor(db));

                    var repo = container.Resolve<IScriptCreation>();
                    return await repo.CreateCollectionItems(c);
                }
            };

            collectionItem.Add("WebsiteDomain", new Guid("96F51FD2-6539-49BD-A1C8-1F8DDE73CE1E"));
            collectionItem.Add("Username", new Guid("D8BB9D68-DC56-E011-B21D-001E4F27A50B"));
            collectionItem.Add("websitePassword", new Guid("D9BB9D68-DC56-E011-B21D-001E4F27A50B"));
            collectionItem.Add("MemberID", new Guid("AAC881E8-ECB6-4B9B-835E-A56C89E5973F"));
            collectionItem.Add("PatientDOB", new Guid("B6147C25-8F9A-E411-82F5-000C29729DFF"));
            collectionItem.Add("PatientLastName", new Guid("D19691B4-9E92-E411-82F5-000C29729DFF"));
            collectionItem.Add("PatientFirstName", new Guid("D09691B4-9E92-E411-82F5-000C29729DFF"));
            collectionItem.Add("ToDate", new Guid("A17CC0FD-170A-4F75-AB50-44BA8AA064F5"));
            collectionItem.Add("ServiceDate", new Guid("2917715D-C7F5-E011-ABE5-000C29729DFF"));
            collectionItem.Add("SearchType", new Guid("c75bcc73-5356-e011-b21d-001e4f27a50b"));
            collectionItem.Add("3PartDate", new Guid("62593b00-4777-e011-b21d-001e4f27a50b"));
            collectionItem.Add("Health Plan", new Guid("b59302d2-226c-e011-b21d-001e4f27a50b"));
            collectionItem.Add("HighmarkRequestingProvider", new Guid("44F4842D-88D3-E211-81D2-000C29729D00"));
            collectionItem.Add("UHCRadRequestingProvider", new Guid("8c379ceb-11c8-e011-a8c4-000c29729dff"));
            collectionItem.Add("AetnaRequestingProvider", new Guid("2e9c25ae-d25b-e011-b21d-001e4f27a50b"));
            collectionItem.Add("State", new Guid("7B466BB8-D0A6-49DC-876A-2B237ADAC07C"));
            collectionItem.Add("UserContactName", new Guid("6A3E2FF8-07F5-4657-ACDC-3AABEE967A21"));
            collectionItem.Add("UserContactPhone", new Guid("f3fbbdfe-e84c-4dea-b471-0883a443ca0f"));
            collectionItem.Add("TextEntry", new Guid("154f0a0c-f1a4-e511-96c2-000c29729dff"));
            collectionItem.Add("ProviderFax", new Guid("1043b0fa-5f3d-e111-a475-000c29729dff"));

            var deviceId = "OSAetna";

            var websiteDomain = new ScriptCollectionItem();
            websiteDomain.FieldKey = collectionItem["WebsiteDomain"];
            websiteDomain.ScriptKey = new Guid("ca8754e0-8eb4-e511-96c2-000c29729dff");
            websiteDomain.OverrideLabel = "Website Domain";
            websiteDomain.DeviceId = deviceId + "_001";
            collectionItems.Add(websiteDomain);

            var username = new ScriptCollectionItem();
            username.FieldKey = collectionItem["Username"];
            username.ScriptKey = new Guid("ca8754e0-8eb4-e511-96c2-000c29729dff");
            username.OverrideLabel = "Username";
            username.DeviceId = deviceId + "_001";
            collectionItems.Add(username);

            var websitePassword = new ScriptCollectionItem();
            websitePassword.FieldKey = collectionItem["websitePassword"];
            websitePassword.ScriptKey = new Guid("ca8754e0-8eb4-e511-96c2-000c29729dff");
            websitePassword.OverrideLabel = "website Password";
            websitePassword.DeviceId = deviceId + "_001";
            collectionItems.Add(websitePassword);

            //var userContactName = new ScriptCollectionItem();
            //userContactName.FieldKey = collectionItem["UserContactPhone"];
            //userContactName.ScriptKey = new Guid("31a4e68f-91a5-e511-96c2-000c29729dff");
            //userContactName.OverrideLabel = "Phone Number";
            //userContactName.DeviceId = deviceId + "_002";
            //collectionItems.Add(userContactName);

            //var uhcRadRequestingProvider = new ScriptCollectionItem();
            //uhcRadRequestingProvider.FieldKey = collectionItem["UHCRadRequestingProvider"];
            //uhcRadRequestingProvider.ScriptKey = new Guid("a30a7eea-d28e-e511-96c2-000c29729dff");
            //uhcRadRequestingProvider.OverrideLabel = "Requesting Provider";
            //uhcRadRequestingProvider.DeviceId = deviceId + "_008";
            //collectionItems.Add(uhcRadRequestingProvider);

            //var requesterName = new ScriptCollectionItem();
            //requesterName.FieldKey = collectionItem["UserContactName"];
            //requesterName.ScriptKey = new Guid("a40a7eea-d28e-e511-96c2-000c29729dff");
            //requesterName.OverrideLabel = "Enter requester name";
            //requesterName.DeviceId = deviceId + "_009";
            //collectionItems.Add(requesterName);

            //var faxNumber = new ScriptCollectionItem();
            //faxNumber.FieldKey = collectionItem["ProviderFax"];
            //faxNumber.ScriptKey = new Guid("a50a7eea-d28e-e511-96c2-000c29729dff");
            //faxNumber.OverrideLabel = "Enter provider's fax number";
            //faxNumber.DeviceId = deviceId + "_010";
            //collectionItems.Add(faxNumber);

            var memberId = new ScriptCollectionItem();
            memberId.FieldKey = collectionItem["MemberID"];
            memberId.ScriptKey = new Guid("31a4e68f-91a5-e511-96c2-000c29729dff");
            memberId.OverrideLabel = "Enter member Id";
            memberId.DeviceId = deviceId + "_002";
            collectionItems.Add(memberId);

            var firstName = new ScriptCollectionItem();
            firstName.FieldKey = collectionItem["PatientFirstName"];
            firstName.ScriptKey = new Guid("31a4e68f-91a5-e511-96c2-000c29729dff");
            firstName.OverrideLabel = "Enter patient first name";
            firstName.DeviceId = deviceId + "_002";
            collectionItems.Add(firstName);

            //var lastName = new ScriptCollectionItem();
            //lastName.FieldKey = collectionItem["PatientLastName"];
            //lastName.ScriptKey = new Guid("c88c89f2-e3a4-e511-96c2-000c29729dff");
            //lastName.OverrideLabel = "Enter patient first name";
            //lastName.DeviceId = deviceId + "_002";
            //collectionItems.Add(lastName);

            var pateintDateOfBirth = new ScriptCollectionItem();
            pateintDateOfBirth.FieldKey = collectionItem["PatientDOB"];
            pateintDateOfBirth.ScriptKey = new Guid("31a4e68f-91a5-e511-96c2-000c29729dff");
            pateintDateOfBirth.OverrideLabel = "Enter patient date of birth";
            pateintDateOfBirth.DeviceId = deviceId + "_002";
            collectionItems.Add(pateintDateOfBirth);


            //var requestionProvider = new ScriptCollectionItem();
            //requestionProvider.FieldKey = collectionItem["ServiceDate"];
            //requestionProvider.ScriptKey = new Guid("f44474d3-f083-e511-96c2-000c29729dff");
            //requestionProvider.OverrideLabel = "Service Date";
            //requestionProvider.DeviceId = deviceId + "_003a";
            //collectionItems.Add(requestionProvider);

            //var serviceDate = new ScriptCollectionItem();
            //serviceDate.FieldKey = collectionItem["ServiceDate"];
            //serviceDate.ScriptKey = new Guid("f44474d3-f083-e511-96c2-000c29729dff");
            //serviceDate.OverrideLabel = "Service Date";
            //serviceDate.DeviceId = deviceId + "_003a";
            //collectionItems.Add(serviceDate);


            foreach (var item in collectionItems)
            {
                var result = await CreateCollectionItems(item);
                write(result.OverrideLabel + " " + result.ScriptKey);
            }
        }
        [Test]
        public async Task Add_ExtractionMap_Test()
        {
            var container = new UnityContainer();
            var em = new WebsiteExtractionMap();

            em.WebsiteKey = new Guid("526d0fa1-8198-4e27-92cc-7ba692fb721d");
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
            var prodRecord = await AddRecord(em, _prodAppConfigName);
            Console.WriteLine(prodRecord.WebsiteKey.ToString());
        }
        [Test]
        public async Task Add_ClientScripts_Test()
        {
            var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString);
            var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);
            var container = new UnityContainer();
            var websiteKey = new Guid("9b82289b-6f72-e511-96c2-000c29729dff");

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(devDb));
            var devRepo = container.Resolve<ISmartAgentRepository>();



            var scripts = await devRepo.FindScripts(websiteKey);

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(prodDb));
            var prodRepo = container.Resolve<ISmartAgentRepository>();

            var results = await prodRepo.AddScripts(scripts);

            foreach (var item in results)
            {
                Console.WriteLine(item.ScriptCode.ToString());
            }
        }
        [Test]
        public async Task MoveReturnValuesToProdTest()
        {
            var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString);
            var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var container = new UnityContainer();
            var websiteKey = new Guid("11c6bc55-4014-e511-96c2-000c29729dff");

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(devDb));
            var devRepo = container.Resolve<ISmartAgentRepository>();

            var returnValues = await devRepo.FindScriptReturnValues(websiteKey);

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(prodDb));
            var prodRepo = container.Resolve<ISmartAgentRepository>();
            await prodRepo.AddScriptReturnValues(returnValues);

        }
        [Test]
        public async Task MoveCollectionItemsToProd()
        {
            var prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString);
            var devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var container = new UnityContainer();
            var websiteKey = new Guid("11c6bc55-4014-e511-96c2-000c29729dff");

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(devDb));
            var devRepo = container.Resolve<ISmartAgentRepository>();

            var collectionItems = await devRepo.FindCollectionItems(websiteKey);

            container.RegisterType<ISmartAgentRepository, SmartAgentRepo>(new InjectionConstructor(prodDb));
            var prodRepo = container.Resolve<ISmartAgentRepository>();
            await prodRepo.AddScriptCollectionItems(collectionItems);
        }
        [Test]
        public async Task Update_Website_Url_Test()
        {
            IDbConnection prodDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString);
            IDbConnection devDb = new SqlConnection(ConfigurationManager.ConnectionStrings[_devSmartAgent].ConnectionString);

            var uhc = new WebsiteMaster();
            uhc.WebsiteKey = new Guid("aef4b705-176a-e111-90b9-000c29729dff");
            uhc.WebsiteDomain = "https://www.unitedhealthcareonline.com/b2c/CmaAction.do?viewKey=PreLoginMain&forwardToken=UserLogin";

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
            var clientMaster = ClientMaster.CreateClientMaster(
                "Citizens Medical Center",
                new Guid("DF31493C-9654-4F86-9D04-F404B417167C"),
                "ECNAUTH2"
            );

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
