using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class CriteriaDetails
    {
        [Key]
        public int IRecordKey { get; set; }
        public Guid CriteriaDetailKey { get; set; }
        public Guid CriteriaSetKey { get; set; }
        public Guid FieldKey { get; set; }
        public string IprKey { get; set; }
        public string ClientKey { get; set; }
        public string ClientLocationKey { get; set; }
        public string LastUserId { get; set; }
        public int FieldPosition { get; set; }
        public string CompareValue { get; set; }
        public string DeviceId { get; set; }
        public int KeyCount { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("CriteriaDetailKey: {0}", this.CriteriaDetailKey),
                string.Format("CriteriaSetKey: {0}", this.CriteriaSetKey),
                string.Format("FieldKey: {0}", this.FieldKey),
                string.Format("FieldPostion: {0}", this.FieldPosition),
                string.Format("CompareValue: {0}", this.CompareValue),
                string.Format("DeviceId: {0}", this.DeviceId),
            });
        }
    }
}
