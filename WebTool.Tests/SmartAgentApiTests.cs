using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Moq;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace WebTool.Tests
{
    /// <summary>
    /// Summary description for SmartAgentApiTests
    /// </summary>
    [TestClass]
    public class SmartAgentApiTests
    {
        public SmartAgentApiTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public async Task GetFacilitesTest()
        //{
        //    IDbConnection smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["SmartAgentDev"].ConnectionString);
        //    IKernel kerenl = new StandardKernel(new ApiControllersModule(smartAgentDb));
        //    var facilityController = kerenl.Get<FacilitiesController>();
        //    var facilites = await facilityController.GetFacilites();
        //    Console.WriteLine(facilites);

        //    var facilityRepoMock = new Mock<IAsyncRepository<FacilityMaster>>();

        //}
    }
}
