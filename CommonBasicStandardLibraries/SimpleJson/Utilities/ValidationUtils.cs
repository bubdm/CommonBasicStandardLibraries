using System;


namespace SimpleJson.Utilities
{
    internal static class ValidationUtils
    {
        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
