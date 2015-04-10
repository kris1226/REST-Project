using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models.Common;



namespace iAgentDataTool.Repositories.AsyncRepositoires
{
    public class UserLoginsRepositoryAsync : IAsyncRepository<UserLogin>, IAddMultipleEntitesToDbAsync<UserLogin>
    {
        private readonly IDbConnection _db;

        public UserLoginsRepositoryAsync(IDbConnection db)
        {
            this._db = db;
        }
        public IDbConnection Database { get { return _db; }  }

        public async Task AddAll(IEnumerable<UserLogin> logins)
        {
            foreach (var login in logins)
	        {
                var user = new UserLogin();
                var parameters = new DynamicParameters();
                var sql = @"INSERT INTO dsa_userLogins (clientKey, clientLocationKey, websiteKey, websiteUsername, websitePassword, userID)
	                    VALUES (@clientKey, @clientLocationKey, @websiteKey, @websiteUserName,@websitePassword, @userId)";

                parameters.Add("@clientKey", login.ClientKey);
                parameters.Add("@clientLocationKey", login.ClientLocationKey);
                parameters.Add("@websiteKey", login.WebsiteKey);
                parameters.Add("@websiteUserName", login.WebsiteUserName);
                parameters.Add("@websitePassword", login.WebsitePassword);
                parameters.Add("@userId", login.UserID);

                try
                {
                    await Database.ExecuteAsync(sql, new { login.ClientKey, login.ClientLocationKey, login.WebsiteKey, login.WebsiteUserName, login.WebsitePassword, UserId = login.UserID });
                }
                catch (SqlException ex)
                {

                    var errorMessage = "Error adding client " + ex;
                    Console.WriteLine(errorMessage);
                    login.log = errorMessage.ToString();
                }
	        }        
        }
        
        public async Task<IEnumerable<UserLogin>> GetAllAsync()
        {
            var error = new List<UserLogin>();
            var query = @"SELECT clientName FROM dsa_clientMaster ORDER BY clientName";
            try
            {
                return await Database.QueryAsync<UserLogin>(query);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database Exception " + ex);
                return error;
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserLogin>> FindWithGuidsAsync(Guid clientLocationKey)
        {
            var query = @"SELECT userId, websiteUsername, websitePassword, clientKey, clientLocationKey, UL.websiteKey, WM.websiteDescription 
                          FROM dsa_userLogins UL
                          INNER JOIN dsa_websiteMaster WM on WM.websiteKey = UL.websiteKey 
                          WHERE clientLocationKey = @clientLocationKey";
            try
            {
                return await Database.QueryAsync<UserLogin>(query, new { clientLocationKey });
            }
            catch (SqlException ex)
            {
                var errors = new List<UserLogin>();
                var error = new UserLogin()
                {
                    log = ex.ToString()
                };
                errors.Add(error);
                Console.WriteLine("Error finding userLogins " + ex);
                return errors;
            }
        }



        public async Task AddAsync(UserLogin login)
        {
            var user = new UserLogin();
            var parameters = new DynamicParameters();
//            var sql = @"INSERT INTO dsa_userLogins (clientKey, clientLocationKey, websiteKey, websiteUsername, websitePassword, userID)
//	                    VALUES (@clientKey, @clientLocationKey, @websiteKey, @websiteUserName,@websitePassword, @userId)";

            parameters.Add("@clientKey", login.ClientKey);
            parameters.Add("@clientLocationKey", login.ClientLocationKey);
           // parameters.Add("@websiteKey", login.WebsiteKey);
            parameters.Add("@websiteUserName", login.WebsiteUserName);
            parameters.Add("@websitePassword", login.WebsitePassword);
            parameters.Add("@autoAgentId", login.UserID);

            try
            {
                await Database.ExecuteAsync("CreateUserLoginReocrds", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                
                var errorMessage = "Error adding client " + ex;
                Console.WriteLine(errorMessage);
                login.log = errorMessage.ToString();
            }
        }
        public Task<IEnumerable<UserLogin>> FindWithIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task RemoveAsync(UserLogin entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserLogin entity)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<UserLogin>> Find(UserLogin userlogin)
        {
            var query = @"SELECT websiteUsername, websitePassword
                          FROM dsa_userLogins 
                          WHERE clientKey = @clientKey AND clientLocationKey = @clientLocationKey 
                          AND websiteKey= @websiteKey";
            try
            {
                return await Database.QueryAsync<UserLogin>(query, new { userlogin.ClientKey, userlogin.ClientLocationKey, userlogin.WebsiteKey });
            }
            catch (SqlException ex)
            {                
                throw ex;
            }
            throw new NotImplementedException();
        }


        public Task<IEnumerable<UserLogin>> FindWithGuidAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserLogin>> FindWith2GuidsAsync(Guid clientKey, Guid clientLocationKey )
        {
            var query = @"SELECT userId, websiteUsername, websitePassword, clientKey, clientLocationKey, UL.websiteKey, UL.lastUserId, UL.deviceId, WM.websiteDescription 
                          FROM dsa_userLogins UL
                          INNER JOIN dsa_websiteMaster WM on WM.websiteKey = UL.websiteKey 
                          WHERE clientKey = @clientKey and clientLocationKey =@clientLocationKey";
            try
            {
                return await Database.QueryAsync<UserLogin>(query, new { clientKey, clientLocationKey });
            }
            catch (SqlException ex)
            {
                var errors = new List<UserLogin>();
                var error = new UserLogin()
                {
                    log = ex.ToString()
                };
                errors.Add(error);
                Console.WriteLine("Error finding userLogins " + ex);
                return errors;
            }
        }

        public Task<IEnumerable<UserLogin>> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task AddMultipleToProd(IEnumerable<UserLogin> entities)
        {
            throw new NotImplementedException();
        }
    }
}
