using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

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

        public Task<IEnumerable<CriteriaSets>> FindByName(string name)
        {
            string term = "%" + name + "%";
            var query = @"select criteriaSetKey, CriteriaSetName, ScriptKey 
                          FROM dsa_criteriaSets where (CriteriaSetName like @term) order by criteriaSetName";
            if (!name.Equals(null))
            {
                try
                {
                    return _db.QueryAsync<CriteriaSets>(query, new { term });
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<CriteriaSets>> Find(CriteriaSets obj)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(CriteriaSets entity)
        {
            //var criteriaSetName = entity.CriteriaSetName;
            //var scriptKey = entity.ScriptKey;

            var parameters = new DynamicParameters();
            parameters.Add("@criteriaSetName", entity.CriteriaSetName);
            parameters.Add("@scriptKey", entity.ScriptKey);
        
            try
            {
                await _db.ExecuteAsync("CreateCriteriaSet", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                var errorMessage = "Error adding client ";
                Console.WriteLine(errorMessage);
            }
        }

        public Task AddMultipleToProd(IEnumerable<CriteriaSets> entities)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(CriteriaSets entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CriteriaSets entity)
        {
            throw new NotImplementedException();
        }
    }
}
