using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ScriptDataHelpers;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Repositories.RemixRepositories;
using Dapper;

namespace iAgentDataTool.AsyncRepositories.Common
{
    public class WebsiteMasterAsyncRepository : IAsyncRepository<WebsiteMaster>
    {
        private readonly IDbConnection _db;

        public WebsiteMasterAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }

        public Task<IEnumerable<WebsiteMaster>> GetAllAsync()
        {
            var query = @"SELECT websiteKey, websiteDescription, websiteDomain, DeviceId  
						FROM dsa_websiteMaster
						ORDER BY websiteDescription";
            try
            {
                return _db.QueryAsync<WebsiteMaster>(query);
            }
            catch (SqlException)
            {
                throw;
            }
        }


        public Task<IEnumerable<WebsiteMaster>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WebsiteMaster>> Find(WebsiteMaster obj)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> AddAsync(WebsiteMaster website)
        {
            if (String.IsNullOrWhiteSpace(website.WebsiteDescription))
            {
                throw new ArgumentNullException("Need website description to process.");
            }
            var webSiteExsits = await FindByName(website.WebsiteDescription);
            if (webSiteExsits.Any())
            {
                return new Guid("00000000-0000-0000-0000-000000000000");
            }
            var query = @"IF NOT EXISTS 
                          (SELECT websiteDescription 
                           FROM dsa_websiteMaster 
                           WHERE websiteDescription = @websiteDescription)
                           BEGIN
                            INSERT INTO [ScriptingAgentDatabase].dbo.dsa_websiteMaster(
                            dateAdded,
                            dateChanged,
                            lastUserID, 
                            deviceID, 
                            websiteKey,
                            websiteDomain,
                            websiteDescription,  
                            DontValidateHCPCS)
	                        VALUES (GetDate(),
                                    GetDate(),
                                    'kris.lindsey',
                                    @deviceId,
                                    @websiteKey,
                                    @websiteDomain,
                                    @websiteDescription                                    
                                     0)
                          END
                          SELECT websiteKey FROM [ScriptingAgentDatabase].dbo.dsa_websiteMaster WHERE websiteDescription = @websiteDescription";

            var p = new DynamicParameters();

            p.Add("@websiteKey", website.WebsiteKey);
            p.Add("@deviceId", website.DeviceId);
            p.Add("@websiteDescription", website.WebsiteDescription);
            p.Add("@websiteDomain", website.WebsiteDomain);

            try
            {
                var websiteKey = await _db.QueryAsync<Guid>(query, p);
                return websiteKey.SingleOrDefault();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public async Task<Guid> AddAsync(IEnumerable<WebsiteMaster> websites)
        {
            Guid lastInsertedWebsiteKey = new Guid();
            foreach (var website in websites)
            {
                if (String.IsNullOrWhiteSpace(website.WebsiteDescription))
                {
                    throw new ArgumentNullException("Need website description to process.");
                }
                var webSiteExsits = await FindByName(website.WebsiteDescription);

                if (webSiteExsits.Any())
                {
                    return new Guid("00000000-0000-0000-0000-000000000000");
                }
                var query = @"IF NOT EXISTS 
                          (SELECT websiteDescription 
                           FROM dsa_websiteMaster 
                           WHERE websiteDescription = @websiteDescription)
                           BEGIN
                            INSERT INTO [ScriptingAgentDatabase].dbo.dsa_websiteMaster(
                            dateAdded,
                            dateChanged,
                            lastUserID, 
                            deviceID, 
                            websiteKey,
                            websiteDomain,
                            websiteDescription,  
                            DontValidateHCPCS)
	                        VALUES (GetDate(),
                                    GetDate(),
                                    'kris.lindsey',
                                    @deviceId,
                                    @websiteKey,
                                    @websiteDomain,
                                    @websiteDescription                                    
                                     0)
                          END
                          SELECT websiteKey FROM [ScriptingAgentDatabase].dbo.dsa_websiteMaster WHERE websiteDescription = @websiteDescription";

                var p = new DynamicParameters();

                p.Add("@websiteKey", website.WebsiteKey);
                p.Add("@deviceId", website.DeviceId);
                p.Add("@websiteDescription", website.WebsiteDescription);
                p.Add("@websiteDomain", website.WebsiteDomain);

                try
                {
                    var websiteKey = await _db.QueryAsync<Guid>(query, p);
                    lastInsertedWebsiteKey = websiteKey.FirstOrDefault();
                }
                catch (SqlException)
                {
                    throw;
                }
                return lastInsertedWebsiteKey;
            }
            return new Guid("00000000-0000-0000-0000-000000000000");
        }

        public Task RemoveAsync(WebsiteMaster entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(WebsiteMaster entity)
        {
            var sql = @"Update dsa_websiteMaster SET websiteDomain = @newUrl, dateChanged = GETDATE()
                        where websitekey = @websitekey";
            if (entity != null)
            {
                var p = new DynamicParameters();
                p.Add("@newUrl", entity.WebsiteDomain);
                p.Add("@websiteKey", entity.WebsiteKey);

                try
                {
                    await _db.ExecuteAsync(sql, p);
                }
                catch (Exception)
                {                    
                    throw;
                }
            }
        }

        public Task<IEnumerable<WebsiteMaster>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WebsiteMaster>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WebsiteMaster>> FindByName(string websiteName)
        {
            if (websiteName.CheckForNullOrWhiteSpace() == false)
            {
                var name = "%" + websiteName +"%";
                var query = @"SELECT websiteKey, websiteDescription, websiteDomain, DeviceId  
						FROM dsa_websiteMaster 
                        WHERE (websiteDescription LIKE @name)
						ORDER BY websiteDescription";
                var p = new DynamicParameters();
                p.Add("@name", name);
                try
                {
                    return await _db.QueryAsync<WebsiteMaster>(query, p);
                }
                catch (SqlException)
                {
                    throw;
                }
            }
            return null;
        }


        public Task AddMultipleToProd(IEnumerable<WebsiteMaster> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }



    }
}
