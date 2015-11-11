using iAgentDataTool.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class ClientLocRepo : IClientLocaionRepository
    {
        private readonly IDbConnection _db;

        public ClientLocRepo(IDbConnection db)
        {
            this._db = db;
        }
        public Task UpdateTpid(string tpid)
        {
            //var sql = @"UPDATE";
            throw new NotImplementedException();
        }

        public Task UpdateClientId(string clientId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFacilityId(string facilityId)
        {
            throw new NotImplementedException();
        }
    }
}
