using System;
using System.Reflection;
using System.Threading.Tasks;
using MxReturnCode;

namespace Gdpr.Domain
{
    public class GdprDomain : IGdprDomain
    {
        public static readonly String WebAppVersion = typeof(GdprDomain)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        public static readonly string WebAppName = typeof(GdprDomain)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";

        public string GetComponentName() { return WebAppName;}
        public string GetComponentVersion(){return WebAppVersion;}

    }
}
