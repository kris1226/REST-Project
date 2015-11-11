using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;


namespace iAgentDataTool.Repositories.RemixRepositories
{
    public class ClientScriptsAsyncRepostiory : IAsyncRepository<ClientScript>
    {
        private readonly IDbConnection _db;

        public ClientScriptsAsyncRepostiory(IDbConnection db)
        {
            this._db = db;
        }

        protected IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<ClientScript>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientScript>> FindWithGuidAsync(Guid clientKey)
        {
            var sql = @" SELECT criteriaSetName, PayerKey, ClientLocationKey, clientKey, InitialScriptId, TransactionType
                         FROM ClientScripts WHERE ClientKey = @clientKey";
            try
            {
                return Database.QueryAsync<ClientScript>(sql, new { clientKey });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Task<IEnumerable<ClientScript>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientScript>> Find(ClientScript obj)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(ClientScript entity)
        {
            //var sql = @"";
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ClientScript entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ClientScript entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ClientScript>> FindWith2GuidsAsync(Guid clientKey, Guid clientLocationKey)
        {
            var sql = @" SELECT criteriaSetName, PayerKey, ClientLocationKey, clientKey, InitialScriptId, TransactionType
                         FROM ClientScripts WHERE ClientKey = @clientKey 
                         AND ClientLocationKey = @clientLocationKey";
            try
            {
                return Database.QueryAsync<ClientScript>(sql, new { clientKey, clientLocationKey });
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Task<IEnumerable<ClientScript>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task AddMultipleToProd(IEnumerable<ClientScript> clientScripts)
        {
            if (!clientScripts.Equals(null))
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["AgentProdDb"].ConnectionString);
                var sql = @"IF NOT EXISTS (SELECT clientkey FROM ClientScripts)
                            BEGIN
                                INSERT INTO ClientScripts
                                (criteriaSetName,[ClientKey],ClientLocationKey,PayerKey,TransactionType, InitialScriptId)
                                VALUES(@criteriaSetName, @clientKey, @clientLocationKey ,@payerKey,3, @InitialScriptId)
                            END";
    
                foreach (var clientScript in clientScripts)
                {
                    var p = new DynamicParameters();
                    p.Add("@criteriaSetName", clientScript.CriteriaSetName);
                    p.Add("@clientKey", clientScript.ClientKey);
                    p.Add("@clientLocationKey", clientScript.ClientLocationKey);
                    p.Add("@payerKey", clientScript.PayerKey);
                    p.Add("@InitialScriptId", clientScript.InitialScriptId);

                    await db.ExecuteAsync(sql, p);
                }
            }           
        }

        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public Task<Guid> AddAsync(IEnumerable<ClientScript> entity)
        {
            throw new NotImplementedException();
        }
    }
}
