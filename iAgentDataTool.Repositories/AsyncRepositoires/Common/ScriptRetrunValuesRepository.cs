using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using iAgentDataTool.Models;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;


namespace iAgentDataTool.Repositories
{
    public class ScriptRetrunValuesRepository 
    {
        private readonly IDbConnection _db;

        public ScriptRetrunValuesRepository(IDbConnection db)
        {
            _db = db;
        }
        public IEnumerable<ScriptReturnValue> GetInformation()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScriptReturnValue> Find(Guid websiteKey)
        {
            var query = @"SELECT scriptKey, returnValue, valueOperation, nextScriptID, mappingValue
                          FROM dsa_scriptReturnValues WHERE (scriptKey IN
                         (SELECT scriptKey FROM dsa_scriptMaster
                          WHERE (websiteKey = @websiteKey))) ORDER BY deviceID";
            try
            {
                return _db.Query<ScriptReturnValue>(query, new {websiteKey });
            }
            catch (SqlException ex)
            {
                var error = new List<ScriptReturnValue>();
                Console.WriteLine("Sql Exception thrown " + ex);
                return error;
            }
        }

        public void Add(ScriptReturnValue enity)
        {
            throw new NotImplementedException();
        }

        public ScriptReturnValue Update(ScriptReturnValue entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid key)
        {
            throw new NotImplementedException();
        }

        public ScriptReturnValue GetAll()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ScriptReturnValue> FindWithGuid(Guid Key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScriptReturnValue> FindWithId(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
