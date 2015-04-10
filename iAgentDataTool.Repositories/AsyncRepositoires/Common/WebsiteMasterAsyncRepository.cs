using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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
        public IDbConnection Database { get { return _db; } }

		public Task<IEnumerable<WebsiteMaster>> GetAllAsync()
		{
			var query = @"SELECT wm.websiteKey, wm.websiteDescription, wm.websiteDomain, PWM.Portal_Id  
						FROM dsa_websiteMaster wm
                        INNER JOIN PortalWebsiteMap PWM on PWM.WebsiteKey = wm.websiteKey
						ORDER BY wm.websiteDescription";
			try
			{
                return Database.QueryAsync<WebsiteMaster>(query);
			}
			catch (Exception)
			{
				
				throw;
			}

			throw new NotImplementedException();
		}


		public Task<IEnumerable<WebsiteMaster>> FindWithIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WebsiteMaster>> Find(WebsiteMaster obj)
		{
			throw new NotImplementedException();
		}

		public Task AddAsync(WebsiteMaster entity)
		{
			throw new NotImplementedException();
		}

		public Task RemoveAsync(WebsiteMaster entity)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(WebsiteMaster entity)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WebsiteMaster>> FindWithGuidAsync(Guid key)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WebsiteMaster>> FindWith2GuidsAsync(Guid key, Guid secondKey = new Guid())
		{
			throw new NotImplementedException();
		}

        public Task<IEnumerable<WebsiteMaster>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<WebsiteMaster> entities)
        {
            throw new NotImplementedException();
        }
    }
}
