using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using iAgentDataTool.Models;
using iAgentDataTool.Models.SmartAgentModels;
using ScriptDataHelpers;

namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public class ScriptCreationRepo : IScriptCreation
    {
        private readonly IDbConnection _db;
        public ScriptCreationRepo(IDbConnection db)
        {
            this._db = db;
        }
        public async Task<Guid> CreateScritp(Script script)
        {
            if (string.IsNullOrWhiteSpace(script.WebsiteDescription))
            {
                throw new ArgumentNullException("Need a script description to create a new record.");
            }
            var query = @"INSERT INTO [dsa_scriptMaster]([dateAdded]
	                               ,[dateChanged]
                                   ,[lastUserID]
                                   ,[deviceID]
                                   ,[scriptKey]
                                   ,[noRetries],[delayBefore]
	                               ,[delayAfter],[timeout],[scriptDesc],scriptCode,[websiteKey]
	                               ,[iterative],[setAgentAs],[noIterations],[tableRow],[tableColumn],[Category]
	                               ,[Priority])
		                            VALUES(GETDATE(),GETDATE(),'kris.lindsey'
                                    , @deviceId
                                    , @scriptKey
                                    , 0
		                            , 0
                                    , 0
                                    , 60
                                    , @Desc
                                    , @Code
                                    , @websiteKey
                                    , 0
                                     ,'I.E.7'
                                    , NULL
		                            , NULL
                                    , NULL
                                    , @category
                                    , 1)
                          SELECT scriptKey 
                          FROM [dsa_scriptMaster] WHERE scriptDesc = @Desc";

            var p = new DynamicParameters();

            p.Add("@Desc", script.WebsiteDescription);
            p.Add("@Code", script.Code);
            p.Add("@websiteKey", script.WebsiteKey);
            p.Add("@category", script.Category);
            p.Add("@deviceId", script.DeviceId);
            p.Add("@scriptKey", Guid.NewGuid());

            try
            {
               var record = await _db.QueryAsync<Guid>(query, p);
               return record.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task CreateReturnValues(ScriptReturnValue returnValues)
        {
            if (string.IsNullOrWhiteSpace(returnValues.DeviceId))
            {
                throw new ArgumentNullException("Please provide a deviceId before adding a record");
            }
            var query = @"INSERT INTO dbo.[dsa_scriptReturnValues]
                               ([dateAdded]
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
                         VALUES
                               (GETDATE()
                               ,GETDATE()
                               ,@lastUserID
                               ,@deviceId
                               ,NULL
                               ,@scriptKey
                               ,@returnValue
                               ,NULL
                               ,'EQ'
                               ,@EQScriptKey
                               ,@mappingValue)";

            var retrunValue2 = @"INSERT INTO .dbo.[dsa_scriptReturnValues]
                               ([dateAdded]
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
                         VALUES
                               (GETDATE()
                               ,GETDATE()
                               ,@lastUserID
                               ,@deviceId
                               ,NULL
                               ,@scriptKey
                               ,@returnValue
                               ,NULL
                               ,'NE'
                               ,@NEGuid
                               ,@mappingValue)";

            var p = new DynamicParameters();

            p.Add("@lastUserID", "kris.lindsey");
            p.Add("@deviceId", returnValues.DeviceId);
            p.Add("@scriptKey", returnValues.ScriptKey);
            p.Add("@returnValue", returnValues.ReturnValue);
            p.Add("@NEGuid", returnValues.NotEquelScriptKey);
            p.Add("@EQScriptKey", returnValues.EqualScripKey);
            p.Add("@deviceId", returnValues.DeviceId);
            p.Add("@mappingValue", returnValues.MappingValue);

            try
            {
                await _db.ExecuteAsync(query, p);
                await _db.ExecuteAsync(retrunValue2, p);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ScriptCollectionItem> CreateCollectionItems(ScriptCollectionItem collectionItem)
        {
            if (string.IsNullOrWhiteSpace(collectionItem.DeviceId))
            {
                throw new ArgumentNullException("Need a deviceId in order to add collection item.");
            }
            var query = @"INSERT INTO dbo.dsa_scriptCollectionItems
                        (dateAdded,dateChanged,lastUserID, deviceID, scriptKey, fieldKey,overrideLabel, defaultValue, required, 
                        collectionMask, validationRoutine,Can_Go_back)
                        VALUES(GetDate(),GetDate(),'kris.lindsey',@deviceId, @scriptKey,@fieldKey,@overideLabel,NULL,1,NULL,NULL, 1)
                         SELECT scriptKey, fieldKey, deviceId, overrideLabel
                         FROM dbo.dsa_scriptCollectionItems 
                         WHERE scriptkey = @scriptkey and overrideLabel = @overideLabel";

            var parameters = new DynamicParameters();

            parameters.Add("@deviceId", collectionItem.DeviceId);
            parameters.Add("@scriptKey", collectionItem.ScriptKey);
            parameters.Add("@fieldKey", collectionItem.FieldKey);
            parameters.Add("@overideLabel", collectionItem.OverrideLabel);

            try
            {
                var record = await _db.QueryAsync<ScriptCollectionItem>(query, parameters);
                return record.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<WebsiteExtractionMap> CreateExtractionMap(WebsiteExtractionMap extractMap)
        {
            if (extractMap.WebsiteKey == null)
            {
                throw new ArgumentNullException("Need websiteKey in order to create a extraction record");
            }
            var query = @"INSERT INTO [ScriptingAgentDatabase].[dbo].[dsa_websiteExtractionMapping]
                           ([WebsiteKey]
                           ,[DataName]
                           ,[DocumentLocation]
                           ,[LocationType]
                           ,[LocationValue]
                           ,[FormatFunction]
                           ,[ValueFunction]
                           ,[Priority])
                     VALUES
                           (@websiteKey
                           ,@dataName
                           ,@documentLocation
                           ,@locationType
                           ,@locationValue
                           ,@formatFunction
                           ,@valueFunction
                           ,@priority)";

            var p = new DynamicParameters();
            p.Add("@websiteKey", extractMap.WebsiteKey);
            p.Add("@dataName", extractMap.DataName);
            p.Add("@documentLocation", extractMap.DocumentLocation);
            p.Add("@locationType", extractMap.LocationType);
            p.Add("@locationValue", extractMap.LocationValue);
            p.Add("@formatFunction", extractMap.FormatFunction);
            p.Add("@valueFunction", extractMap.ValueFunction);
            p.Add("@priority", extractMap.Priority);
            try
            {
                await _db.ExecuteAsync(query, p);
                return await FindExtractionMap(extractMap);
            }
            catch (SqlException)
            {                
                throw;
            }
        }
        public async Task<WebsiteExtractionMap> FindExtractionMap(WebsiteExtractionMap extractMap)
        {
            if (extractMap.WebsiteKey == null)  
            {
                throw new ArgumentNullException("Need websiteKey in order to create a extraction record");
            }
            var query = @"SELECT WebsiteKey, DataName, DocumentLocation, LocationType, LocationValue, FormatFunction,ValueFunction,Priority
                          FROM [ScriptingAgentDatabase].[dbo].[dsa_websiteExtractionMapping] 
                          WHERE websiteKey = @websiteKey 
                          AND DataName = @dataName 
                          AND Priority = @priority";

            var p = new DynamicParameters();
            p.Add("@websiteKey", extractMap.WebsiteKey);
            p.Add("@dataName", extractMap.DataName);
            p.Add("@priority", extractMap.Priority);

            try
            {
               var result =  await _db.QueryAsync<WebsiteExtractionMap>(query, p);
               return result.SingleOrDefault();
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<ScriptMaster>> GetScripts(Guid websiteKey)
        {
            if(StaticHelpers.CheckForNullOrWhiteSpace(websiteKey)) {
                throw new ArgumentNullException();
            }
            var query = @"SELECT scriptKey, scriptDesc, ScriptCode, websiteKey
                          FROM ScriptingAgentDatabase.dbo.dsa_scriptMaster
                          WHERE websiteKey = @websiteKey
                          ORDER BY scriptDesc";

            var p = new DynamicParameters();
            p.Add("@websiteKey", websiteKey);
            try {
                return await _db.QueryAsync<ScriptMaster>(query, p);
            }
            catch (Exception) {   
                throw;
            }
        }

        public async Task UpdateScriptCode(string scriptCode, Guid scriptKey)
        {
            if (string.IsNullOrWhiteSpace(scriptCode) || scriptKey == null)
            {
                throw new ArgumentNullException("Please provide script code and script key");
            }
            var query = @"UPDATE dsa_scriptMaster SET scriptCode=@scriptCode
                          WHERE scriptKey = @scriptKey";
            var parameters = new DynamicParameters();
            parameters.Add("@scriptCode", scriptKey);
            try
            {
                await _db.ExecuteAsync(query, parameters);
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}
