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


namespace RepoTests
{
    [TestClass]
    public class UpwRepoTests
    {
        IAsyncRepository<Upw> _repo;
        IUpwAsyncRepository _upwRepo;
        Upw _upw = new Upw();

        /// <summary>
        /// Sets up.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            var smartAgentDb = new SqlConnection(ConfigurationManager.ConnectionStrings["UPW"].ConnectionString);
            var kernel = new StandardKernel(new RepoTestsModule(smartAgentDb));
            _repo = kernel.Get<IAsyncRepository<Upw>>();
            _upwRepo = kernel.Get<IUpwAsyncRepository>();
            InitialiseParams();
        }
        [TestMethod]
        public void InitialiseParams()
        {
            _upw.ClientKey = "";
            _upw.SqlDb = "Martin";
            _upw.EntKey = "01496000";
            _upw.SiteKey = "01496D000";
        }
        [TestMethod]
        public async Task UpwTests()
        {
            SetUp();
            await Finding_Sql_Db_From_Upw(_upw.SqlDb);
        }
        /// <summary>
        /// Find Upw Record with entKey
        /// </summary>
        public async Task Find_Upw_Records_with_EntKey(string entKey)
        {
            // Arrange
            // Act
            var upwRecords = await _upwRepo.FindWithEntKey(entKey);

            if (upwRecords.Count() > 0)
            {
                foreach (var record in upwRecords)
                {
                    Console.WriteLine("Records Found!.. {0}", record);
                    Assert.IsTrue(upwRecords.Count() > 0, "Found at least 1 record");
                }
            }
            else
            {
                Console.WriteLine("No Records found!");
            }
        }
        /// <summary>
        /// Find Upw Record
        /// </summary>
        public async Task Finding_Sql_Db_From_Upw(string databaseName)
        {
            // Arrange
            // Act
            var upwRecords = await _repo.FindByName(databaseName);

            if (upwRecords.Count() > 0)
            {
                foreach (var record in upwRecords)
                {
                    Console.WriteLine("Records Found!.. {0}", record);
                    Assert.IsTrue(upwRecords.Count() > 0, "Found at least 1 record");                    
                }
            }
            else
            {
                Console.WriteLine("No Records found!");
            }
        }
    }
}
