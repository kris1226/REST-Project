using iAgentDataTool.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class PortalUserRepo : IPortalUserRepo
    {
        private readonly IDbConnection _db;

        public PortalUserRepo(IDbConnection db)
        {
            this._db = db;
        }
        public Task UpdateUsername(string username, int id)
        {
            var query = @"UPDATE PortalUsers SET username = @username WHERE id = @id";
            try
            {
                return _db.ExecuteAsync(query, new { username, id });
            }
            catch (SqlException)
            {                
               throw;
            }
        }

        public async Task UpdatePassword(string password, int id)
        {
            if (!password.Equals(null) || !id.Equals(null))
            {
                var query = @"UPDATE PortalUsers SET password = @password WHERE id = @id";
                await _db.ExecuteAsync(query, new { password, id });                
            }   
        }

        public async Task UpdatePortalId(int Portal_Id, int Id)
        {

            if (!Portal_Id.Equals(null) || !Id.Equals(null))
            {
                var query = @"UPDATE PortalUsers 
                              SET Portal_Id = @Portal_Id 
                              WHERE Id = @Id";
                var parameters = new DynamicParameters();

                parameters.Add("@Portal_Id", Portal_Id);
                parameters.Add("@Id", Id);

                await _db.ExecuteAsync(query, parameters);
            }
            else
            {
                throw new ArgumentNullException("null values found try again");
            }
        }
    }
}
