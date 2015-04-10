using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class UserLogin
    {
        public DateTime DateAdded { get; set; }
        public DateTime DateChanged { get; set; }
        public string LastUserID { get; set; }
        public string UserID { get; set; }
        public string DeviceID { get; set; }
        public string WebsiteDescription { get; set; }
        public Guid ClientKey { get; set; }
        public Guid ClientLocationKey { get; set; }
        public Guid WebsiteKey { get; set; }
        public string AgentLogin { get; set; }
        public string WebsiteUserName { get; set; }
        public string WebsitePassword { get; set; }
        public string log { get; set; }


        public override string ToString()
        {
            return string.Join(",", new string[] {
                string.Format("{0}",  this.DateAdded),
                string.Format(" {0}", this.DateChanged),
                string.Format(" {0}", this.LastUserID),
                string.Format(" {0}", this.UserID),
                string.Format(" {0}", this.DeviceID),
                string.Format("ClientKey: {0}", this.ClientKey),
                string.Format("ClientLocationKey: {0}", this.ClientLocationKey),
                string.Format("WebsiteKey: {0}", this.WebsiteKey),
                string.Format("Password {0}", this.WebsitePassword),
                string.Format("Username {0}", this.WebsiteUserName),
                string.Format("{0} ", this.WebsiteDescription)
            });
        }
    }

}
