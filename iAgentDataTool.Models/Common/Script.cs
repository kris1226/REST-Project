using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class Script
    {
        public Guid ScriptKey { get; set; }
        public string DeviceId { get; set; }
        public Guid WebsiteKey { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string WebsiteDescription { get; set; }  
        public int NumberOfIterations { get; set; } 
        public ICollection<ScriptCollectionItem> CollectionItems { get; set; }
        public ICollection<ScriptReturnValue> ReturnValues { get; set; }
        public ImmutableList<Script> Scripts { get; private set; }

        private Script(string websiteDescription, string code, string deviceId, string category, Guid websiteKey)
        {
            this.WebsiteDescription = websiteDescription;
            this.Code = code;
            this.DeviceId = deviceId;
            this.Category = category;
            this.WebsiteKey = websiteKey;
        }
        //public Script WithScriptCode(IEnumerable<Script>)
        public static Script CreateScript(string websiteDescription, string code, string deviceId, string category, Guid websiteKey)
        {
            return new Script(websiteDescription, code, deviceId, category, websiteKey);
        }
    }
}
