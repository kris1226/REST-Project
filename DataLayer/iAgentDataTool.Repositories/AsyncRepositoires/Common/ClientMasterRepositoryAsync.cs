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

        public async Task<IEnumerable<ClientMaster>> GetAllAsync()
        {
            var query = @"SELECT DISTINCT clientKey, clientName, HowToDeliver 
                          FROM dsa_clientMaster
                          ORDER BY clientName";
            try
            {

                return await _db.QueryAsync<ClientMaster>(query);
  
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
                return await _db.QueryAsync<ClientMaster>(query, new { clientKey });
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
                var task = await _db.QueryAsync<ClientMaster>(query, new { clientName = "%" + obj.ClientName + "%" });
                return task.ToList();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error finding client " + ex);
                var error = new List<ClientMaster>();
                return error;
            }
        }

        public async Task<Guid> AddAsync(ClientMaster client)
        {
            var p = new DynamicParameters();
            p.Add("@clientName", client.ClientName);
            p.Add("@howToDeliver", client.HowToDeliver);

            var c = new DynamicParameters();
            c.Add("@clientKey", client.ClientKey);

            try
            {
                var result = await _db.QueryAsync<ClientMaster>("CreateClient", p, commandType: CommandType.StoredProcedure);
                return result.SingleOrDefault().ClientKey;
            }
            catch (SqlException)
            {
                return new Guid("00000000-0000-0000-0000-000000000000");
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

        public async Task<IEnumerable<ClientMaster>> FindByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            try
            {
                var term = "%" + name + "%";
                var query = @"select clientname, clientkey, howtodeliver
                              FROM dsa_clientMaster where clientName like @term";
                var result = await _db.QueryAsync<ClientMaster>(query, new { term });
                if (result.Any())
                {
                    return result;
                }
                return new List<ClientMaster>();
            }
            catch (Exception)
            {                
                throw;
            }
        }
        public Task AddMultipleToProd(IEnumerable<ClientMaster> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public async Task<Guid> AddAsync(IEnumerable<ClientMaster> entity)
        {
            if (entity.Any())
            {
                var lastCreatedClient = new ClientMaster();
                var p = new DynamicParameters();
                foreach (var item in entity)
                {
                    p.Add("@clientKey", item.ClientKey);
                    p.Add("@deviceId", item.DeviceId);
                    p.Add("@clientName", item.ClientName);
                    p.Add("@howToDeliver", item.HowToDeliver);
                    
                    try
                    {
                        var result = await _db.QueryAsync<ClientMaster>("sp_CreateClientMaster", p, commandType: CommandType.StoredProcedure);
                        lastCreatedClient = result.SingleOrDefault();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return lastCreatedClient.ClientKey;
            }
            return new Guid();
        }
    }
}
