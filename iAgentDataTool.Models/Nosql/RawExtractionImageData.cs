using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iAgentDataTool.Models.Nosql
{
    public class RawExtractionImageData
    {
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// Same value as intImageId
        /// </summary>
        public int SequenceNumber { get; set; }
        public Guid TransactionId { get; set; }
        public byte[] PNG { get; set; }
        public int ScriptRunId { get; set; }

        public DateTime DateCreated { get; private set; }
    }
}
