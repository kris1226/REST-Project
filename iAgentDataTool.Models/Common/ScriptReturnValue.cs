using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models
{
    public class ScriptReturnValue
    {
        public Guid ScriptKey { get; set; }
        public string ReturnValue { get; set; }
        public string ValueOperation { get; set; }
        public Guid NextScriptId { get; set; }
        public string MappingValue { get; set; }

        public override string ToString()
        {
            return string.Join(" ", new string[] {
               string.Format("{0}", this.ScriptKey),
               string.Format("{0}", this.ReturnValue),
               string.Format("{0}", this.ValueOperation),
               string.Format("{0}", this.NextScriptId),
               string.Format("{0}", this.MappingValue)
            });
        }
    }

}
