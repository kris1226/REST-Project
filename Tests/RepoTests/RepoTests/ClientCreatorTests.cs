using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;
using Ninject;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using iAgentDataTool.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCreator;

namespace RepoTests
{
    [TestClass]
    public class ClientCreatorTests
    {
        IClientCreator _creator;
        //IClientCreator _TheCreator;
        IAsyncRepository<ClientMaster> _clientMasterRepo;
        ClientMaster _client;
        IList<ClientMaster> _clients;

        [TestMethod]
        public void IntialiseParams()
        {
            _client = new ClientMaster(
                clientName: "Test1", 
                clientKey: new Guid("A534FF12-2570-4ECB-A5CB-1550D13DA94A"), 
                howToDeliver: "ECNAUTH"
            );

            _clients = new List<ClientMaster>();
            _clients.Add(_client);
        }
        [TestInitialize]
        public void SetUp()
        {
            var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString);
            var kernel = new StandardKernel( new CreatorModule(smartAgentDb));
            _creator = kernel.Get<IClientCreator>();
            _clientMasterRepo = kernel.Get<IAsyncRepository<ClientMaster>>();

            IntialiseParams();            
        }

        [TestMethod]
        public async Task CreatorTests()
        {
            SetUp();
            await CreateClient();
        }
        public async Task CreateClient()
        {            
            await _creator.CreateClients(_clients);
            var clients = await _clientMasterRepo.FindWithGuidAsync(_client.ClientKey);
            foreach (var client in clients)
	        {
		        Assert.AreEqual(_client.ClientKey, client.ClientKey);
	        }
        }
    }
}
