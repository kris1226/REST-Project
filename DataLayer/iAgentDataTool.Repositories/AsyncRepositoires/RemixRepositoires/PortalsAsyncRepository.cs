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
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Repositories.RemixRepositories;
using Dapper;


namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class PortalsAsyncRepository : IAsyncRepository<Portals>
    {
        private readonly IDbConnection _db;

        public PortalsAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<Portals>> GetAllAsync()
        {
            var query = @"SELECT Id, url, Description, IsEnabled
                          FROM Portals order by description";
            try
            {
                return Database.QueryAsync<Portals>(query);
            }
            catch (Exception)
            {           
                throw;
            }
        }

        public async Task<IEnumerable<Portals>> FindWithGuidAsync(Guid websiteKey)
        {
            var query = @"SELECT PWM.Portal_Id, PWM.WebsiteKey, P.Description
                        FROM Portals P
                        INNER JOIN PortalWebsiteMap PWM on p.Id = pwm.Portal_Id
                        WHERE PWM.WebsiteKey = @websiteKey";
            try
            {
                return await Database.QueryAsync<Portals>(query, new { websiteKey });
            }
            catch (Exception)
            {                
                throw;
            }

        }

        public Task<IEnumerable<Portals>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Portals>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Portals>> FindByName(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) {
                throw new ArgumentNullException("Please provide search criteria");
            }
            var name = "%" + description + "%";
            var query = @"SELECT Id, url, Description, IsEnabled
                          FROM Portals order by description
                          WHERE description like @description ";
            var parameters = new DynamicParameters();
            parameters.Add("@description", description);
            try
            {
                return await _db.QueryAsync<Portals>(query, parameters);
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public Task<IEnumerable<Portals>> Find(Portals obj)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(Portals entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Portals entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Portals entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<Portals> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public Task<Guid> AddAsync(IEnumerable<Portals> entity)
        {
            throw new NotImplementedException();
        }
    }
}
