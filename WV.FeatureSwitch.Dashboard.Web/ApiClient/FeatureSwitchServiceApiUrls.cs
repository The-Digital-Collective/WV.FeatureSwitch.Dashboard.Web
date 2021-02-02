namespace WV.FeatureSwitch.Dashboard.Web.APIClient
{
    public class FeatureSwitchServiceApiUrls
    {
        //public const string eventApiServicePrefix = "FeatureSwitchService/api/";       
        public const string eventApiServicePrefix = "/api/";

        public static class FeatureSwitchApiUrl
        {
            public const string LoadList = eventApiServicePrefix + "FeatureSwitch/AllFeatureList";
            public const string Load = eventApiServicePrefix + "FeatureSwitch/GetFeature?name={name}";
            public const string Create = eventApiServicePrefix + "FeatureSwitch/CreateBatchFeature";
            public const string Delete = eventApiServicePrefix + "FeatureSwitch/RemoveBatchFeature?name={name}";
            public const string ResetAll = eventApiServicePrefix + "FeatureSwitch/ResetAllFeatures";


           
            public const string Deletes = eventApiServicePrefix + "FeatureSwitch/RemoveBatchFeature";
            //public const string ResetAll = eventApiServicePrefix + "FeatureSwitch/ResetUpdateBatchFeature";




            //delete this
            public const string Update = eventApiServicePrefix + "ChoosingEvent/UpdateChoosingEvent";
                       
        }       
    }
}