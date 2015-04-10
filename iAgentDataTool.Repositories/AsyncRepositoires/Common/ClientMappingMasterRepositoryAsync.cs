using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;

namespace iAgentDataTool.Repositories.Common
{
    public class ClientMappingMasterRepositoryAsync : IAsyncRepository<ClientMappingMaster>
    {
        private readonly IDbConnection _db;

        public ClientMappingMasterRepositoryAsync(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database 
        { 
            get{return _db;}
        }
        public Task<IEnumerable<ClientMappingMaster>> GetAllAsync()
        {

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientMappingMaster>> FindWithGuidAsync(Guid clientKey)
        {
            var sql = @"SELECT fieldKey FROM dsa_clientMappingMaster where clientKey = @clientKey";

            try
            {
                return await Database.QueryAsync<ClientMappingMaster>(sql, new { clientKey });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingMaster>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingMaster>> Find(ClientMappingMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(ClientMappingMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ClientMappingMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClientMappingMaster entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ClientMappingMaster>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMappingMaster>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<ClientMappingMaster> entities)
        {
            throw new NotImplementedException();
        }
    }
}
