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
using iAgentDataTool.Models.Remix;
using Dapper;


namespace iAgentDataTool.Repositories.RemixRepositories
{
    public class AgentConfigurationAsyncRepository : IAsyncRepository<AgentConfiguration>
    {
        private readonly IDbConnection _db;

        public AgentConfigurationAsyncRepository()
        {
            this._db = new SqlConnection(ConfigurationManager.ConnectionStrings["AgentProdDb"].ConnectionString); ;
        }

        public IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<AgentConfiguration>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AgentConfiguration>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<AgentConfiguration> FindWith2GuidsAsync2(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AgentConfiguration>> FindWithIdAsync(int id)
        {
            var query = @"select agentid, Parent_Id, A.AgentMachineName, A.isEnabled
	                      FROM AgentConfiguration AC
	                      INNER JOIN Agents A on a.Id = ac.AgentId
	                      where AC.Parent_Id = @id
	                      order by A.AgentMachineName";
            try
            {
                return Database.QueryAsync<AgentConfiguration>(query, new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<AgentConfiguration>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AgentConfiguration>> Find(AgentConfiguration obj)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(AgentConfiguration entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(AgentConfiguration entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AgentConfiguration entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<AgentConfiguration>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<AgentConfiguration> entities)
        {
            throw new NotImplementedException();
        }
    }
}
