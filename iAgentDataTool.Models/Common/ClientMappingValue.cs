using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ClientMappingValue
    {
        public Guid WebsiteKey { get; set; }
        public Guid FieldKey { get; set; }
        public string FieldValue { get; set; }
        public string NormalizedValue { get; set; }

        public override string ToString()
        {
            return string.Join("  |  ", new string[] {
                string.Format("{0}", this.WebsiteKey),
                string.Format(" {0}", this.FieldKey),
                string.Format(" {0}", this.FieldValue),
                string.Format(" {0}", this.NormalizedValue)
            });
        }
    }
}
