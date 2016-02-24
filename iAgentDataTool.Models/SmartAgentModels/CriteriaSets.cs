using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class CriteriaSets
    {
        readonly DateTime dateAdded;
        public DateTime DateAdded { get { return dateAdded; } }

        readonly string deviceId;
        public string DeviceId { get { return deviceId; }  }

        readonly Guid criteriaSetKey;
        public Guid CriteriaSetKey { get { return criteriaSetKey; } }

        readonly Guid scriptKey;
        public Guid ScriptKey { get { return scriptKey; } }

        readonly string criteriaSetName;
        public string CriteriaSetName { get { return criteriaSetName; } }

        readonly string lastUserId;
        public string LastUserId { get { return lastUserId; } }

        public CriteriaSets(DateTime dateAdded,
                            Guid criteriaSetKey,
                            Guid scriptKey,
                            string criteriaSetName = "not found",
                            string deviceId = "not found",
                            string lastUserId = "not found"
                            ) 
        {
            this.dateAdded = dateAdded;
            this.criteriaSetKey = criteriaSetKey;
            this.scriptKey = scriptKey;
            this.criteriaSetName = criteriaSetName;
            this.deviceId = deviceId;
            this.lastUserId = lastUserId;
        }
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
