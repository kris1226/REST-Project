using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using System.Data.SqlClient;

namespace iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent
{
    public class CriteriaDetialsRepository : IAsyncRepository<CriteriaDetails>
    {
        private IDbConnection _db;

        public CriteriaDetialsRepository(IDbConnection db)
        {
            this._db = db; 
        }
        public System.Threading.Tasks.Task<IEnumerable<CriteriaDetails>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CriteriaDetails>> FindWithGuidAsync(Guid criteriaSetKey)
        {
            if (string.IsNullOrEmpty(criteriaSetKey.ToString()))
            {
                return null;
            }
            else
            {
                var query = @"select criteriaSetKey, criteriaDetailKey,fieldKey, fieldPosition, compareValue, deviceID 
                                from dsa_criteriaDetails where criteriaSetKey IN (
                                SELECT criteriaSetKey from dsa_criteriaSets
                                where (criteriaSetKey = @criteriaSetKey))";
                try
                {
                    return await _db.QueryAsync<CriteriaDetails>(query, new { criteriaSetKey });
                }
                catch (SqlException)
                { 
                    throw;
                }
            }
        }

        public System.Threading.Tasks.Task<IEnumerable<CriteriaDetails>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<IEnumerable<CriteriaDetails>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CriteriaDetails>> FindByName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }
            else
            {
                var term = "%" + name + "%";
                var sql = @"select criteriaSetKey, criteriaDetailKey, fieldKey, fieldPosition, compareValue, deviceID 
                            from dsa_criteriaDetails where criteriaSetKey IN (
                            SELECT criteriaSetKey from dsa_criteriaSets
                            where (criteriaSetName like @term))";
                try
                {
                    return await _db.QueryAsync<CriteriaDetails>(sql, new { term });
                }
                catch (Exception)
                {
                    throw;
                }
            }        
        }

        public System.Threading.Tasks.Task<IEnumerable<CriteriaDetails>> Find(CriteriaDetails obj)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> AddAsync(IEnumerable<CriteriaDetails> criteriaDetials)
        {
            var parameters = new DynamicParameters();
            foreach (var item in criteriaDetials)
            {
                parameters.Add("@criteriaSetKey", item.CriteriaSetKey);
                parameters.Add("@clientKey", item.ClientKey);
                parameters.Add("@clientLocationKey", item.ClientLocationKey);
                parameters.Add("@lastUserId", item.LastUserId);
                parameters.Add("@iprKey", item.IprKey);
                parameters.Add("@deviceId", item.DeviceId);

                try
                {
                    var result = await _db.QueryAsync("CreateCriteriaDetailsRecords", parameters, commandType: CommandType.StoredProcedure);
                    return result.SingleOrDefault();
                }
                catch (SqlException)
                {
                    return new Guid("00000000-0000-0000-0000-000000000000");
                }
            }
            return new Guid("00000000-0000-0000-0000-000000000000");
        }

        public System.Threading.Tasks.Task AddMultipleToProd(IEnumerable<CriteriaDetails> entities)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task RemoveAsync(CriteriaDetails entity)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(CriteriaDetails entity)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }
    }
}
