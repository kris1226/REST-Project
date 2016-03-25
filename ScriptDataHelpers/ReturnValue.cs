using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptDataHelpers
{
    public static class ReturnValue
    {
        public static ScriptReturnValue CreateReturnValueTemplate(Guid scriptKey, string deviceId) {
            return ScriptReturnValue
                       .Build()
                       .WithDeviceId(deviceId)
                       .WithScriptKey(scriptKey)
                       .WithNotEqualKey(new Guid("00000000-0000-0000-0000-000000000000"))
                       .WithEqualKey(new Guid("00000000-0000-0000-0000-000000000000"))
                       .WithReturnValue("SUCCESS")
                       .WithValueOperation("EQ")
                       .Build();
        }
    }
}
