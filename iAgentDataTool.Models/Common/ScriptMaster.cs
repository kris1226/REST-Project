using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ScriptMaster
    {
        public class Builder {
            private Guid _scriptKey;
            private Guid _websiteKey;
            private string _scriptDesc;
            private string _scriptCode;
            private int _noIterations;
            private string _category;
            private string _deviceId;
            
            public Builder WithScriptKey(Guid value) { _scriptKey = value; return this; }
            public Builder WithWebsiteKey(Guid value) { _websiteKey = value; return this; }
            public Builder WithScriptDesc(string value) { _scriptDesc = value; return this; }
            public Builder WithScriptCode(string value) { _scriptCode = value; return this; }
            public Builder WithNumberOfIterations(int value) { _noIterations = value; return this; }
            public Builder WithCategory(string value) { _category = value; return this; }
            public Builder WithDeviceId(string value) { _deviceId = value; return this; }

            public ScriptMaster Build() { return new ScriptMaster(_scriptKey, _websiteKey, _scriptDesc, _scriptCode, _category, _deviceId, _noIterations); }

        }
        private readonly Guid _scriptKey;
        private Guid _websiteKey;
        public string _scriptDesc;
        private string _scriptCode;
        private int _noIterations;
        private string _category;
        private string _deviceId;


        public Guid ScriptKey { get { return _scriptKey; } }
        public Guid WebsiteKey { get { return _websiteKey; } }
        public string ScriptDesc { get { return _scriptDesc; } }
        public string ScriptCode { get { return _scriptCode; } }
        public int NoIterations { get { return _noIterations; } }
        public string Category { get { return _category; } }
        public string DeviceId { get { return _deviceId; } }
        
        private ScriptMaster(Guid scriptKey, 
                             Guid websiteKey, 
                             string scriptDesc = "no desc found", 
                             string scriptCode = "no code found", 
                             string category ="no category found",
                             string deviceId ="no deviceId found",
                             int noIterations = 0)
        {

            _scriptKey = scriptKey;
            _websiteKey = websiteKey;
            _scriptDesc = scriptDesc;
           _scriptCode =  scriptCode;
            _noIterations = noIterations;
           _deviceId = deviceId;
           _category = category;
        }
        public static Builder Build() { return new Builder(); }

        public override string ToString()
        {
            return string.Join("", new string[] 
            {
                string.Format("{0}:",  this.ScriptKey),
                string.Format(" {0}:", this.ScriptDesc),
                string.Format(" {0}:", this.ScriptCode),
                string.Format(" {0}:", this.NoIterations),
                string.Format(" {0}:", this.Category),
                string.Format(" {0}:", this.WebsiteKey),
                string.Format(" {0}:", this.DeviceId)
            });
        }
    }
    
}
