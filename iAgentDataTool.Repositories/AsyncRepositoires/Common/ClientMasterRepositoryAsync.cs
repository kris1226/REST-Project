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
    public class ClientMasterRepositoryAsync : IAsyncRepository<ClientMaster>
    {
        private readonly IDbConnection _db; 

        public ClientMasterRepositoryAsync(IDbConnection db)
        {
            this._db = db;
        }

        public IDbConnection Database { get { return _db; } set { } }

        public async Task<IEnumerable<ClientMaster>> GetAllAsync()
        {
            var query = @"SELECT DISTINCT clientKey, clientName, HowToDeliver 
                          FROM dsa_clientMaster
                          ORDER BY clientName";
            try
            {

                return await Database.QueryAsync<ClientMaster>(query);
  
            }
            catch (SqlException ex)
            {
                var error = new List<ClientMaster>();
                Console.WriteLine("Database Exception " + ex);
                return error;
            } 
        }

        public async Task<IEnumerable<ClientMaster>> FindWithGuidAsync(Guid clientKey)
        {
            var query = @"Select clientName, clientKey, HowToDeliver From dsa_clientMaster where clientKey = @clientKey";
            try
            {
                return await Database.QueryAsync<ClientMaster>(query, new { clientKey });
            }
            catch (SqlException)
            {              
                throw;
            }
        }

        public Task<IEnumerable<ClientMaster>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ClientMaster>> Find(ClientMaster obj)
        {
            var query = @"SELECT clientName, clientKey, HowToDeliver FROM dsa_clientMaster WHERE clientName LIKE @clientName";
            try
            {
                var task = await Database.QueryAsync<ClientMaster>(query, new { clientName = "%" + obj.ClientName + "%" });
                return task.ToList();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error finding client " + ex);
                var error = new List<ClientMaster>();
                return error;
            }
        }
        public async Task AddAsync(ClientMaster client)
        {
            var p = new DynamicParameters();
            p.Add("@clientName", client.ClientName);
            p.Add("@howToDeliver", client.HowToDeliver);

            var c = new DynamicParameters();
            c.Add("@clientKey", client.ClientKey);

            try
            {
                var task = await _db.ExecuteScalarAsync<ClientMaster>("CreateClient", p, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error adding client " + ex);
            }
        }

        public Task RemoveAsync(ClientMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClientMaster entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ClientMaster>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientMaster>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        Task IAsyncRepository<ClientMaster>.AddAsync(ClientMaster entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<ClientMaster> entities)
        {
            throw new NotImplementedException();
        }
    }
}
