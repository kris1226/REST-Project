using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;


namespace iAgentDataTool.Repositories.Common
{
    public class FacilityMasterAsyncRepository : IAsyncRepository<FacilityMaster>
    {
        private readonly IDbConnection _db;

        public FacilityMasterAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database { get { return _db; } }

        public async Task<IEnumerable<FacilityMaster>> GetAllAsync()
        {
            var query = @"select FacilityName, OrderMap, FacilityKey, ClientKey, ClientLocationKey
                            from dsa_facilityMaster
                            order by FacilityName";
            try
            {
                return await _db.QueryAsync<FacilityMaster>(query);
            }
            catch (SqlException)
            {              
                throw;
            }
        }

        public Task<IEnumerable<FacilityMaster>> FindWithGuidAsync(Guid clientKey, Guid clientLocationKey = new Guid())
        {       
            throw new NotImplementedException();
        }
        public Task<IEnumerable<FacilityMaster>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<FacilityMaster>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FacilityMaster>> Find(FacilityMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(FacilityMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(FacilityMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FacilityMaster entity)
        {
            throw new NotImplementedException();
        }





        public Task<IEnumerable<FacilityMaster>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<FacilityMaster>> FindWith2GuidsAsync(Guid clientKey, Guid clientLocationKey)
        {
            var query = @"SELECT FacilityKey, OrderMap, FacilityName from dsa_facilityMaster 
                          WHERE ClientKey = @key1 and ClientLocationKey = @key2";
            var key1 = clientKey.ToString();
            var key2 = clientLocationKey.ToString();
       
                try
                {
                    return await _db.QueryAsync<FacilityMaster>(query, new { key1, key2 });
                }
                catch (Exception)
                {
                    throw;
                }
           
        
        }


        Task IAsyncRepository<FacilityMaster>.AddAsync(FacilityMaster entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<FacilityMaster> entities)
        {
            throw new NotImplementedException();
        }
    }
}
