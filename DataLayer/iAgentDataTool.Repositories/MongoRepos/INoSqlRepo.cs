using iAgentDataTool.Models.Nosql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.MongoRepos
{
    public interface INoSqlRepo
    {
        AgentResponseData GetAgentResponse(Guid transactionId);
        AuthImageData GetAuthImage(int extractionAuthId);
        IList<RawExtractionImageData> GetExtractionImage(Guid transactionId, int scriptRunId);
        AuthImageData GetAuthImage(string externalMongoId);
        IList<AuthImageData> GetAuthImages(IList<string> externalMongoIds);
    }
}
