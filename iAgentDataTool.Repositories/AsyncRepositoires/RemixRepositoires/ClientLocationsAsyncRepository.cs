using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.Repositories.Common
{
    public class ClientLocationsAsyncRepository : IAsyncRepository<ClientLocations>
    {
        private readonly IDbConnection _db;

        public ClientLocationsAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }

        protected IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<ClientLocations>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientLocations>> FindWithGuidAsync(Guid clientKey)
        {
            var query = @"SELECT clientLocationName, tpid, facilityId, clientId, clientLocationKey, id, parent_id, clientKey
                          FROM dsa_clientLocations WHERE clientKey = @clientKey";
            try
            {

                return await Database.QueryAsync<ClientLocations>(query, new { clientKey });  

                   
            }
            catch (Exception)
            {          
                throw;
            }
        }

        public Task<IEnumerable<ClientLocations>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientLocations>> Find(ClientLocations obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(ClientLocations entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ClientLocations entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClientLocations entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ClientLocations>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ClientLocations>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<ClientLocations> entities)
        {
            throw new NotImplementedException();
        }
    }
}
