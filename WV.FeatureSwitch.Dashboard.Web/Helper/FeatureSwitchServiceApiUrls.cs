namespace WV.FeatureSwitch.Dashboard.Web.APIClient
{
    public class FeatureSwitchServiceApiUrls
    {
        public const string featureSwitchApiServicePrefix = "/FeatureSwitchService/api/";       

        public static class FeatureSwitchApiUrl
        {
            public const string LoadList = featureSwitchApiServicePrefix + "FeatureSwitch/AllFeature";
            public const string Create = featureSwitchApiServicePrefix + "FeatureSwitch/CreateFeature";
            public const string Delete = featureSwitchApiServicePrefix + "FeatureSwitch/RemoveFeature?name={name}";
        }       
    }
}