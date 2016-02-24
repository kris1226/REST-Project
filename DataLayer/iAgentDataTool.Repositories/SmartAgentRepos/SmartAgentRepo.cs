using iAgentDataTool.Models.Common;
using iAgentDataTool.Models;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using log4net;
using System.Reflection;


namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public class SmartAgentRepo : ISmartAgentRepository
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDbConnection _db;

        public SmartAgentRepo(IDbConnection db)
        {
            this._db = db;
        }

        public Task<IEnumerable<ClientApps>> FindClientAppsRecords(Guid clientLocationKey)
        {
            throw new NotImplementedException();
        }

        public Task AddClientAppsRecord(IEnumerable<ClientApps> clientApps)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientMaster> FindClientMasterRecord(Guid clientKey)
        {
            var query = @"SELECT DISTINCT clientKey, clientName, HowToDeliver, DeviceId 
                          FROM dsa_clientMaster WHERE clientKey =@clientKey";
            try
            {
                logger.Debug("Searching for client master record.");
                var clientRecord = await _db.QueryAsync<ClientMaster>(query, new { clientKey });
                logger.Debug("Search complete!.");
                return clientRecord.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Debug("Error getting client maste record: {0}", ex);
                throw;
            }
        }

        public async Task<Guid> AddClientMasterRecord(ClientMaster clientMasterRecord)
        {
            var query = @"INSERT INTO [dbo].[dsa_clientMaster]([dateAdded],[dateChanged],[lastUserID],[deviceID],[clientKey],[clientName],[imageDelay],[HowToDeliver])
				        VALUES(GETDATE(), GETDATE(),'kris.lindsey', @DeviceId, @clientKey, @clientName, 0,@howToDeliver)
                        SELECT clientKey from dsa_clientMaster where clientName = @clientName";

            var p = new DynamicParameters();

            p.Add("@clientName", clientMasterRecord.ClientName);
            p.Add("@howToDeliver", clientMasterRecord.HowToDeliver);
            p.Add("@DeviceId", clientMasterRecord.DeviceId);
            p.Add("@clientKey", clientMasterRecord.ClientKey);

            try
            {
                logger.Debug("Attempting to add client record");
                var result = await _db.QueryAsync(query, p);                
                logger.Debug("No errors return from add attempt");
                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.Debug("Error adding client: {0} ", ex);
                return new Guid("00000000-0000-0000-0000-000000000000");
            }
        }

        public async Task<IEnumerable<ClientMappingMaster>> FindClientMappingMasterRecords(Guid clientKey)
        {
            var sql = @"SELECT clientKey, fieldKey, UseExternalTable FROM dsa_clientMappingMaster where clientKey = @clientKey";
            try
            {
                logger.Debug("Getting Client Mapping Master records");
                var clientMappingMasterRecords = await _db.QueryAsync<ClientMappingMaster>(sql, new { clientKey });
                logger.Debug("Retrived Client Mapping Masters successfully");
                return clientMappingMasterRecords;
            }
            catch (SqlException ex)
            {
                logger.Debug("Error getting Mapping Master records {0}", ex);
                throw;
            }
        }

        public async Task AddClientMappingMasterRecords(Guid clientKey)
        {
            if (clientKey.Equals(null))
            {
                logger.Debug("clientKey was null or empty");
                throw new ArgumentNullException("Please provide a client Guid value");
            }
            else
            {
                var sql = @"INSERT INTO dsa_clientMappingMaster(clientKey, fieldKey, useExternalTable)
				            VALUES (@clientKey, '7F8C762B-EA7B-E211-97DF-000C29729DFF', 0),
		                           (@clientKey, '03D86C0C-B22F-E011-BF23-001E4F27A50B', 0),
					               (@clientKey, 'C5DE7649-BED7-4CFC-B758-70DE4BC33213', 0)";
                try
                {
                    var p = new DynamicParameters();

                    p.Add("@clientKey", clientKey);

                    logger.Debug("Adding Client Mapping Master records");
                    await _db.QueryAsync<ClientMappingMaster>(sql, p);
                    logger.Debug("adding Client Mapping Master records returned no errors");
                }
                catch (SqlException ex)
                {
                    logger.Debug("Error Adding Mapping Master records {0}", ex);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<ClientMappingValue>> FindClientMappingValues(Guid clientKey)
        {
            if (clientKey.Equals(null))
            {
                logger.Debug("No clientKey used");
                throw new ArgumentNullException("uh oh! ClienKey value is empty or null. Please provide a clientKey guid");
            }
            else
            {
                var query = @"SELECT clientKey, websiteKey, fieldKey, fieldValue, NormalizedValue FROM dsa_clientMappingValues
                          WHERE clientKey = @clientKey";
                try
                {
                    logger.Debug("Adding Client Mapping Values records");
                    return await _db.QueryAsync<ClientMappingValue>(query, new { clientKey });
                }
                catch (SqlException ex)
                {
                    logger.Debug("Error adding Client Mapping Values records: {0}", ex);
                    throw;
                }
            }
        }

        public async Task AddClientMappingValues(Guid clientKey)
        {
            if (clientKey.Equals(null))
            {
                logger.Debug("Client Key guid was not found");
                throw new ArgumentNullException("Client guid is empty or null");
            }
            else
            {
                var query = @"INSERT INTO dsa_clientMappingValues(clientKey, websitekey, fieldKey, fieldValue, NormalizedValue)
				                VALUES (@clientKey,'522BB0AC-8CC0-E111-B39A-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '0', 'INP1'),
					                   (@clientKey,'522BB0AC-8CC0-E111-B39A-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '1', 'OUT1'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '2', 'CHILD'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '1', 'OUT1'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '0', 'INP1'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '1', 'SPOUSE'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '2', 'REF1'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '5', 'SELF'),
					                   (@clientKey,'7DC329C4-7A69-E211-97DF-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '4', 'OTHER'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '0', 'INP1'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '1', 'OUT1'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '1', 'SPOUSE'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','03D86C0C-B22F-E011-BF23-001E4F27A50B', '2', 'REF1'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '2', 'CHILD'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '4', 'OTHER'),
					                   (@clientKey,'2A317730-21C1-E111-B39A-000C29729DFF','C5DE7649-BED7-4CFC-B758-70DE4BC33213', '5', 'SELF'),
					                   (@clientKey,'fa7afcf9-aace-e211-a3ca-000c29729dff','03D86C0C-B22F-E011-BF23-001E4F27A50B', '0', 'INP1'),
					                   (@clientKey,'fa7afcf9-aace-e211-a3ca-000c29729dff','03D86C0C-B22F-E011-BF23-001E4F27A50B', '1', 'OUT1'),
					                   (@clientKey,'fa7afcf9-aace-e211-a3ca-000c29729dff','03D86C0C-B22F-E011-BF23-001E4F27A50B', '2', 'REF1')";
                var p = new DynamicParameters();
                p.Add("@clientKey", clientKey);

                try
                {
                    logger.Debug("Adding ClientMapping Values");
                    await _db.ExecuteAsync(query, p);
                }
                catch (SqlException ex)
                {
                    logger.Debug("Error Adding ClientMapping Values: {0}", ex);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<ClientLocations>> FindClientLocationRecords(Guid clientKey)
        {
            if (clientKey.Equals(null))
            {
                logger.Debug("Client Key null or empty");
                throw new ArgumentNullException("Client Key / Guid value is null or empty");
            }
            else
            {
                var query = @"SELECT DeviceId, clientLocationName, clientLocationKey, clientKey, clientId, tpid, facilityId
                              FROM dsa_clientLocations WHERE clientKey = @clientKey";
                try
                {
                    logger.Debug("Search for client location records");
                    return await _db.QueryAsync<ClientLocations>(query, new { clientKey });
                }
                catch (SqlException ex)
                {
                    logger.Debug("Error searching for client location records: {0}", ex);
                    throw;
                }
            }
        }

        public async Task AddClientLocationRecords(IEnumerable<ClientLocations> clientLocations)
        {
            if (clientLocations.Any())
            {
                var query = @"INSERT INTO [dsa_clientLocations](dateAdded,dateChanged,lastUserID, deviceID, clientKey, clientLocationKey, clientLocationName, clientid, tpid, facilityId)
		                      VALUES(GETDATE() ,GETDATE(),'kris.lindsey', @deviceId, @clientKey ,@clientLocationKey, @clientLocationName, @clientId ,@tpid ,@facilityId)";

                var insertIntoClientApps = @"INSERT INTO [dsa_clientApps] ([dateAdded],[dateChanged],[lastUserID],[deviceID],[clientLocKey],[appKey])
                                            VALUES(GETDATE(), GETDATE(),'kris.lindsey',@deviceId, @clientLocationKey,'7B1580D7-DD22-47F5-A369-B7C47B9132EA')";
                var p = new DynamicParameters();
                foreach (var location in clientLocations)
                {
                    if (location.ClientLocationKey.Equals(null))
                    {
                        logger.Debug("client location key is null or empty");
                        throw new ArgumentNullException("locationKey guid value is null or empty");
                    }
                    else
                    {
                        p.Add("@deviceId", location.DeviceId);
                        p.Add("@clientKey", location.ClientKey);
                        p.Add("@clientLocationKey", location.ClientLocationKey);
                        p.Add("@clientLocationName", location.ClientLocationName);
                        p.Add("@clientId", location.ClientId);
                        p.Add("@tpid", location.TpId);
                        p.Add("@facilityId", location.FacilityId);
                        try
                        {
                            logger.Debug("Adding client location record");
                            await _db.ExecuteAsync(query, p);
                            await _db.ExecuteAsync(insertIntoClientApps, p);
                        }
                        catch (Exception ex)
                        {
                            logger.Debug("Error Adding client location records: {0}", ex);
                        }
                    }
                }
            }
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> FindPayerWebsiteMappingValues(Guid clientKey, Guid clientLocationKey)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<PayerWebsiteMappingValue>> FindPayerWebsiteMappingValues(Guid clientKey)
        {
            if (clientKey.Equals(null))
            {
                throw new ArgumentNullException("Client key guid value is null or empty");
            }
            else
            {
                var query = @"SELECT clientKey, clientLocationKey, Primary_PayerKey, WebSiteKey, DefaultValue, DetailLabel FROM dsa_PayerWebsiteMappingValues 
                            WHERE ClientKey = @clientKey ORDER BY DetailLabel";
                var parameters = new DynamicParameters();

                parameters.Add("@clientKey", clientKey.ToString());
                try
                {
                    return await _db.QueryAsync<PayerWebsiteMappingValue>(query, parameters);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error Adding client location records: {0}", ex);
                    throw;
                }
            }
        }
        bool ClientKeyIsValid(string clientKey)
        {
            if (clientKey.Equals(null) || clientKey.ToString().Equals("00000000-0000-0000-0000-000000000000"))
            {
                return false;
            }
            return true;
        }
        public async Task AddPayerWebsiteMappingValues(IEnumerable<PayerWebsiteMappingValue> payerWebsiteMappingValues)
        {
            if (payerWebsiteMappingValues.Any())
            {                                
                foreach (var payerMappingValue in payerWebsiteMappingValues)
                {
                    if (ClientKeyIsValid(payerMappingValue.ClientKey))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@payerKey", payerMappingValue.Primary_PayerKey);
                        parameters.Add("@websiteKey", payerMappingValue.WebsiteKey.ToString());
                        parameters.Add("@defaultValue", payerMappingValue.DefaultValue);
                        parameters.Add("@clientKey", payerMappingValue.ClientKey.ToString());
                        parameters.Add("@clientLocationKey", payerMappingValue.ClientLocationKey.ToString());
                        parameters.Add("@detailLabel", payerMappingValue.DetailLabel);

                        try
                        {
                            var query = @"INSERT INTO dsa_PayerWebsiteMappingValues(Primary_PayerKey, WebSiteKey, DefaultValue, ClientKey, ClientLocationKey, DetailLabel)
			                  VALUES(@payerKey, @websiteKey, @defaultValue, @clientKey, @clientLocationKey, @detailLabel)";

                            await _db.ExecuteAsync(query, parameters);
                        }
                        catch (Exception ex)
                        {
                            logger.Debug("Error Adding payer website mapping vlaue records: {0}", ex);
                            throw;
                        }                        
                    }
                    else
                    {
                        throw new ArgumentNullException("Client Key is null or empty");
                    }
                }
            }
        }

        public async Task<IEnumerable<FacilityMaster>> FindFacilityMasterRecords(Guid clientKey)
        {
            if (clientKey.Equals(null) || clientKey == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                throw new NoNullAllowedException("client key is null or empty");
            }
            else
            {
                var query = @"SELECT RecordKey,FacilityKey, OrderMap, FacilityName, clientKey, clientLocationKey from dsa_facilityMaster 
                          WHERE ClientKey = @clientKey";

                try
                {
                    return await _db.QueryAsync<FacilityMaster>(query, new { clientKey });
                }
                catch (Exception ex)
                {
                    logger.Debug("Error serching facilites: {0}", ex);
                    throw;
                }
            }
        }

        public async Task AddFacilityMasterRecord(IEnumerable<FacilityMaster> facilites)
        {
            if (facilites.Any())
            {
                var query = @"INSERT INTO [dbo].[dsa_facilityMaster]([FacilityKey],[FacilityName],[OrderMap],[ClientKey],[ClientLocationKey],[AutoRunUserID])
			              VALUES(@facilityKey,@facilityName , @orderMap,@clientKey, @clientLocationKey, NULL)";
                var p = new DynamicParameters();

                foreach (var facility in facilites)
                {
                    if (facility.ClientKey.Equals(null))
                    {
                        throw new ArgumentNullException("client key is null or empty string");
                    }
                    else
                    {
                        p.Add("@facilityKey", facility.FacilityKey);
                        p.Add("@facilityName", facility.FacilityName);
                        p.Add("@orderMap", facility.Ordermap);
                        p.Add("@clientKey", facility.ClientKey);
                        p.Add("@clientLocationKey", facility.ClientLocationKey);

                        try
                        {
                            await _db.ExecuteAsync(query, p);
                        }
                        catch (Exception ex)
                        {
                            logger.Debug("Error adding facility records: {0}", ex);
                            throw;
                        }
                    }
                }
            }
        }
        public async Task<IEnumerable<FacilityDetail>> FindFacilityDetailsRecords(IEnumerable<Guid> facilityKeys)
        {
            if (facilityKeys.Any())
            {
                var query = @"SELECT DetailLabel, DetailValue, FacilityKey, RecordKey FROM dsa_facilityDetails WHERE FacilityKey IN @facilityKeys";

                var p = new DynamicParameters();
                p.Add("@facilityKeys", facilityKeys);

                try
                {
                    return await _db.QueryAsync<FacilityDetail>(query, p);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding facility Detials records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("Facility Keys are null or empyty");
            }
        }
        public async Task<IEnumerable<FacilityDetail>> FindFacilityDetailsRecords(Guid facilityKey)
        {
            if (facilityKey.Equals(null))
            {
                throw new ArgumentNullException("Facility Key is null or empty.");
            }
            else
            {
                var query = @"SELECT * from dsa_facilityDetails Where facilityKey = @facilityKey";
                var p = new DynamicParameters();
                p.Add("@facilityKey", facilityKey);
                try
                {
                    return await _db.QueryAsync<FacilityDetail>(query, p);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding facility Detials records: {0}", ex);
                    throw;
                }
            }
            throw new NotImplementedException();
        }

        public async Task AddFacilityDetailsRecords(IEnumerable<FacilityDetail> facilityDetails)
        {
            if (facilityDetails.Any())
            {
                var query = @"INSERT INTO dsa_facilityDetails(DetailLabel, DetailValue, FacilityKey, UpdatedBy, LastUpdate)
			                  VALUES(@detailLabel,@detailValue, @facilityKey, 'kris.lindsey', GETDATE())";
                var p = new DynamicParameters();

                foreach (var facilityDetail in facilityDetails)
                {
                    p.Add("@detailLabel", facilityDetail.DetailLabel);
                    p.Add("@detailValue", facilityDetail.DetailValue);
                    p.Add("@facilityKey", facilityDetail.FacilityKey);

                    try
                    {
                        await _db.ExecuteAsync(query, p);
                    }
                    catch (Exception ex)
                    {
                        logger.Debug("Error adding facility Detials records: {0}", ex);
                        throw;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Facility Detials are null or empty");
            }
        }

        public Task<IEnumerable<CriteriaDetails>> FindCriteriaWithClientKey(Guid clientKey)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CriteriaSets>> FindCriteriaSetsWithCriteriaSetKeys(IEnumerable<Guid> criteriaSetKeys)
        {
            if (criteriaSetKeys.Any().Equals(false))
            {
                throw new ArgumentNullException("Criteria Set Key is null or empty");
            }
            else
            {
                var query = @"SELECT criteriaSetKey, CriteriaSetName, ScriptKey, DeviceId FROM dsa_criteriaSets WHERE criteriaSetKey IN @criteriaSetKeys";
                var p = new DynamicParameters();

                p.Add("@criteriaSetKeys", criteriaSetKeys);

                try
                {
                    return await _db.QueryAsync<CriteriaSets>(query, p);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        public async Task<IEnumerable<CriteriaDetails>> FindWithCriteriaSetKeys(IEnumerable<Guid> criteriaSetKeys)
        {
            if (criteriaSetKeys.Any())
            {
                var query = @"SELECT deviceId, criteriaSetKey, criteriaDetailKey, fieldKey, fieldPosition, compareValue, irecordKey 
                              FROM dsa_criteriaDetails WHERE criteriaSetKey IN @criteriaSetKeys";

                var p = new DynamicParameters();
                p.Add("@criteriaSetKeys", criteriaSetKeys);

                try
                {
                    return await _db.QueryAsync<CriteriaDetails>(query, p);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding criteria Detials records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("criteria set keys are null or empty");
            }
        }

        public Task<IEnumerable<CriteriaDetails>> FindCriteriaDetails(string wildCard)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CriteriaSets>> FindCriteriaSetRecords(string wildCard)
        {
            if (wildCard.Any())
            {
                string term = "%" + wildCard + "%";
                var query = @"SELECT dateAdded,criteriaSetKey, ScriptKey, CriteriaSetName, DeviceId, lastUserId 
                              FROM dsa_criteriaSets WHERE (CriteriaSetName like @term) ORDER BY criteriaSetName";

                try
                {
                    return _db.QueryAsync<CriteriaSets>(query, new { term });
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding criteria set records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("Wild care is null or empty");
            }
        }

        public async Task AddCriterias(IEnumerable<CriteriaSets> criteriaSets)
        {
            if (criteriaSets.Any())
            {
                var query = @"INSERT INTO dsa_criteriaSets(lastUserID, deviceID, criteriaSetKey, criteriaSetName, transactionTypeKey, scriptKey, Priority)
			                  VALUES('kris.lindsey',@deviceId, @criteriaSetKey, @criteriaSetName  ,'ebcd603b-9fd8-e411-96c2-000c29729dff', @scriptKey, 1)";
                var p = new DynamicParameters();

                foreach (var item in criteriaSets)
                {
                    if (item.CriteriaSetKey.Equals(null))
                    {
                        throw new ArgumentNullException("Criteria set key is null or empty");
                    }
                    else
                    {
                        p.Add("@deviceId", item.DeviceId);
                        p.Add("@criteriaSetKey", item.CriteriaSetKey);
                        p.Add("@criteriaSetName", item.CriteriaSetName);
                        p.Add("@scriptKey", item.ScriptKey);

                        try
                        {
                            await _db.ExecuteAsync(query, p);
                        }
                        catch (Exception ex)
                        {
                            logger.Debug("Error adding criteria set records: {0}", ex);
                            throw;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Criteria Set records are null or empty.");
            }
        }

        public async Task AddCriteriaDetails(IEnumerable<CriteriaDetails> criteriaDetials)
        {
            if (criteriaDetials.Any())
            {
                var query = @"INSERT INTO dsa_criteriaDetails(lastUserID, deviceID, criteriaSetKey, criteriaDetailKey, fieldKey, fieldPosition, operator, compareValue, lineBooleanOperator)
                              VALUES('kris.lindsey', @deviceId, @criteriaSetKey, @criteriaDetailKey, @fieldKey, @fieldPosition,'EQ',@compareValue,'AND')";
                var p = new DynamicParameters();

                foreach (var item in criteriaDetials)
                {
                    p.Add("@deviceId", item.DeviceId);
                    p.Add("@criteriaSetKey", item.CriteriaSetKey);
                    p.Add("@criteriaDetailKey", item.CriteriaDetailKey);
                    p.Add("@fieldKey", item.FieldKey);
                    p.Add("@fieldPosition", item.FieldPosition);
                    p.Add("@compareValue", item.CompareValue);

                    try
                    {
                        await _db.ExecuteAsync(query, p);
                    }
                    catch (SqlException ex)
                    {
                        logger.Debug("Error adding criteria details set records: {0}", ex);
                        throw;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("criteria Details are null or empty");
            }
        }

        public async Task<CriteriaSets> FindCriteriaSetRecord(Guid criteriaSetKey)
        {
            if (criteriaSetKey.Equals(null))
            {
                throw new ArgumentNullException("Criteria Set Key is null or empty");
            }
            else
            {
                var query = @"SELECT criteriaSetKey, CriteriaSetName, ScriptKey, DeviceId FROM dsa_criteriaSets WHERE criteriaSetKey = @criteriaSetKey";
                var p = new DynamicParameters();

                p.Add("@criteriaSetKey", criteriaSetKey);

                try
                {
                    var criteriaSet = await _db.QueryAsync<CriteriaSets>(query, p);
                    return criteriaSet.SingleOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<CriteriaSets> AddCriteriaSetRecord(CriteriaSets criteriaSetRecord)
        {
            if (criteriaSetRecord.CriteriaSetKey != null)
            {
                var query = @"INSERT INTO dsa_criteriaSets(lastUserID, deviceID, criteriaSetKey, criteriaSetName, transactionTypeKey, scriptKey, Priority)
			                  VALUES('kris.lindsey',@deviceId, @criteriaSetKey, @criteriaSetName  ,'ebcd603b-9fd8-e411-96c2-000c29729dff', @scriptKey, 1) 
                              SELECT criteriaSetKey, CriteriaSetName, ScriptKey, DeviceId FROM dsa_criteriaSets WHERE criteriaSetKey = @criteriaSetKey";

                var p = new DynamicParameters();

                p.Add("@deviceId", criteriaSetRecord.DeviceId);
                p.Add("@criteriaSetKey", criteriaSetRecord.CriteriaSetKey);
                p.Add("@criteriaSetName", criteriaSetRecord.CriteriaSetName);
                p.Add("@scriptKey", criteriaSetRecord.ScriptKey);

                try
                {
                    var criteriaSets = await _db.QueryAsync<CriteriaSets>(query, p);
                    return criteriaSets.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    logger.Debug("Error adding criteria set records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("Criteria Set records are null or empty.");
            }
        }

        public async Task<IEnumerable<CriteriaDetails>> FindCriteriaDetailRecords(Guid criteriaSetKey)
        {
            if (criteriaSetKey != null)
            {
                var query = @"SELECT deviceId, criteriaSetKey, criteriaDetailKey, fieldKey, fieldPosition, compareValue, irecordKey 
                              FROM dsa_criteriaDetails WHERE criteriaSetKey = @criteriaSetKey";

                var p = new DynamicParameters();
                p.Add("@criteriaSetKey", criteriaSetKey);

                try
                {
                    return await _db.QueryAsync<CriteriaDetails>(query, p);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding criteria Detials records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("UH OH! criteria set key is null or empty!");
            }
        }

        public Task<IEnumerable<CriteriaDetails>> AddCriteriaDetailRecords(IEnumerable<CriteriaDetails> criteriaDetailRecords)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CriteriaDetails>> FindCriteriaDetailRecords(IEnumerable<Guid> criteriaSetKeys)
        {
            if (criteriaSetKeys.Any())
            {
                var query = @"SELECT deviceId, criteriaSetKey, criteriaDetailKey, fieldKey, fieldPosition, compareValue, irecordKey 
                              FROM dsa_criteriaDetails WHERE criteriaSetKey IN @criteriaSetKeys";

                var p = new DynamicParameters();
                p.Add("@criteriaSetKeys", criteriaSetKeys);

                try
                {
                    return await _db.QueryAsync<CriteriaDetails>(query, p);
                }
                catch (Exception ex)
                {
                    logger.Debug("Error finding criteria Detials records: {0}", ex);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("UH OH! criteria set key is null or empty!");
            }
        }

        public Task<IEnumerable<WebsiteExtractionMap>> FindExtractions(Guid websiteKey)
        {
            if (string.IsNullOrWhiteSpace(websiteKey.ToString()))
            {
                throw new ArgumentNullException("Need websiteKey to process");
            }
            var query = @"SELECT * FROM dbo.dsa_websiteExtractionMapping
                          WHERE websiteKey = @websiteKey";
            try
            {
                return _db.QueryAsync<WebsiteExtractionMap>(query, new { websiteKey });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ScriptMaster>> FindScripts(Guid websiteKey)
        {
            if (string.IsNullOrWhiteSpace(websiteKey.ToString()))
            {
                throw new ArgumentNullException("Please provide websitekey to find scriptmaster records");
            }
            var query = @"SELECT * FROM dbo.dsa_scriptMaster
                          WHERE websiteKey = @websiteKey 
                          ORDER By scriptDesc";
            try
            {
                return await _db.QueryAsync<ScriptMaster>(query, new { websiteKey });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddScripts(IEnumerable<ScriptMaster> scripts)
        {
            var scriptCheck = await FindScriptMaster(scripts.FirstOrDefault().WebsiteKey);
            if (scriptCheck.Any())
            {
                throw new Exception("Script Already Exists in production");
            }
            var query = @"INSERT INTO dbo.[dsa_scriptMaster]([dateAdded]
	                               ,[dateChanged],[lastUserID]
                                   ,[deviceID]
                                   , scriptKey
                                   ,[noRetries],[delayBefore]
	                               ,[delayAfter],[timeout]
                                   ,[scriptDesc]
                                   ,scriptCode
                                   ,[websiteKey]
	                               ,[iterative],[setAgentAs]
                                   ,[noIterations],[tableRow],[tableColumn]
                                   ,[Category]
	                               ,[Priority])
		                            VALUES(
                                      GETDATE()
                                    , GETDATE()
                                    ,'kris.lindsey'
                                    , @deviceId
                                    , @scriptKey
                                    , 0
		                            , 0,0,60
                                    , @Desc
                                    , @Code
                                    , @websiteKey
                                    ,0
                                     ,'I.E.7'
                                    ,NULL
		                            ,NULL,NULL
                                    ,@category,1)";
            try
            {
                var p = new DynamicParameters();
                scripts.ToList().ForEach(async script =>
                {
                    p.Add("@Desc", script.ScriptDesc);
                    p.Add("@scriptKey", script.ScriptKey);
                    p.Add("@Code", script.ScriptCode);
                    p.Add("@websiteKey", script.WebsiteKey);
                    p.Add("@category", script.Category);
                    p.Add("@deviceId", script.DeviceId);
                    try
                    {
                        await _db.ExecuteAsync(query, p);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ScriptReturnValue>> FindScriptReturnValues(Guid websiteKey)
        {
            if (string.IsNullOrWhiteSpace(websiteKey.ToString()))
            {
                throw new ArgumentNullException("Need website key to find records");
            }
            var query = @"SELECT deviceID, scriptKey,returnValue,overrideLabel, nextScriptID
                            FROM dbo.dsa_scriptReturnValues 
                            Where scriptKey 
                            In (
	                            SELECT scriptKey
	                            FROM dsa_scriptMaster 
		                              where websitekey = @websiteKey
                                )
                            ORDER BY deviceID";
            try
            {
                return await _db.QueryAsync<ScriptReturnValue>(query, new { websiteKey });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task AddScriptReturnValues(IEnumerable<ScriptReturnValue> returnValues, Guid websiteKey)
        {
            var returnValuesCheck = await FindScriptReturnValues(websiteKey);
            if (returnValuesCheck.Any())
            {
                throw new Exception("Script Already Exists in production");
            }
            var query = @"INSERT INTO dbo.[dsa_scriptReturnValues](
                                [dateAdded]
                               ,[dateChanged]
                               ,[lastUserID]
                               ,[deviceID]
                               ,[fieldKey]
                               ,[scriptKey]
                               ,[returnValue]
                               ,[overrideLabel]
                               ,[valueOperation]
                               ,[nextScriptID]
                               ,[mappingValue])
                         VALUES( 
                                 GETDATE()
                               , GETDATE()
                               , 'kris.lindsey'
                               , @deviceId
                               , NULL
                               , @scriptKey
                               , @returnValue
                               , NULL
                               ,'EQ'
                               , @EQScriptKey
                               , @mappingValue)";

                var p = new DynamicParameters();
                
                    foreach (var record in returnValues)
                    {
                        p.Add("@deviceId", record.DeviceId);
                        p.Add("@scriptKey", record.ScriptKey);
                        p.Add("@returnValue", record.ReturnValue);
                        p.Add("@EQScriptKey", record.NextScriptId);
                        p.Add("@deviceId", record.DeviceId);
                        p.Add("@mappingValue", record.MappingValue);     

                        try
                        {
                            await _db.ExecuteAsync(query, p);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }                             
            }
        
        public async Task<IEnumerable<ScriptCollectionItem>> FindCollectionItems(Guid websiteKey)
        {
            var isKeyValid = CheckForNullOrWhiteSpace(websiteKey);
            if (isKeyValid)
            {
                var query = @"SELECT DeviceId, scriptKey, fieldKey, overrideLabel, defaultValue, collectionMask
                                FROM dbo.dsa_scriptCollectionItems
                                WHERE scriptKey 
                                IN (
	                                SELECT scriptkey FROM dbo.dsa_scriptMaster
	                                  where websitekey = @websiteKey
                                   )
                                order by DeviceId";
                try
                {
                    return await _db.QueryAsync<ScriptCollectionItem>(query, new { websiteKey });
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new ArgumentNullException("Need websitekey to search");
        }

        public async Task<bool> AddScriptCollectionItems(IEnumerable<ScriptCollectionItem> collectionItems)
        {
            if (collectionItems.Any())
            {
                var query = @"INSERT INTO dbo.dsa_scriptCollectionItems
	                            (lastUserID
	                            ,deviceID
	                            ,scriptKey
	                            ,fieldKey
	                            ,overrideLabel
	                            ,defaultValue
	                            ,required
	                            ,collectionMask
	                            ,validationRoutine
	                            ,Can_Go_back)
                            VALUES
	                            ('kris.lindsey'
	                             ,@DeviceId
	                             ,@scriptKey
	                             ,@fieldKey
	                             ,@overrideLabel
	                             ,NULL
	                             ,1
	                             ,NULL
	                             ,NULL
                                 ,1)";
                foreach (var collectionItem in collectionItems)
                {
                    var p = new DynamicParameters();

                    p.Add("@deviceId", collectionItem.DeviceId);
                    p.Add("@scriptKey", collectionItem.ScriptKey);
                    p.Add("@fieldKey", collectionItem.FieldKey);
                    p.Add("@overrideLabel", collectionItem.OverrideLabel);
                    await _db.ExecuteAsync(query, p);
                }
                return true;
            }
            return false;
        }

        public bool CheckForNullOrWhiteSpace<T>(T value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<ClientMaster>> FindClientMasterRecord(string term)
        {
            var query = @"SELECT clientName, ClientKey, HowToDeliver
                          FROM dsa_clientMaster
                          WHERE clientName like (@searchTerm)";
            var searchTerm = "%" + term + "%";
            try
            {
                return await _db.QueryAsync<ClientMaster>(query, new { searchTerm });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClientLocations>> FindClientLocationRecords(string term)
        {
            var query = @"SELECT clientLocationName, ClientKey, clientLocationKey, Tpid, clientId, facilityid
                          FROM dsa_clientLocations
                          WHERE clientLocationName like (@searchTerm)";
            var searchTerm = "%" + term + "%";
            try
            {
                return await _db.QueryAsync<ClientLocations>(query, new { searchTerm });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FacilityMaster>> FindFacilityMasterRecords(string term)
        {
            var query = @"SELECT facilityName, ClientKey, clientLocationKey, facilityKey, OrderMap
                          FROM dsa_facilityMaster
                          WHERE facilityName like (@searchTerm)";
            var searchTerm = "%" + term + "%";
            try
            {
                return await _db.QueryAsync<FacilityMaster>(query, new { searchTerm });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task<Guid> GetFirstScriptKey(Criteria critera)
        {
            var ps = new DynamicParameters();
            ps.Add("@iprKey", critera.IprKey);
            ps.Add("@clientKey", critera.ClientKey.ToString());
            ps.Add("@clientLocationKey", critera.ClientLocationKey.ToString());
            ps.Add("@transactionKey", "2");

            try
            {
                var result = await _db.QueryAsync("usp_SelectScriptKey", ps, commandType: CommandType.StoredProcedure);
                return result.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<int> CreateClientMappings(Guid clientKey)
        {
            var p = new DynamicParameters();
            p.Add("@clientKey", clientKey);
            try
            {
                return await _db.ExecuteAsync("sp_CreateClientMappingValues", p, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<Criteria>> AddCriteriaRecord(Criteria criteria)
        {
            var p = new DynamicParameters();
            p.Add("@scriptKey", criteria.ScriptKey);
            p.Add("@iprkey", criteria.IprKey);
            p.Add("@locKey", criteria.ClientLocationKey);
            p.Add("@deviceId", criteria.DeviceId);
            p.Add("@cKey", criteria.ClientKey);
            p.Add("@criteriaSetName", criteria.CriteriaSetName);
            p.Add("@updatedBy", criteria.UpdatedBy);

            try
            {
                return await _db.QueryAsync<Criteria>("[CreateScriptCriteriaRecord]", p, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        Task ISmartAgentRepository.AddCriteriaRecord(Criteria criteria)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Criteria>> ISmartAgentRepository.AddCriteriaSetRecord(CriteriaSets criteriaSetRecord)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ScriptMaster>> FindScriptMaster(Guid websiteKey)
        {
            var query = @"select scriptKey, websiteKey, scriptDesc, scriptCode, Category, deviceID, noIterations
                          FROM dsa_scriptMaster
                          WHERE websiteKey = @websiteKey";

            var parameters = new DynamicParameters();

            parameters.Add("@websiteKey", websiteKey);
            try
            {
                return await _db.QueryAsync<ScriptMaster>(query, parameters);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public Task<IEnumerable<ScriptCollectionItem>> FindScriptCollectionItems(Guid key)
        {
            throw new NotImplementedException();
        }
    }
}
