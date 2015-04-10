using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iAgentDataTool.Models.Nosql
{
    public class AgentResponseData
    {
        [BsonId]
        public Object Id { get; set; }
        public Guid TransactionId { get; set; }
        public string RawAgentResponse { get; set; }
        public DateTime DateCreated { get; set; }

        public AgentResponseData()
        {
            
        }
    }
}
