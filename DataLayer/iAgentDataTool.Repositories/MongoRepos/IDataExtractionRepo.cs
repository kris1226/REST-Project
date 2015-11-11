using iAgentDataTool.Models.Nosql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.MongoRepos
{
    public interface IDataExtractionRepo
    {
        IEnumerable<AuthImage> GetAuthImage(string cvsAuthId);
    }
}
