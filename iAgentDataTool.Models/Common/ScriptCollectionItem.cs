using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAgentDataTool.Models.Common
{
    public class ScriptCollectionItem
    {
        private Guid _scriptKey;
        private Guid _fieldKey;
        string _deviceId;
        string _overrideLabel;

        public Guid ScriptKey { get { return _scriptKey; } }
        public string DeviceId { get { return _deviceId; } }
        public string OverrideLabel { get { return _overrideLabel; } }
        public Guid FieldKey { get { return _fieldKey; } }
        public string ReturnValue { get; set; }
        public string ValueOperation { get; set; }
        public string Sentinel { get; set; }
        public Guid NextScriptId { get; set; }

        public ScriptCollectionItem(Guid scriptKey, Guid fieldKey, string deviceId, string overrideLabel)
        {
            _scriptKey = scriptKey;
            _fieldKey = fieldKey;
            _deviceId = deviceId;
            _overrideLabel = overrideLabel;
        }
    }
}
