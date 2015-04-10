using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.StringHelpers
{
    public static class StringExtensions
    {
        public static bool IsInt(this string text)
        {
            int num;
            return int.TryParse(text, out num);
        }

        //Get Null if conversion falied
        public static int? ToIntOrRetrunNull(this string text)
        {
            int num;
            return int.TryParse(text, out num) ? num : (int?)null;
        }

        public static string StringFormat(this string text, params object[] formatArgs)
        {
            return string.Format(text, formatArgs);
        }

        public static int ToInt(this string text)
        {
            return int.Parse(text);
        }
    }
}
