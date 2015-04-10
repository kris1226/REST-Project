using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models.Common;
using Dapper;

namespace iAgentDataTool.Repositories.Common
{
    public class ApcoWebDevAsyncRepoisitory : IAsyncRepository<ApcoWebDev>
    {
        private readonly IDbConnection _db;

        public ApcoWebDevAsyncRepoisitory(IDbConnection db)
        {
            this._db = db;
        }

        public IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<ApcoWebDev>> GetAllAsync()
        {
           
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApcoWebDev>> FindByName(string name)
        {
            // var encoder = name.Replace("%", "[%]").Replace("[", "[]]").Replace("]", "[]]");
            string term = "%" + name + "%";
            var query = @"SELECT TPID, ClientID, FacilityID, entKey, siteKey, KOPSiteName, IsRemixClient
                          FROM businessUnitsClientMapping
                          WHERE (KOPSiteName LIKE @term) ORDER BY KOPSiteName";
            try
            {
                return await Database.QueryAsync<ApcoWebDev>(query, new { term });
            }
            catch (Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ApcoWebDev>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<ApcoWebDev> FindWith2GuidsAsync2(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApcoWebDev>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApcoWebDev>> Find(ApcoWebDev obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(ApcoWebDev entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ApcoWebDev entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApcoWebDev entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ApcoWebDev>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<ApcoWebDev> entities)
        {
            throw new NotImplementedException();
        }
    }
}
