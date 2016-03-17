using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using iAgentDataTool.Models.Common;
using iAgentDataTool.ScriptHelpers.Interfaces;
using Dapper;



namespace iAgentDataTool.Repositories
{
    public class ScriptMasterRepository : IAsyncRepository<ScriptMaster>
    {
        private readonly IDbConnection _db;

        public ScriptMasterRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ScriptMaster>> GetAllAsync()
        {
            var query = @"SELECT scriptKey, ScriptDesc, ScriptCode, websiteKey, noIterations, Category 
                          FROM dsa_scriptMaster WHERE websiteKey = @websiteKey ORDER BY scriptDesc";
            try
            {
                return await _db.QueryAsync<ScriptMaster>(query);
            }
            catch (SqlException)
            {               
                throw;
            }
        }

        public async Task<IEnumerable<ScriptMaster>> FindWithGuidAsync(Guid websiteKey)
        {
            var query = @"SELECT scriptKey, ScriptDesc, ScriptCode, noIterations, Category 
                          FROM dsa_scriptMaster WHERE websiteKey = @websiteKey ORDER BY scriptDesc";
            try
            {
                return await _db.QueryAsync<ScriptMaster>(query, new { websiteKey });
            }
            catch (SqlException)
            {                
                throw;
            }
        }
        public async Task<DataTable> GetData(Guid websiteKey)
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AgentDevDb"].ConnectionString);
            var query = @"SELECT scriptKey, ScriptDesc, ScriptCode, noIterations, Category 
                          FROM dsa_scriptMaster WHERE websiteKey = @websiteKey ORDER BY ScriptDesc";
            var table = new DataTable();
            var param = new SqlParameter();
            param.ParameterName = "@websiteKey";
            param.Value = websiteKey;

            if (websiteKey != null)
            {
                try
                {
                    using (conn)
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add(param);
                            var dataAdapter = new SqlDataAdapter(query, conn);
                            dataAdapter.SelectCommand = cmd;
                            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            var scripts = await Task.Run(() => dataAdapter.Fill(table));
                        }
                    }
                    return table;
                }
                catch (SqlException)
                {
                    throw;
                }
            }
            return table;
        }
        public Task<IEnumerable<ScriptMaster>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScriptMaster>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScriptMaster>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScriptMaster>> Find(ScriptMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(ScriptMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ScriptMaster entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ScriptMaster entity)
        {
            var sql = @"UPDATE dsa_scriptMaster
                        SET scriptCode = @scriptCode
                        WHERE scriptKey in( @scriptKey)
                        AND websiteKey = @websiteKey";
            var parameters = new DynamicParameters();

            parameters.Add("@scriptCode", entity.ScriptCode);
            parameters.Add("@scriptKey", entity.ScriptKey);
            parameters.Add("@websiteKey", entity.WebsiteKey);

            try
            {
                await _db.ExecuteAsync(sql, parameters);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public void GetAllData()
        {

        }
        public async Task<Script> GetScript(Guid scriptKey) {
            if (scriptKey == null) {
                throw new ArgumentNullException("no script key passed in");
            }
            
            var query = @"select scriptDesc, scriptCode, deviceId, category, websitekey 
                          from dsa_scriptMaster where scriptkey = @scriptKey";
            var parameters = new DynamicParameters();
            parameters.Add("@scriptKey", scriptKey);

            try {
               var script = await _db.QueryAsync<Script>(query, parameters);
               return script.SingleOrDefault();
            }
            catch (Exception) {                
                throw;
            }
        }
        public Task AddMultipleToProd(IEnumerable<ScriptMaster> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public Task<Guid> AddAsync(IEnumerable<ScriptMaster> entity)
        {
            throw new NotImplementedException();
        }
    }
}
