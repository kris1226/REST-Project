using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptDataHelpers;

namespace Submits.cs
{
    public static class MMGColItems
    {
        private static readonly string _deviceId = "MMG";
        private static readonly Dictionary<string, Guid> smartAgentCollectionItems = SmartAgentCollectionItems.GetCollectionItemKeys();

        public static IEnumerable<ScriptCollectionItem> LoginCollectionItems(Guid scriptKey)
        {
            return new List<ScriptCollectionItem>()
            { 
                new ScriptCollectionItem(
                    scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["websiteDomain"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "website domain"),
                new ScriptCollectionItem(scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["websiteUsername"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "website username"),
                new ScriptCollectionItem(scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["websitePassword"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "website password")
            };
        }

        public static IEnumerable<ScriptCollectionItem> PatientDemoGraphicsCollectionItems(Guid scriptKey)
        {
            return new List<ScriptCollectionItem>()
            { 
                new ScriptCollectionItem(
                    scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["MemberID"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "member id"),
                new ScriptCollectionItem(scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["PatLname"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "patient last name"),
                new ScriptCollectionItem(scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["PatFname"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "patient first name"),
                new ScriptCollectionItem(scriptKey: scriptKey, 
                    fieldKey: smartAgentCollectionItems["PatDOB"], 
                    deviceId: string.Concat(_deviceId, 1), 
                    overrideLabel: "patient date of birth")
            };
        }
    }
}
