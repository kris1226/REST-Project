using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Remix
{
    public class AgentSchedule
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid WebsiteKey { get; set; }
        public string PayerKey { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime NextRun { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastRun { get; set; }
        public bool ScriptFinished { get; set; }
    }
}
