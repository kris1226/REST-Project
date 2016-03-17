using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;
using iAgentDataTool.ScriptHelpers.Interfaces;
using Dapper;

namespace iAgentDataTool.Repositories.Common
{
    public class FacilityDetialsAsyncRepository  : IAsyncRepository<FacilityDetail>
    {
        private readonly IDbConnection _db;

        public FacilityDetialsAsyncRepository(IDbConnection db)
        {
            this._db = db;
        }
        protected IDbConnection Database { get { return _db;} }

        public Task<IEnumerable<FacilityDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FacilityDetail>> FindWithGuidAsync(Guid facilityKey)
        {
            var query = @"SELECT DetailLabel, DetailValue, FacilityKey FROM dsa_facilityDetails WHERE FacilityKey = @facilityKey";
            try
            {
                return _db.QueryAsync<FacilityDetail>(query, new { facilityKey });
            }
            catch (Exception)
            {           
                throw;
            }
            throw new NotImplementedException();
        }

        public Task<FacilityDetail> FindWith2GuidsAsync2(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FacilityDetail>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FacilityDetail>> Find(FacilityDetail obj)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(FacilityDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(FacilityDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FacilityDetail entity)
        {
            //var sql = @"update dsa_facilityDetials";
            return null;
        }


        public Task<IEnumerable<FacilityDetail>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<FacilityDetail>> FindWith2GuidsAsync(Guid key, Guid secondKey)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<FacilityDetail> entities)
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey)
        {
            throw new NotImplementedException();
        }


        public async Task<Guid> AddAsync(IEnumerable<FacilityDetail> entity)
        {
            if (entity.Any())
            {
                var p = new DynamicParameters();
                Guid lastCreatedKey = new Guid();
                foreach (var item in entity)
                {
                    p.Add("@facilityKey", item.FacilityKey);
                    p.Add("@UpdatedBy", item.UpdatedBy);

                    try
                    {
                        var result = await _db.QueryAsync<Guid>("sp_CreateFacilityDetails", p, commandType: CommandType.StoredProcedure);
                        lastCreatedKey = result.FirstOrDefault();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return lastCreatedKey;

            }
            return new Guid();
        }
    }
}
