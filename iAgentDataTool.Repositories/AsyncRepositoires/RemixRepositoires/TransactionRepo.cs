using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.Helpers.Interfaces;
using Dapper;



namespace iAgentDataTool.Repositories.AsyncRepositoires.RemixRepositoires
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly IDbConnection _db;

        public TransactionRepo(IDbConnection db)
        {
            this._db = db;
        }
    
        public async Task<IEnumerable<AgentSchedule>> FindInAgentScheduleAsync(Guid transId)
        {
            var query = @"SELECT id, TransactionId, WebsiteKey, PayerKey, DateOfService, ScriptFinished, LastRun, NumTries, NextRun, CreateDateTime
                          FROM AgentSchedule WHERE TransactionId =@transId";

            return await _db.QueryAsync<AgentSchedule>(query, new { transId });
        }

        public void DeleteFromAgentRequestTable(Guid transId)
        {
            var deleteQuery = @"delete from dsa_agentRequestQueue where transactionID = @transId";
            _db.Execute(deleteQuery, new { transId });
        }

        public void UpdateAgentSchedule(Guid transId)
        {
            var updateQuery = @"Update dsa_WorkQueueProcessing set QueueStatus = 0 WHERE TransactionID =@transId";
            _db.Execute(updateQuery, new { transId });
        }

        public async Task<int> ScheduleTransaction(Guid transId)
        {
            var scheduleQuery = @"UPDATE AgentSchedule 
                                  SET lastrun = NULL, NextRun = getdate(), NumTries = 0
                                  WHERE transactionID=@transId";
            return await _db.ExecuteAsync(scheduleQuery, new { transId });
        }

        public async Task<int> UpdateAgentTransResponses(Guid transId)
        {
            var updateQuery = @"update AgentTransactionResponses SET IsProcessed = 0 where TransactionId =@transId";
            return await _db.ExecuteAsync(updateQuery, new { transId });
        }

        public async Task<int> UpdateAgentResponseTableAsync(Guid requestTransId)
        {
            //var stringTransId = requestTransId.ToString();

            var updateQuery = @"UPDATE dsa_agentResponseQueue SET currentState = 3
                                where requestTransactionID =@requestTransId";
            return await _db.ExecuteAsync(updateQuery, new { requestTransId });
        }


        public void UpdateAgentTransResponses(List<Guid> transIds)
        {
            if (!transIds.Equals(null))
            {
                foreach (var transId in transIds)
                {
                    var updateQuery = @"update AgentTransactionResponses SET IsProcessed = 0 where TransactionId =@transId";
                    _db.Execute(updateQuery, new { transId });
                }
            }
        }

        public void UpdateAgentResponseTableAsync(List<Guid> requestTransIds)
        {
            if (!requestTransIds.Equals(null))
            {
                foreach (var requestTransId in requestTransIds)
                {
                    var updateQuery = @"UPDATE dsa_agentResponseQueue SET currentState = 3
                                        WHERE requestTransactionID =@requestTransId";
                    _db.Execute(updateQuery, new { requestTransId });
                }
            }
        }
    }
}
