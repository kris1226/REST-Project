using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using iAgentDataTool.Models.Nosql;
using System.Data;
using System.Configuration;
using MongoDB.Driver.Builders;

namespace iAgentDataTool.Repositories.MongoRepos
{
    public class MongoRepo : INoSqlRepo
    {
        private MongoClient _mongoClient;
        private MongoServer _server;
        private MongoDatabase _database;
        private string _connectionString = @"mongodb://HWUser:89akjdj#1a@hapromdb02:27017/iAgent?replicaSet=HAMDB01ReplicaSet";

       // MongoClient _myDB = new MongoClient(new MongoUrl(ConfigurationManager.ConnectionStrings["MongoBlobStorage"].ConnectionString));

        public MongoRepo()
        {
            _mongoClient = new MongoClient(_connectionString);
            _server = null;
            _database = _server.GetDatabase("iAgent");
   
        }
        public AgentResponseData GetAgentResponse(System.Guid transactionId)
        {
            throw new System.NotImplementedException();
        }

        public AuthImageData GetAuthImage(int extractionAuthId)
        {
   
            throw new System.NotImplementedException();
        }

        public IList<RawExtractionImageData> GetExtractionImage(System.Guid transactionId, int scriptRunId)
        {
            throw new System.NotImplementedException();
        }

        public AuthImageData GetAuthImage(string externalMongoId)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(externalMongoId, out objectId))
            {
                throw new ArgumentException("Invalid Mongo identifier");
            }
            else
            {
                var collection = _database.GetCollection<AuthImageData>("ExtractionAuthImages");
                var authImage = collection.AsQueryable<AuthImageData>().FirstOrDefault(i => i.Id == objectId);

                var lib = new PHC.Utilities.ELib();
                authImage.PNGdata = lib.D(authImage.PNGdata);
                return authImage;
            }
            throw new System.NotImplementedException();
        }

        public IList<AuthImageData> GetAuthImages(IList<string> externalMongoIds)
        {

            throw new System.NotImplementedException();
        }
    }
}
