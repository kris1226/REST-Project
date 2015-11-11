using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ClientMappingMaster : AgentClient
    {
        public Guid FieldKey { get; set; }
        public bool UseExternalTable { get; set; }
    }
}
