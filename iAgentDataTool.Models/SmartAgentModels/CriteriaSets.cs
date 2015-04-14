using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class CriteriaSets
    {
        public Guid CriteriaSetKey { get; set; }
        public string CriteriaSetName { get; set; }
        public Guid ScriptKey { get; set; }
        public string DeviceId { get; set; }
    }
}
