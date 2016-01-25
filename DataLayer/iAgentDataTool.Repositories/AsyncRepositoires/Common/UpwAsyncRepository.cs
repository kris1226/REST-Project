using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.AsyncRepositories.Common
{
    public class UpwAsyncRepository : IAsyncRepository<Upw>, IUpwAsyncRepository
    {
        private readonly IDbConnection _db;

        public UpwAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database { get { return _db; } }

        public Task<IEnumerable<Upw>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Upw>> FindAutoAgentId(string clientKey, string clientLocationKey)
        {
            if (clientKey != null || clientKey != null)
            {
                var query = @"SELECT Username, EntKey, SiteKey, SqLServer, SqlDb, ClientKey, ClientLocationKey
                              FROM dbo.UPW_UPW with (nolock) 
                              WHERE ClientKey = @clientKey and ClientLocationKey = @ClientLocationKey and username like 'AUTO%'";
                try
                {
                    return await Database.QueryAsync<Upw>(query, new { clientKey, clientLocationKey });
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("client key or client location key is null");
            }
        }
        public Task<IEnumerable<Upw>> FindWith2GuidsAsync(Guid clientKey)
        {
            var sql = @"SELECT UserName, clientKey, clientLocationKey FROM UPW_UPW with(nolock) 
                        WHERE clientKey = @clientKey 
                        AND clientLocationKey = @clientLocationKey";
            var parameters = new DynamicParameters();

            parameters.Add("@clientKey", clientKey.ToString());
            try
            {
                return Database.QueryAsync<Upw>(sql, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error getting UPW Records " + ex);
                throw;
            }
        }

        public async Task<IEnumerable<Upw>> Find(Upw upw)
        {
            var clientLocationKey = upw.ClientLocationKey.ToString();
            var clientKey = upw.ClientKey.ToString();

            var sql = @"SELECT UserName, clientKey, clientLocationKey FROM UPW_UPW with(nolock) 
                        WHERE clientKey = @clientKey 
                        AND clientLocationKey = @clientLocationKey AND username like '%AUTO%'";
            var parameters = new DynamicParameters();
            parameters.Add("@clientKey", clientKey);
            parameters.Add("@@clientLocationKey", clientLocationKey);

            try
            {
                return await Database.QueryAsync<Upw>(sql, parameters);
            }
            catch (SqlException ex)
            {
                var error = new List<Upw>();
                Console.WriteLine("Error finding UPW record" + ex);
 
            }
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Upw>> FindByName(string name)
        {
            if (IsValid(name))
            {
                var term = "%" + name + "%";
                var query = @"SELECT Username, EntKey, SiteKey, SqLServer, SqlDb, ClientKey, ClientLocationKey
                          FROM dbo.UPW_UPW with (nolock) 
                          WHERE(SqlDb LIKE @term) ORDER BY Username, SITEKEY";

                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["UPW"].ConnectionString))
                {
                    try
                    {
                        return await db.QueryAsync<Upw>(query, new { term });
                    }
                    catch (Exception)
                    {
                        return null;
                    }                    
                }
            }            
            return null;
        }

        bool IsValid(string value)
        {
            if (value != null || value != "")
            {
                return true;
            }
            return false;
        }

        public Task<IEnumerable<Upw>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(Upw entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Upw entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Upw entity)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Upw>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Upw>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }

        public Task AddMultipleToProd(IEnumerable<Upw> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public Task<Guid> AddAsync(IEnumerable<Upw> entity)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Upw>> FindWithEntKey(string entKey)
        {
            if (IsValid(entKey))
            {
                var sqlParams = new DynamicParameters();
                sqlParams.Add("@entKey", entKey);

                var query = @"SELECT [UserName]
	                      , Password
                          ,[EntKey]
                          ,[SiteKey]
                          ,[SQLServer]
                          ,[SQLDB]
                          ,[SQLUser]
                          ,[SQLPW]
                          ,[ClientKey]
                          ,[ClientLocationKey]
                          ,queueNames
                          ,queueUser
                      FROM [UPW].[dbo].[UPW_UPW]
                      where EntKey = @entKey";
                try
                {
                    return await _db.QueryAsync<Upw>(query, sqlParams);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
