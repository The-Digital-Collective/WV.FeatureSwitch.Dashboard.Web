namespace WV.FeatureSwitch.Dashboard.DAL.APIClient
{
    public class HelperUrls
    {
        public const string eventApiServicePrefix = "/SerilogService/api/";
        public static class SerilogApiUrl
        {
            public const string LogError = eventApiServicePrefix + "Serilog/LogErrors?applicationName={applicationName}&logContent={logContent}";
        }
    }
}