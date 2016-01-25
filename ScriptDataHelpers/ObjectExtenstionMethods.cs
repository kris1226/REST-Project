using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptDataHelpers
{
    public static class ObjectExtenstionMethods
    {
        public static bool CheckForNullOrWhiteSpace<T>(this T value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }
            return false;
        }
        static Predicate<T> ConvertToPredicate<T>(Func<T, bool> func)
        {
            return new Predicate<T>(func);
        }
        public static bool AnyPropsNull<T>(T entity)
        {
            return entity.GetType().GetProperties()
                                   .Where(p => p.GetValue(entity) is string)
                                   .Any(p => string.IsNullOrWhiteSpace((p.GetValue(entity) as string)));
        }
    }
}
