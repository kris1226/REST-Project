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
    public class PayerWebsiteMappingValuesAsyncRepository : IAsyncRepository<PayerWebsiteMappingValue>
    {
        private readonly IDbConnection _db;

        public PayerWebsiteMappingValuesAsyncRepository(IDbConnection db) 
        {
            this._db = db;
        }

        protected IDbConnection Database { get { return _db;} }

        public Task<IEnumerable<PayerWebsiteMappingValue>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> FindWith2GuidsAsync(Guid clientKey, Guid clientLocationKey)
        {
            if (clientKey != null && clientLocationKey != null)
            {
                var query = @"SELECT Primary_PayerKey, WebSiteKey, DefaultValue, DetailLabel
                            FROM dsa_PayerWebsiteMappingValues 
                            WHERE ClientKey = @clientKey AND ClientLocationKey =@clientLocationKey ORDER BY DetailLabel";
                try
                {
                    return Database.QueryAsync<PayerWebsiteMappingValue>(query, new { clientKey, clientLocationKey });
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("client key or location key is null");
            }
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayerWebsiteMappingValue>> Find(PayerWebsiteMappingValue obj)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(PayerWebsiteMappingValue payerWebsiteMappingValuesRecord)
        {
            var p = new DynamicParameters();
            p.Add("@clientKey", payerWebsiteMappingValuesRecord.ClientKey);
            p.Add("@clientLocationKey", payerWebsiteMappingValuesRecord.ClientLocationKey);

            try
            {
                var task = await _db.ExecuteScalarAsync("CreatePayerWebsiteMappingValuesRecords", p, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Task RemoveAsync(PayerWebsiteMappingValue entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PayerWebsiteMappingValue entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<PayerWebsiteMappingValue> entities)
        {
            throw new NotImplementedException();
        }
    }
}
