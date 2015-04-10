using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iAgentDataTool.Models.Nosql
{
    public class AuthImage
    {
        public int ExtractionAuthId { get; set; }
        public byte[] PNGdata { get; set; }
        public string ExternalMongoId { get; set; }
    }
}
