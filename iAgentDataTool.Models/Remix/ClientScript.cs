using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.Models.Remix
{
    public class ClientScript
    {
        public string CriteriaSetName { get; set; }
        public string PayerKey { get; set; }
        public Guid InitialScriptId { get; set; }
        public Guid ClientLocationKey { get; set; }
        public Guid ClientKey { get; set; }
        public int TransactionType { get; set; }
        public string ScriptDesc { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("{0}",  this.CriteriaSetName),
                string.Format("IPR Key: {0} ",  this.PayerKey),
                string.Format("Initial script key: {0}",  this.InitialScriptId),
                string.Format("Client Location Key: {0}",  this.ClientLocationKey),
                string.Format("Client Key: {0}", this.ClientKey)
            });

        }
    }
}
