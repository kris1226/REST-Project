using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAgentDataTool.Models.Common
{
    public class ScriptCollectionItem
    {
        public Guid ScriptKey { get; set; }
        public string DeviceId { get; set; }
        public string OverrideLabel { get; set; }
        public Guid FieldKey { get; set; }
        public string ReturnValue { get; set; }
        public string ValueOperation { get; set; }
        public string Sentinel { get; set; }
        public Guid NextScriptId { get; set; }
    }
}
