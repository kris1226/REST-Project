using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class Criteria
    {
        public string DeviceId { get; set; }
        public Guid ClientKey { get; set; }
        public Guid CriteriDetailKey { get; set; }
        public Guid CriteriaSetKey { get; set; }
        public Guid ClientLocationKey { get; set; }
        public Guid FieldKey { get; set; }
        public Guid ScriptKey { get; set; }
        public string CriteriaSetName { get; set; }
        public string UpdatedBy { get; set; }
        public string IprKey { get; set; }
        public string CompareValue { get; set; }
        public Criteria(){}
        public Criteria(string criteriaSetName, Guid criteriaSetKey, Guid scriptKey, string iprkey, Guid clientKey, Guid clientLocationKey, string updatedBy, string deviceId)
        {
            this.CriteriaSetName = criteriaSetName;
            this.CriteriaSetKey = criteriaSetKey;
            this.ScriptKey = scriptKey;
            this.IprKey = iprkey.Trim();
            this.ClientKey = clientKey;
            this.ClientLocationKey = clientLocationKey;
            this.UpdatedBy = updatedBy;
            this.DeviceId = deviceId;
        }

        public static Criteria CreateCriteria(string criteriaSetName, Guid criteriaSetKey, Guid scriptKey, string iprkey, Guid clientKey, Guid clientLocationKey, string updatedBy, string deviceId)
        {
            return new Criteria(criteriaSetName, criteriaSetKey,  scriptKey, iprkey, clientKey, clientLocationKey, updatedBy, deviceId);
        }
        public static Criteria CreateCriteria(Criteria criteria)
        {
            return new Criteria()
            {
                CriteriaSetName = criteria.CriteriaSetName,
                CriteriaSetKey = Guid.NewGuid(),
                ScriptKey = criteria.ScriptKey,
                IprKey = criteria.IprKey,
                ClientKey = criteria.ClientKey,
                ClientLocationKey = criteria.ClientLocationKey,
                UpdatedBy = criteria.UpdatedBy,
                DeviceId = criteria.DeviceId
            };
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
