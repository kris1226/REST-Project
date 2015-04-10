using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Remix.ViewModels;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires.ViewModelRepos
{
    public class PortalUserDataRepo : IAsyncRepository<PortalUserAndLocationId>
    {
        IDbConnection _db;

        public PortalUserDataRepo(IDbConnection db)
        {
            this._db = db;
        }
        public Task<IEnumerable<PortalUserAndLocationId>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PortalUserAndLocationId>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PortalUserAndLocationId>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PortalUserAndLocationId>> FindWithIdAsync(int locationId)
        {
            var query = @"SELECT  pulm.Location_Id, pulm.PortalUser_Id, PU.Portal_Id, PU.Username, 
                            PU.Password, pu.DateExpired, p.IsEnabled, PU.IsExpired, p.IsEnabled, PU.AllowsConcurrent, p.Description
                            FROM PortalUsers pu
                            INNER JOIN Portals p on p.id = PU.Portal_Id
                            INNER JOIN PortalUserLocationMap pulm on pulm.PortalUser_Id = pu.Id
                            where pulm.Location_Id =@locationId";
            try
            {
                return await _db.QueryAsync<PortalUserAndLocationId>(query, new { locationId });
            }
            catch (SqlException)
            {             
                throw;
            }
        }
        public async Task AddAsync(PortalUserAndLocationId portalUser)
        {
            if (portalUser != null)
            {
                var parameters = new DynamicParameters();
                var insertPortalUserQuery = @"INSERT INTO PortalUsers(Portal_Id, Username, Password, IsExpired, IsEnabled, AllowsConcurrent)
                        VALUES (@Portal_Id, @Username, @Password, @IsExpired, @IsEnabled, @AllowsConcurrent)";

                var portalUserIdQuery = "SELECT TOP 1 Id From PortalUsers ORDER BY Id DESC";

                var insertPortalUserLocationsQuery = @"INSERT INTO PortalUserLocationMap(Location_Id, PortalUser_Id)
                                          VALUES(@Location_Id, @PortalUser_Id)";

                parameters.Add("@Portal_Id", portalUser.Portal_Id);
                parameters.Add("@Username", portalUser.Username);
                parameters.Add("@Password", portalUser.Password);
                parameters.Add("@IsExpired", portalUser.IsExpired);
                parameters.Add("@IsEnabled", portalUser.IsEnabled);
                parameters.Add("@Allows", portalUser.AllowsConcurrent);

                try
                {
                    await _db.ExecuteAsync(insertPortalUserQuery, new
                    {
                        portalUser.Portal_Id,
                        portalUser.Username,
                        portalUser.Password,
                        portalUser.IsExpired,
                        portalUser.IsEnabled,
                        portalUser.AllowsConcurrent
                    });

                    var newPortalId = await _db.QueryAsync<int>(portalUserIdQuery);
                    var PortalUser_Id = newPortalId.SingleOrDefault();

                    await _db.ExecuteAsync(insertPortalUserLocationsQuery, new
                    {
                        PortalUser_Id,
                        portalUser.Location_Id
                    });
                }
                catch (SqlException)
                {
                    throw;
                }
            }
        }
        public Task<IEnumerable<PortalUserAndLocationId>> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PortalUserAndLocationId>> Find(PortalUserAndLocationId obj)
        {
            throw new NotImplementedException();
        }

        

        public Task RemoveAsync(PortalUserAndLocationId entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PortalUserAndLocationId entity)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<PortalUserAndLocationId> entities)
        {
            throw new NotImplementedException();
        }
    }
}
