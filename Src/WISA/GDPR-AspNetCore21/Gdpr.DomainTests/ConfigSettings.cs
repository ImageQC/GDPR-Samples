using System;
using System.Collections.Generic;
using System.Text;

namespace Gdpr.DomainTests
{
    public static class ConfigSettings
    {
        public static string EmptyDbConnectionStr = "";
        public static string LocalDbConnectionStr = "Server=(localdb)\\mssqllocaldb;Database=GdprCore21;Trusted_Connection=True;MultipleActiveResultSets=true";
        public static string AzureDbConnectionStr = "Server=(localdb)\\mssqllocaldb;Database=GdprCore21;Trusted_Connection=True;MultipleActiveResultSets=true";

    }
}
