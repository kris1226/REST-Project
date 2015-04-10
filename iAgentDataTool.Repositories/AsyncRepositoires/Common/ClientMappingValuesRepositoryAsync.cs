using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace iAgentDataTool.Repositories.Common
{
    public class ClientMappingValuesRepositoryAsync : IAsyncRepository<ClientMappingValue>
    {
        private readonly IDbConnection _db;

        public ClientMappingValuesRepositoryAsync(IDbConnection db)
        {
           this._db = db;
        }
        public Task<IEnumerable<ClientMappingValue>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientMappingValue>> FindWithGuidAsync(Guid clientKey)
        {
            var query = @"SELECT websitekey, fieldKey, fieldValue, NormalizedValue 
                          FROM dsa_clientMappingValues WHERE clientKey = @clientKey";
            try
            {

                return await _db.QueryAsync<ClientMappingValue>(query, new { clientKey });
            }
            catch (SqlException)
            {
                throw;
            }      
        }

        public Task<IEnumerable<ClientMappingValue>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingValue>> Find(ClientMappingValue obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(ClientMappingValue entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ClientMappingValue entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClientMappingValue entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingValue>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingValue>> FindByName(string name)
        {
            throw new NotImplementedException();
        }



        public Task AddMultipleToProd(IEnumerable<ClientMappingValue> entities)
        {
            throw new NotImplementedException();
        }
    }
}
