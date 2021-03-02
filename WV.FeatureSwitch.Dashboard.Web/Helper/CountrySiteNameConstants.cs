using System.Collections.Generic;

namespace WV.FeatureSwitch.Dashboard.Web.Helper
{
    public static class CountrySiteNameConstants
    {
        public static readonly Dictionary<string, string> CountryName = new Dictionary<string, string>
        {
            { "sandbox", "Sandbox" },
            { "staging", "Staging" },
            { "ics", "ICS" },
            { "uk", "UK" },
            { "za", "South Africa" },
            { "es", "Spain" },
            { "ir", "Ireland" },
        };
    }
}