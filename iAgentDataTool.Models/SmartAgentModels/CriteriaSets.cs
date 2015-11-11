using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class CriteriaSets
    {
        public string DeviceId { get; set; }
        public Guid CriteriaSetKey { get; set; }
        public Guid ScriptKey { get; set; }
        public string CriteriaSetName { get; set; }
        public string LastUserId { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Criteria set key: {0}", this.CriteriaSetKey),
                string.Format("Criteria set name: {0}", this.CriteriaSetName),
                string.Format("First ScriptKey: {0}", this.ScriptKey),
                string.Format("Device Id: {0}", this.DeviceId),
            });
        }
    }   
}
