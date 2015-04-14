using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;

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

        public System.Threading.Tasks.Task<IEnumerable<CriteriaDetails>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
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
                var sql = @"select criteriaSetKey, criteriaDetailKey,fieldKey, fieldPosition, compareValue, deviceID 
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

        public System.Threading.Tasks.Task AddAsync(CriteriaDetails entity)
        {
            throw new NotImplementedException();
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
    }
}
