using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Remix;
using Dapper;

namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class PayerResponseRepositoryAsync : IPayerResponseRepository, IDapperAsyncRepository<PayerResponseMap>
    {
        private readonly IDbConnection _db;

        protected IDbConnection Database { get { return _db; } }

        public PayerResponseRepositoryAsync()
        {
            this._db = new SqlConnection(ConfigurationManager.ConnectionStrings["AgentProdDb"].ConnectionString);
        }

        public async Task<IEnumerable<PayerResponseMap>> GetAllAsync()
        {
            var query = @"SELECT PayerResponseId,[IPRKey],[LookupText],[ResponseType], HtmlResponseType
                          FROM [PayerResponseMap] ORDER BY PayerResponseId desc";
            try
            {
                return await Database.QueryAsync<PayerResponseMap>(query);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PayerResponseMap>> GetByIPRKeyAsync(string iprkey)
        {
            var query = @"SELECT [PayerResponseId],[IPRKey],[LookupText],[ResponseType], HtmlResponseType
                          FROM [PayerResponseMap] WHERE IPRKey = @iprkey ORDER BY ResponseType";
            try
            {
                return await Database.QueryAsync<PayerResponseMap>(query, new { iprkey });
            }
            catch (SqlException)
            {
                throw;
            }  
        }

        public async Task<IEnumerable<PayerResponseMap>> GetById(long payerResponseId)
        {
            var query = @"SELECT [PayerResponseId],[IPRKey],[LookupText],[ResponseType], HtmlResponseType
                          FROM PayerResponseMap WHERE PayerResponseId = @payerResponseId";
            try
            {
                return await Database.QueryAsync<PayerResponseMap>(query, new { payerResponseId });
            }
            catch (SqlException)
            {              
                throw;
            }
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<PayerResponseMap>> GetAllResponseMapIPRKeys()
        {
            var query = @"SELECT DISTINCT IPRKey FROM PayerResponseMap ORDER BY IPRKey";
            try
            {
                return await Database.QueryAsync<PayerResponseMap>(query);
            }
            catch (SqlException)
            {                
                throw;
            }
        }
    }
}
