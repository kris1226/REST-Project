using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAgent.Web.Helpers
{
    public static class Common
    {
        public static bool RecordsExits<T>(this IEnumerable<T> records)
        {
            if (records.Any())
            {
                return true;
            }
            return false;
        }

        public static bool IsValid(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return true;
        }
    }
}