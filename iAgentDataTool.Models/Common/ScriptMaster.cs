using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ScriptMaster
    {
        public Guid ScriptKey { get; set; }
        public string ScriptDesc { get; set; }
        public string ScriptCode { get; set; }
        public int NumberOfIterations { get; set; }
        public string Category { get; set; }

        public override string ToString()
        {
            return string.Join("", new string[] 
            {
                string.Format("{0}",  this.ScriptKey),
                string.Format(" {0}", this.ScriptDesc),
                string.Format(" {0}", this.ScriptCode),
                string.Format(" {0}", this.NumberOfIterations),
                string.Format(" {0}", this.Category)
            });
        }
    }
    
}
