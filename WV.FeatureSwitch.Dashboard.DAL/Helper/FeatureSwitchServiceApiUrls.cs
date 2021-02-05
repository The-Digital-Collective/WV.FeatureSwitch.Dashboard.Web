namespace WV.FeatureSwitch.Dashboard.DAL.APIClient
{
    public class FeatureSwitchServiceApiUrls
    {
        public const string eventApiServicePrefix = "/FeatureSwitchService/api/";       

        public static class FeatureSwitchApiUrl
        {
            public const string LoadList = eventApiServicePrefix + "FeatureSwitch/AllFeature";
            public const string Create = eventApiServicePrefix + "FeatureSwitch/CreateFeature";
            public const string Delete = eventApiServicePrefix + "FeatureSwitch/RemoveFeature?name={name}";
        }       
    }
}