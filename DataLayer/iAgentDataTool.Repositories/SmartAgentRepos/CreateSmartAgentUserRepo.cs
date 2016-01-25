
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.SmartAgentModels;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using iAgentDataTool.Models.Common;
using ScriptDataHelpers;


namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public class CreateSmartAgentUserRepo : ICreatSmartAgentClient, ISmartAgentRepo
    {
        private readonly IDbConnection _db;
        public CreateSmartAgentUserRepo(IDbConnection db)
	    {
            this._db = db;
	    }
        public async Task CreateSmartAgentClient(SmartAgentClient newClient)
        {
            var clientLocationKey = await CreateClientLocation(newClient);
            var clientkey = await CreateClientMasterRecord(newClient);
            await CreateClientAppsRecord(clientLocationKey, newClient.DeviceId);
            await CreateFacilityMasterRecord(newClient);
        }

        private async Task CreateClientAppsRecord(Guid clientLocationKey, string deviceId)
        {
            if (clientLocationKey.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var query = @"INSERT INTO [dbo].[dsa_clientApps]([dateAdded],[dateChanged],[lastUserID],[deviceID],[clientLocKey],[appKey])
			                  VALUES(GETDATE(),GETDATE(),@UpdatedBy,@DeviceId,@clientLocationKey,'7B1580D7-DD22-47F5-A369-B7C47B9132EA')";
                var p = new DynamicParameters();

                p.Add("@DeviceId",deviceId);
                p.Add("@UpdatedBy","kris.lindsey");
                p.Add("@clientLocationKey",clientLocationKey);

                try
                {
                    var key = await _db.ExecuteAsync(query, p);
                }
                catch (SqlException)
                {                    
                    throw;
                }
            }
            throw new ArgumentNullException("Need client Location key");
        }

        private async Task<Guid> CreateClientMasterRecord(SmartAgentClient client)
        {
            if (client.ClientName != null)
            {
                var query = @"INSERT INTO [dbo].[dsa_clientMaster]([dateAdded],[dateChanged],[lastUserID],[deviceID],[clientKey],[clientName],[imageDelay],[HowToDeliver])
                              VALUES(GETDATE(), GETDATE(),'kris.lindsey', @DeviceId, @ClientKey, @ClientName, 0,@HowToDeliver)
                               SELECT clientKey FROM dbo.dsa_clientMaster WHERE clientName =@ClientName";

                var p = new DynamicParameters();
                p.Add("@DeviceId",client.DeviceId);
                p.Add("@ClientKey",client.ClientKey);
                p.Add("@ClientName",client.ClientName);
                p.Add("@HowToDeliver",client.HowToDeilver);

                using (_db)
                {
                    var key = await _db.QueryAsync<Guid>(query, p);
                    return key.SingleOrDefault();
                }
            }
            throw new ArgumentNullException("Please provide data for entry");
        }
        public async Task<Guid> CreateClientLocation(SmartAgentClient newClient)
        {
            var clientLocationKey = await FindClientLocation(newClient.ClientLocationName);

            if (clientLocationKey == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                var term = "%" + newClient.ClientLocationName + "%";

                var query = @"INSERT INTO [dsa_clientLocations]([dateAdded],[dateChanged],[lastUserID],[deviceID],[clientKey],[clientLocationKey]
                             ,[clientLocationName], clientId, tpid, facilityId)
		                      VALUES(GETDATE() ,GETDATE(),'kris.lindsey',@deviceId, @clientKey ,@clientLocationKey,@clientLocationName, @clientId, @tpid, @facilityId)
                              SELECT clientLocationKey FROM dbo.dsa_clientLocations WHERE (clientLocationName LIKE @term)";

                var p = new DynamicParameters();

                p.Add("@clientId", newClient.ClientId);
                p.Add("@tpid", newClient.TpId);
                p.Add("@facilityId", newClient.FacilityId);
                p.Add("@term", newClient.ClientLocationName);
                p.Add("@deviceId", newClient.DeviceId);
                p.Add("@clientKey", newClient.ClientKey);
                p.Add("@clientLocationName", newClient.ClientLocationName);
                p.Add("@clientLocationKey", newClient.ClientLocationKey);

 
                   var key = await _db.QueryAsync<Guid>(query, p);
                   return key.SingleOrDefault();

            }
            throw new Exception("Client location already exsits");
        }
        private async Task<Guid> FindClientLocation(string clientLocationName)
        {
            if (clientLocationName != "")
            {
                string term = "%" + clientLocationName + "%";
                var query = @"select clientlocationkey from dbo.dsa_clientLocations where (clientLocationName like @term) ";
                try
                {
                    var clientLocationKey = await _db.QueryAsync<Guid>(query, new { term });
                    return clientLocationKey.SingleOrDefault();
                }
                catch (SqlException)
                {
                    return new Guid("00000000-0000-0000-0000-000000000000");
                }                    
            }
            throw new ArgumentNullException("Please provide client location name");
        }

        public async Task<Guid> CreateFacilityMasterRecord(SmartAgentClient client)
        {
            if (client.ClientLocationName != "")
            {
                var query = @"INSERT INTO [dbo].[dsa_facilityMaster]([FacilityKey],[FacilityName],[OrderMap],[ClientKey],[ClientLocationKey],[AutoRunUserID])
			                  VALUES(@facilityKey,@clientLocationName ,@OrderMap, CAST(@clientKey as varchar(50)),@clientLocationKey,NULL)";
                var p = new DynamicParameters();

                p.Add("@OrderMap", client.OrderMap);
                p.Add("@clientKey", client.ClientKey);
                p.Add("@clientLocationName", client.ClientLocationName);
                p.Add("@clientLocationKey", client.ClientLocationKey);
                p.Add("@facilityKey", client.FacilityKey);

                try
                {
                    var key = await _db.QueryAsync<Guid>(query, p);
                    return key.SingleOrDefault();
                }
                catch (SqlException)
                {                    
                    throw;
                }
            }
            throw new ArgumentNullException("Need facility key");
        }

        public async Task<Guid> CreateWebsiteMasterRecord(WebsiteMaster website)
        {
            if (String.IsNullOrWhiteSpace(website.WebsiteDescription))
            {
                throw new ArgumentNullException("Need website description to process.");
            }
            var webSiteExsits = await CheckForWebsiteRecord(website.WebsiteDescription);
            if (webSiteExsits)
            {
                return new Guid("00000000-0000-0000-0000-000000000000");
            }
            var query = @"IF NOT EXISTS 
                          (SELECT websiteDescription FROM dsa_websiteMaster 
                            WHERE websiteDescription = @websiteDescription)
                           BEGIN
                          INSERT INTO [ScriptingAgentDatabase].dbo.dsa_websiteMaster(
                            dateAdded,dateChanged,lastUserID, 
                            deviceID
                           , websitekey
                           , websiteDescription, 
                            websiteDomain, DontValidateHCPCS)
	                        VALUES (GetDate(),GetDate(),
                                    'kris.lindsey' ,@deviceId
                                    ,@websiteKey 
                                    ,@websiteDescription ,@websiteDomain ,0)
                            SELECT websiteKey FROM [ScriptingAgentDatabase].dbo.dsa_websiteMaster WHERE websiteDescription = @websiteDescription
                          END";

            var p = new DynamicParameters();

            p.Add("@deviceId", website.DeviceId);
            p.Add("@websiteDescription", website.WebsiteDescription);
            p.Add("@websiteDomain", website.WebsiteDomain);
            p.Add("@websiteKey", website.WebsiteKey);

            try
            {
               var websiteKey = await _db.QueryAsync<Guid>(query, p);
               return websiteKey.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }
        public async Task<bool> CheckForWebsiteRecord(string websiteDesciption)
        {
            if (string.IsNullOrWhiteSpace(websiteDesciption))
            {
                throw new ArgumentNullException("please provide a web description as search criteria.");
            }
            var term = "%" + websiteDesciption + "%";
            var query = @"SELECT websiteDescription FROM [ScriptingAgentDatabase].dbo.dsa_websiteMaster WHERE websiteDescription LIKE (@term)";
            try
            {
                var foundWebsite = await _db.QueryAsync<string>(query, new { term });

                if (string.IsNullOrWhiteSpace(foundWebsite.SingleOrDefault()))
                {
                    return false;
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task<WebsiteMaster> FindWebsiteRecord(Guid websiteKey)
        {
            var query = @"select deviceId, websiteKey, websiteDescription, websiteDomain
                          FROM dsa_websiteMaster 
                          WHERE websiteKey = @websiteKey";

            var p = new DynamicParameters();
            p.Add("@websiteKey", websiteKey);
            try
            {
               var result = await _db.QueryAsync<WebsiteMaster>(query, p);
               return result.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<Criteria>> CreateCriteraRecords(Criteria record)
        {
           // var arePropsNull = StaticHelpers.AnyPropsNull(record);
            //if (arePropsNull)
            //{
            //    throw new ArgumentNullException();
            //}

            var parameters = new DynamicParameters();

            parameters.Add("@criteriaSetName", record.CriteriaSetName);
            parameters.Add("@clientKey", record.ClientKey.ToString().ToUpper());
            parameters.Add("@clientLocationKey", record.ClientLocationKey.ToString().ToUpper());
            parameters.Add("@updatedBy", record.UpdatedBy);

            try
            {
                return await _db.QueryAsync<Criteria>("CreateCriteriaReocrds", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                return new List<Criteria>();
            }
        }

        public Task DeleteCritera(string criteraSetName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Criteria>> CreateCriteraRecord(Criteria newRecord)
        {
            var arePropsNull = StaticHelpers.AnyPropsNull(newRecord);
            if (arePropsNull)
            {
                throw new ArgumentNullException();
            }

            var parameters = new DynamicParameters();

            parameters.Add("@criteriaSetName", newRecord.CriteriaSetName);
            parameters.Add("@iprKey", newRecord.IprKey );
            parameters.Add("@cKey", newRecord.ClientKey.ToString());
            parameters.Add("@scriptKey", newRecord.ScriptKey);
            parameters.Add("@locKey", newRecord.ClientLocationKey.ToString());
            parameters.Add("@deviceId", newRecord.DeviceId);
            parameters.Add("@updatedBy", newRecord.UpdatedBy);
            parameters.Add("@criteriaSetKey", newRecord.CriteriaSetKey);

            try
            {
                return await _db.QueryAsync<Criteria>("sp_CreateCriteriaRecord", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                return null;
            }            
        }
        public async Task<WebsiteMaster> FindWebsiteRecord(string term)
        {
            var query = @"select deviceId, websiteKey, websiteDescription, websiteDomain
                          FROM dsa_websiteMaster 
                          WHERE websiteDescription like (@websiteDescription)";
            var websiteDescription = "%" + term +"%";

            var p = new DynamicParameters();
            p.Add("@websiteDescription", websiteDescription);
            try
            {
                var result = await _db.QueryAsync<WebsiteMaster>(query, p);
                return result.SingleOrDefault();
            }
            catch (SqlException)
            {
                throw;
            }
        }


        public async Task CreatePayerWebsiteMappingValues(Guid clientkey, Guid clientLocationkey)
        {
            var p = new DynamicParameters();
            p.Add("@clientKey", clientkey);
            p.Add("@clientLocationKey", clientLocationkey);

            try
            {
                await _db.ExecuteReaderAsync("CreatePayerWebsiteMappingValues", p, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<int> AddPayerWebsiteMappingValue(IEnumerable<PayerWebsiteMappingValue> payerWebsiteMappingValue)
        {
            IEnumerable<int> result = null;
            var p = new DynamicParameters();

            foreach (var item in payerWebsiteMappingValue)
            {
                p.Add("@iprkey", item.Primary_PayerKey);
                p.Add("@websitekey", item.WebsiteKey);
                p.Add("@detialValue", item.DefaultValue);
                p.Add("@clientKey", item.ClientKey);
                p.Add("@clientLocationKey", item.ClientLocationKey);
                p.Add("@detailLabel", item.DetailLabel);

                try
                {
                    result =  await _db.QueryAsync<int>("AddPayerWebsiteMappingValue", p, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException)
                {
                    return result.SingleOrDefault();
                    throw;
                }
            }
            return result.SingleOrDefault();
        }
    }
}
