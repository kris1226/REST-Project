using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class WebsiteMaster
    {
        public Guid WebsiteKey { get; set; }
        public string WebsiteDomain { get; set; }
        public string WebsiteDescription { get; set; }
        public string Portal_Id { get; set; }

        public override string ToString()
        {
            return string.Join(", " , new string[] { 
                string.Format("{0}", this.WebsiteDescription).Trim(),
                string.Format(" {0}", this.WebsiteDomain),
                string.Format("Website Key: {0}", this.WebsiteKey)
             });
        }
    }
}
