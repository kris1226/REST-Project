using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.Remix;
using Dapper;

namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class ExtractionMapRepositoryAsync : IDapperAsyncRepository<ExtractionMap>
    {
        private  IDbConnection _db;

        protected IDbConnection Database { get { return _db; } }

        public ExtractionMapRepositoryAsync(IDbConnection db)
        {
            this._db = db;
        }
        public Task<IEnumerable<ExtractionMap>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExtractionMap>> GetById(long payerResponseId)
        {
            var query = @"SELECT [ExtractionMapId],[PayerResponseId],[LocationType],[Location],[DataItem]
                                ,[PageType] ,[NormalizeFunction] ,[Param1] ,[Param2],[Param3],[AttributeName]
                                  FROM [iAgentRemixDatabase].[dbo].[ExtractionMap] 
                                  WHERE PayerResponseId = @payerResponseId
                                  ORDER BY PayerResponseId, ExtractionMapId desc";
            try
            {
               return await Database.QueryAsync<ExtractionMap>(query, new { payerResponseId });
            }
            catch (SqlException)
            {               
                throw;
            }   
        }
    }
}
