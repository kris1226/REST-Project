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
            private int _numberOfIterations;
            private string _category;
            private string _deviceId;
            
            public Builder WithScriptKey(Guid value) { _scriptKey = value; return this; }
            public Builder WithWebsiteKey(Guid value) { _websiteKey = value; return this; }
            public Builder WithScriptDesc(string value) { _scriptDesc = value; return this; }
            public Builder WithScriptCode(string value) { _scriptCode = value; return this; }
            public Builder WithNumberOfIterations(int value) { _numberOfIterations = value; return this; }
            public Builder WithCategory(string value) { _category = value; return this; }
            public Builder WithDeviceId(string value) { _deviceId = value; return this; }

            public ScriptMaster Build() { return new ScriptMaster(_scriptKey, _websiteKey, _scriptDesc, _scriptCode, _category, _deviceId, _numberOfIterations); }

        }
        private Guid _scriptKey;
        private Guid _websiteKey;
        public string _scriptDesc;
        private string _scriptCode;
        private int _numberOfIterations;
        private string _category;
        private string _deviceId;


        public Guid ScriptKey { get { return _scriptKey; } }
        public Guid WebsiteKey { get { return _websiteKey; } }
        public string ScriptDesc { get { return _scriptDesc; } }
        public string ScriptCode { get { return _scriptCode; } }
        public int NumberOfIterations { get { return _numberOfIterations; } }
        public string Category { get { return _category; } }
        public string DeviceId { get { return _deviceId; } }
        
        private ScriptMaster(Guid scriptKey, 
                             Guid websiteKey, 
                             string scriptDesc = "", 
                             string scriptCode = "", 
                             string category ="", 
                             string deviceId ="", 
                             int numberOfIterations = 0) {

            if (scriptKey.Equals(new Guid("00000000-0000-0000-0000-000000000000"))) {
                scriptKey = Guid.NewGuid();
            }
            else
            {
                _scriptKey = scriptKey;
            }
            _websiteKey = websiteKey;
            _scriptDesc = scriptDesc;
           _scriptCode =  scriptCode;
            _numberOfIterations = numberOfIterations;
           _deviceId = deviceId;
        }
        public static Builder Build() { return new Builder(); }

        public override string ToString()
        {
            return string.Join("", new string[] 
            {
                string.Format("{0}:",  this.ScriptKey),
                string.Format(" {0}:", this.ScriptDesc),
                string.Format(" {0}:", this.ScriptCode),
                string.Format(" {0}:", this.NumberOfIterations),
                string.Format(" {0}:", this.Category)
            });
        }
    }
    
}
