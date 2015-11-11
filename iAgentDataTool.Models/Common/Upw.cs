using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class Upw
    {
        public string UserName { get; set; }
        public string SqlDb { get; set; }
        public string SqlServer { get; set; }
        public string ClientKey { get; set; }
        public string ClientLocationKey { get; set; }
        public string EntKey { get; set; }
        public string SiteKey { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Username: {0} ", this.UserName),
                string.Format("SqlDb: {0}", this.SqlDb),
                string.Format("SqlServer: {0}", this.SqlServer),
                string.Format("Client Key: {0}", this.ClientKey),
                string.Format("Client Location Key: {0}", this.ClientLocationKey),
                string.Format("Ent Key: {0}", this.EntKey),
                string.Format("Site Key: {0}", this.SiteKey)
            });
        }
    }
}
