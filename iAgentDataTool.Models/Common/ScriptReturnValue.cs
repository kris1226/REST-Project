using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ScriptReturnValue
    {
        private Guid _scriptKey;
        private Guid _equalKey;
        private Guid _notEqualKey;
        private Guid _nextScriptKey;
        private string _returnValue;
        private string _valueOperation;
        private string _deviceId;
        private string _overrideLabel;
        public class Builder
        {
            private Guid _scriptKey;
            private Guid _equalKey;
            private Guid _notEqualKey;
            private Guid _nextScriptKey;
            private string _returnValue;
            private string _valueOperation;
            private string _deviceId;
            private string _overrideLabel;

            public Builder WithScriptKey(Guid value) { _scriptKey = value; return this; }
            public Builder WithEqualKey(Guid value) { _equalKey = value; return this; }
            public Builder WithNotEqualKey(Guid value) { _notEqualKey = value; return this; }
            public Builder WithReturnValue(string value) { _returnValue = value; return this; }
            public Builder WithValueOperation(string value) { _valueOperation = value; return this; }
            public Builder WithDeviceId(string value) { _deviceId = value; return this; }
            public Builder WithOverrideLabel(string value) { _overrideLabel = value; return this; }


         
            public ScriptReturnValue Build()
            {
                return new ScriptReturnValue(
                    _deviceId,
                    _scriptKey,
                    _returnValue,
                    _overrideLabel,
                    _valueOperation,                    
                    _nextScriptKey
               );
            }

        }
        public Guid ScriptKey { get { return _scriptKey; } }
        public Guid NotEquelScriptKey { get { return _notEqualKey; } }
        public Guid EqualScripKey { get { return _equalKey; } }
        public string DeviceId { get { return _deviceId; } }
        public string ReturnValue { get { return _returnValue; } }
        public string ValueOperation { get { return _valueOperation; } }
        public Guid NextScriptKey { get { return _nextScriptKey; } }
        public string MappingValue { get; set; }

           //deviceId, scriptkey, returnValue, overrideLabel, valueOperation, nextScriptID
        public ScriptReturnValue(string deviceId, Guid scriptKey, string returnValue, string overrideLabel, string valueOperation, Guid nextScriptKey)
        {
            _deviceId = deviceId;
            _scriptKey = scriptKey;
            _returnValue = returnValue;
            _overrideLabel = overrideLabel;
            _valueOperation = valueOperation;
            _nextScriptKey = nextScriptKey;
        }

        public static Builder Build() { return new Builder(); }

        public override string ToString()
        {
            return string.Join(" ", new string[] {
               string.Format("{0}", this.ScriptKey),
               string.Format("{0}", this.ReturnValue),
               string.Format("{0}", this.ValueOperation),
               string.Format("{0}", this.NextScriptKey),
               string.Format("{0}", this.MappingValue)
            });
        }
    }

}
