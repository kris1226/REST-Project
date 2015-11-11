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
        public string WebsiteKey { get; set; }
        public string ClientKey { get; set; }
        public string ClientLocationKey { get; set; }

        private PayerWebsiteMappingValue(Guid clientKey, Guid clientLocationKey)
        {
            this.ClientKey = clientKey.ToString();
            this.ClientLocationKey = clientLocationKey.ToString();
        }
        public static PayerWebsiteMappingValue CreateWebisteMappingValue(Guid clientKey, Guid clientLocationKey)
        {
            return new PayerWebsiteMappingValue(clientKey, clientLocationKey);
        }
        public override string ToString()
        {
            return string.Join(",  ", new string[] {
                string.Format(" {0}", this.Primary_PayerKey),
                string.Format(" {0}", this.DefaultValue),
                string.Format(" {0}", this.DetailLabel),
                string.Format("client key: {0}", this.ClientKey),
                string.Format("client locationKey", this.ClientLocationKey),
                string.Format("website key {0}", this.WebsiteKey)
            });
        }
    }
}
