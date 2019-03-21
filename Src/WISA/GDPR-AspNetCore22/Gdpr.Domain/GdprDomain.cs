using System;
using System.Reflection;

namespace Gdpr.Domain
{
    public class GdprDomain
    {
        public static readonly String WebAppVersion = typeof(GdprDomain)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        public static readonly string WebAppName = typeof(GdprDomain)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";

        public static string GetVersion()
        {
            string rc = "[not set}";

            var ver = Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            if (ver != null)
                rc = ver;
            return rc;
        }
    }
}
