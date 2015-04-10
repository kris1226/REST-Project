using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Models;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Repositories.RemixRepositories;
using Dapper;


namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class PortalsAsyncRepository : IAsyncRepository<Portal>
    {
        private readonly IDbConnection _db;

        public PortalsAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<Portal>> GetAllAsync()
        {
            var query = @"SELECT Id, Description, IsEnabled
                          FROM Portals";
            try
            {
                return Database.QueryAsync<Portal>(query);
            }
            catch (Exception)
            {           
                throw;
            }
        }

        public async Task<IEnumerable<Portal>> FindWithGuidAsync(Guid websiteKey)
        {
            var query = @"SELECT PWM.Portal_Id, PWM.WebsiteKey, P.Description
                        FROM Portals P
                        INNER JOIN PortalWebsiteMap PWM on p.Id = pwm.Portal_Id
                        WHERE PWM.WebsiteKey = @websiteKey";
            try
            {
                return await Database.QueryAsync<Portal>(query, new { websiteKey });
            }
            catch (Exception)
            {                
                throw;
            }

        }

        public Task<IEnumerable<Portal>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Portal>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Portal>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Portal>> Find(Portal obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Portal entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Portal entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Portal entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<Portal> entities)
        {
            throw new NotImplementedException();
        }
    }
}
