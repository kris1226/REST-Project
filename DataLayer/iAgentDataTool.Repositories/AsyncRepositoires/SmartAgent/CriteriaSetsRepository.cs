using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace iAgentDataTool.Repositories.AsyncRepositoires.SmartAgent
{
    public class CriteriaSetsRepository : IAsyncRepository<CriteriaSets>
    {
        IDbConnection _db;

        public CriteriaSetsRepository(IDbConnection db)
        {
            this._db = db;
        }
        public Task<IEnumerable<CriteriaSets>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CriteriaSets>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CriteriaSets>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CriteriaSets>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CriteriaSets>> FindByName(string name) {
            string term = "%" + name + "%";
            var query = @"SELECT dateAdded, criteriaSetKey, ScriptKey, CriteriaSetName, DeviceId, lastUserID
                          FROM dsa_criteriaSets 
                          WHERE (CriteriaSetName like @term) order by criteriaSetName";

            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException("Please provide a search term");
            }
            try {
                return _db.QueryAsync<CriteriaSets>(query, new { term });
            }
            catch (Exception) {
                Task<IEnumerable<CriteriaSets>> error = null; ;
                return error;
            }
        }

        public Task<IEnumerable<CriteriaSets>> Find(CriteriaSets obj)
        {
            throw new NotImplementedException();
        }

        public static bool AnyPropsNull<T>(T entity)
        {
            return entity.GetType().GetProperties()
                                   .Where(p => p.GetValue(entity) is string)
                                   .Any(p => string.IsNullOrWhiteSpace((p.GetValue(entity) as string)));
        }
        public async Task<Guid> AddAsync(CriteriaSets entity)
        {
            var arePropsNull = AnyPropsNull(entity);
            if (arePropsNull)
            {
                throw new ArgumentNullException();
            }
            
            var parameters = new DynamicParameters();

            parameters.Add("@criteriaSetName", entity.CriteriaSetName);
            parameters.Add("@scriptKey", entity.ScriptKey);
            parameters.Add("@deviceId", entity.DeviceId);
            parameters.Add("@updatedBy", entity.LastUserId);

            try
            {
                var result = await _db.QueryAsync("CreateCriteriaSet", parameters, commandType: CommandType.StoredProcedure);
                return result.SingleOrDefault();
            }
            catch (SqlException)
            {
                return new Guid("00000000-0000-0000-0000-000000000000");
            }
        }

        public async Task AddMultipleToProd(IEnumerable<CriteriaSets> cs)
        {
            if (cs.Any())
	        {
//                string query = @"INSERT INTO dsa_criteriaSets(lastUserID, deviceID, criteriaSetKey, criteriaSetName, transactionTypeKey, scriptKey, Priority)
//			                 VALUES('kris.lindsey',@deviceId, @criteriaSetKey, @criteriaSetName,'ebcd603b-9fd8-e411-96c2-000c29729dff', @scriptKey, 1)";
                foreach (CriteriaSets c in cs)
	            {
		            DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@deviceId", c.DeviceId);
                    parameters.Add("@criteriaSetKey", c.CriteriaSetKey);
                    parameters.Add("@criteriaSetName", c.CriteriaSetName);
                    parameters.Add("@scriptKey", c.ScriptKey);
                    try
                    {
                        await _db.ExecuteAsync("CreateCriteriaSetFromAnotherSource", parameters, commandType: CommandType.StoredProcedure);
                    }
                    catch (SqlException)
                    {                       
                        throw;
                    }           
	            }               
	        }
            else
	        {
                throw new NoNullAllowedException("Please provide a list of criteria set records");
	        }       
        }

        public Task RemoveAsync(CriteriaSets entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CriteriaSets entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }
        private bool CheckForNullOrWhiteSpace<T>(T value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }
            return false;
        }


        public Task<Guid> AddAsync(IEnumerable<CriteriaSets> entity)
        {
            throw new NotImplementedException();
        }
    }
}
