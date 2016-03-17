using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers
{
    public static class IEVersion
    {
        public static Dictionary<int, string> GetIEVersion()
        {
            return new Dictionary<int, string>()
            {
                {9, @"""MSIE 09.0"""},
                {10, @"""MSIE 10.0"""},
                {11, @"""MSIE 11.0"""}
            };
        }
    }
}
