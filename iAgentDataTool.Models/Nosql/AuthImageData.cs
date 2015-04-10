using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iAgentDataTool.Models.Nosql
{
    public class AuthImageData : AuthImage
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime DateCreated { get; set; }

        public AuthImageData()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}
