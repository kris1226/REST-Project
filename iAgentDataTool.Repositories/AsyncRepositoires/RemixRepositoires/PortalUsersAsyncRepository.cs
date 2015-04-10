using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.Helpers.Interfaces;

namespace iAgentDataTool.Repositories.RemixRepositories
{
    public class PortalUsersAsyncRepository : IAsyncRepository<PortalUser>
    {
        private readonly IDbConnection _db;

        public PortalUsersAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }

        protected IDbConnection Database
        {
            get { return _db; }
        }
        public async Task<IEnumerable<PortalUser>> GetAllAsync()
        {
            var query = @"SELECT DISTINCT PULM.PortalUser_Id, PU.Portal_Id, Username, Password
                            FROM PortalUsers PU
                            INNER JOIN PortalUserLocationMap PULM on pulm.PortalUser_Id = PU.Id
                            ORDER BY PULM.PortalUser_Id desc";
            try
            {
                return await Database.QueryAsync<PortalUser>(query);
            }
            catch (SqlException ex)
            {               
                throw ex;
            }
        }

        public Task<IEnumerable<PortalUser>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PortalUser>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PortalUser>> FindWithIdAsync(int locationId)
        {
            if (locationId != 0)
            {
                var query = @"SELECT PU.Portal_Id, PULM.PortalUser_Id, Username, Password
                              FROM PortalUsers PU
                              INNER JOIN PortalUserLocationMap PULM ON pulm.PortalUser_Id = PU.Id
                              WHERE PULM.Location_Id = @locationId";
                try
                {
                    return await Database.QueryAsync<PortalUser>(query, new { locationId });
                }
                catch (Exception)
                {
                    throw;
                }        
            }
            else
            {
                throw new ArgumentNullException("Location is null");
            }         
        }

        public async Task<IEnumerable<PortalUser>> Find(PortalUser portalUser)
        {
            var query = @"SELECT TOP 1 Id, Portal_Id, Username, Password, IsExpired, DateExpired, IsEnabled, AllowsConcurrent
                         FROM PortalUsers WHERE id = @Id";
            if (portalUser != null)
            {
                try
                {
                    return await Database.QueryAsync<PortalUser>(query, new { Id = portalUser.PortalUser_Id });
                }
                catch (SqlException ex)
                {                   
                    throw ex;
                }
            }
            throw new NotImplementedException();
        }

        public async Task AddAsync(PortalUser portalUser)
        {
            if (portalUser != null)
            {
                var parameters = new DynamicParameters();
                var sql = @"INSERT INTO PortalUsers(Portal_Id, Username, Password, IsExpired, IsEnabled, AllowsConcurrent)
                        VALUES (@Portal_Id, @Username, @Password, @IsExpired, @IsEnabled, @AllowsConcurrent)";
//                var query = @"INSERT INTO PortalUserLocationMap(Location_Id, PortalUser_Id)
//                          VALUES(@Location_Id, @PortalUser_Id)";
//                var query2 = "SELECT TOP 1 Id From PortalUsers ORDER BY Id DESC";

                parameters.Add("@Portal_Id", portalUser.Portal_Id);
                parameters.Add("@Username", portalUser.Username);
                parameters.Add("@Password", portalUser.Password);
                parameters.Add("@IsExpired", portalUser.IsExpired);
                parameters.Add("@IsEnabled", portalUser.IsEnabled);
                parameters.Add("@Allows", portalUser.AllowsConcurrent);

                try
                {
                    await Database.ExecuteAsync(sql, new
                    {
                        portalUser.Portal_Id,
                        portalUser.Username,
                        portalUser.Password,
                        portalUser.IsExpired,
                        portalUser.IsEnabled,
                        portalUser.AllowsConcurrent
                    });
                    //var newPortalId = await _db.QueryAsync<int>(query2);
                    //var PortalUser_Id = newPortalId.SingleOrDefault();
                    //await Database.ExecuteAsync(query, new { PortalUser_Id, portalUser.Location_Id });
                }
                catch (SqlException)
                {
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("Portal User is empty");
            }          
        }

        public Task RemoveAsync(PortalUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PortalUser portalUser)
        {
            var query = @"UPDATE PortalUsers
                            SET Password = @password, Username = @username
                            WHERE Id = @id and Portal_Id = @portalId";

            var parameters = new DynamicParameters();
            parameters.Add("@username", portalUser.Username);
            parameters.Add("@password", portalUser.Password);
            parameters.Add("@Id", portalUser.PortalUser_Id);
            parameters.Add("@portalId", portalUser.Portal_Id);

            if (portalUser != null)
            {
                try
                {
                    await Database.ExecuteAsync(query, parameters);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Portal User is null");
            }
        }

        public Task<IEnumerable<PortalUser>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<PortalUser> entities)
        {
            throw new NotImplementedException();
        }
    }
}
