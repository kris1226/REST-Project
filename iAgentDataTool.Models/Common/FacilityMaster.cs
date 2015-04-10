using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class FacilityMaster
    {
        public string FacilityName { get; set; }
        public string Ordermap { get; set; }
        public Guid FacilityKey { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Facility Name: {0}", this.FacilityName),
                string.Format("Order Map: {0}", this.Ordermap),
                string.Format("Facility Key {0}", this.FacilityKey)
            });
        }
    }
}
