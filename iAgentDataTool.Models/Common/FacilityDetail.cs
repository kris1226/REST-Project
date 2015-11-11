using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class FacilityDetail
    {
        [Key]
        public int RecordKey { get; set; }
        public string DetailLabel { get; set; }
        public string DetailValue { get; set; }
        public Guid FacilityKey { get; set; }
        public string UpdatedBy { get; set; }
        public IEnumerable<Guid> FacilityKeys { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Record Key: {0}", this.RecordKey),
                string.Format("Detail Label: {0}", this.DetailLabel),
                string.Format("Detail Value: {0}", this.DetailValue),
                string.Format("Facility Key: {0}", this.FacilityKey),
                string.Format("Facility Keys: {0}", this.FacilityKeys),
            });
        }
    }
}
