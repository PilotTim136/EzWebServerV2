using System;

namespace EzWebServerV2
{
    public static class ValueExtensions
    {
        public static T? Value<T>(this string str)
        {
            try
            {
                return (T?)Convert.ChangeType(str, typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
}
