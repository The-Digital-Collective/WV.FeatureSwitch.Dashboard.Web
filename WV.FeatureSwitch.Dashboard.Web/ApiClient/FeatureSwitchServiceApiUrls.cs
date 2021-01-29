namespace WV.FeatureSwitch.Dashboard.Web.APIClient
{
    public class FeatureSwitchServiceApiUrls
    {
        public const string eventApiServicePrefix = "FeatureSwitchService/api/";       

        public static class FeatureSwitchApiUrl
        {
            public const string LoadList = eventApiServicePrefix + "FeatureSwitch/AllFeature";
            public const string Load = eventApiServicePrefix + "FeatureSwitch/GetFeature?name={name}";
            public const string Create = eventApiServicePrefix + "FeatureSwitch/CreateFeature";
            public const string Delete = eventApiServicePrefix + "FeatureSwitch/RemoveFeature?name={name}";
            //delete this
            public const string Update = eventApiServicePrefix + "ChoosingEvent/UpdateChoosingEvent";
                       
        }       
    }
}