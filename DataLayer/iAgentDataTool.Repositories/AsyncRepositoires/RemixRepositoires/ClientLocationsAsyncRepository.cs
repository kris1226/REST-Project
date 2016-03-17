using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.ScriptHelpers;
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

        public async Task<IEnumerable<ClientLocations>> GetAllAsync()
        {
            var query = @"SELECT cl.clientLocationName, cl.clientKey, ca.clientLocKey, cl.clientLocationKey, cl.clientId, cl.tpid, cl.facilityId
                            from dsa_clientLocations cl
                            LEFT JOIN dsa_clientApps ca on ca.clientLocKey = cl.clientLocationKey
                            ORDER by cl.clientLocationName ";
            try
            {
                return await _db.QueryAsync<ClientLocations>(query);
            }
            catch (SqlException)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<ClientLocations>> FindWithGuidAsync(Guid clientKey)
        {
            var query = @"SELECT cl.clientLocationName, cl.clientKey, ca.clientLocKey, cl.clientLocationKey, cl.clientId, cl.tpid, cl.facilityId
                          FROM dsa_clientLocations cl
                          LEFT JOIN dsa_clientApps ca on ca.clientLocKey = cl.clientLocationKey 
                          WHERE clientKey = @clientKey";
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

        public Task<Guid> AddAsync(ClientLocations entity)
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


        public async Task<IEnumerable<ClientLocations>> FindByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            try
            {
                var term = "%" + name + "%";
                var query = @"select clientLocationName, clientkey, clientLocationKey, clientid, facilityId, tpid
                              FROM dsa_clientLocations where clientLocationName like @term";

                var result = await _db.QueryAsync<ClientLocations>(query, new { term });
                if (result.Any())
                {
                    return result;
                }
                return new List<ClientLocations>();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task AddMultipleToProd(IEnumerable<ClientLocations> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public async Task<Guid> AddAsync(IEnumerable<ClientLocations> entity)
        {
            if (entity.Any())
            {
                var p = new DynamicParameters();
                var lastAdded = new ClientLocations(); 
                foreach (var item in entity)
                {
                    p.Add("@clientLocationName", item.ClientLocationName);
                    p.Add("@clientKey", item.ClientKey);
                    p.Add("@clientLocationKey", item.ClientLocationKey);
                    p.Add("@clientId", item.ClientId);
                    p.Add("@tpid", item.TpId);
                    p.Add("@facilityId", item.FacilityId);
                    p.Add("@deviceId", item.DeviceId);
                    p.Add("@lastUserId", item.LastUserId);

                    try
                    {
                        var result = await _db.QueryAsync<ClientLocations>("sp_CreateClientLocation", p, commandType: CommandType.StoredProcedure);
                        lastAdded = result.SingleOrDefault();
                    }
                    catch (Exception)
                    {                        
                        throw;
                    }
                }
                return lastAdded.ClientLocationKey;

            }
            return new Guid();
        }
    }
}
