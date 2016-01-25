using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using iAgentDataTool.Models.Remix;
using Dapper;

namespace iAgentDataTool.Repositories.RemixRepositories
{
    public class RemixRepository : IRemixRepository
    {
        private IDbConnection _db;
 
        public RemixRepository(IDbConnection db)
        {
            this._db = db;
        }
        public async Task<AgentConfiguration> ConfigureAgent(AgentConfiguration agent)
        {
            if (PropsAreNull(agent))
            {
                throw new ArgumentNullException();
            }
            var query = @"IF NOT EXISTS (
	                        SELECT AgentId, Parent_Id
	                        FROM AgentConfiguration
	                        WHERE AgentId = @agentId
	                        AND Parent_Id = @parentId
                        )
                        BEGIN
                        INSERT INTO AgentConfiguration(AgentId, Parent_Id)
                        VALUES(@agentId, @parentId)
                        END";

            var p = new DynamicParameters();
            p.Add("@agentId", agent.AgentId);
            p.Add("@parentId", agent.ParentId);

            try
            {
                var result = await _db.QueryAsync(query, p);
                return result.SingleOrDefault();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public static bool PropsAreNull<T>(T entity)
        {
            return entity.GetType()
                         .GetProperties()
                         .Where(p => p.GetValue(entity) is string)
                         .Any(p => string.IsNullOrWhiteSpace((p.GetValue(entity) as string)));
        }
    }
}
