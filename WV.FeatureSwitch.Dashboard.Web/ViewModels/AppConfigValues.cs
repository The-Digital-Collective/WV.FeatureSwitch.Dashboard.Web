using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public static class AppConfigValues
    {
        public static string ApiBaseUrl { get; set; }
        public static string ApiToken { get; set; }
        public static string ApiVersion { get; set; }
        public static string LogStorageContainer { get; set; }
        public static string StorageAccountKey { get; set; }
        public static string StorageAccountName { get; set; }
        public static string CRMType { get; set; }
        public static string XSLTStorageContainer { get; set; }
        public static string BaseApiBaseUrl { get; set; }
        public static string BaseAdyenApiBaseUrl { get; set; }
        public static string HostedCountry { get; set; }
    }
}
