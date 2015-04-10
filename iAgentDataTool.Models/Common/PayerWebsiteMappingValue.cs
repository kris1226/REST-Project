using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class PayerWebsiteMappingValue
    {
        public string Primary_PayerKey { get; set; }
        public string DefaultValue { get; set; }
        public string DetailLabel { get; set; }
        public Guid ClientKey { get; set; }
        public Guid ClientLocationKey { get; set; }

        public override string ToString()
        {
            return string.Join(",  ", new string[] {
                string.Format(" {0}", this.Primary_PayerKey),
                string.Format(" {0}", this.DefaultValue),
                string.Format(" {0}", this.DetailLabel)
            });
        }
    }
}
