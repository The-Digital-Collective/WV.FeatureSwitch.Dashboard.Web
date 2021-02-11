using System.Text;

namespace WV.FeatureSwitch.Dashboard.Web.Helper
{
    public static class UrlBuilder
    {
        /// <summary>
        /// Builds the Base of the Url for A Country Site
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static string BaseUrlBuilder(string baseUrl, string country)
        {
            StringBuilder stringbuilder = new StringBuilder(baseUrl);
            
            if (stringbuilder.ToString().Contains("sandbox"))
            {
                if (country == "ics")
                {
                    stringbuilder.Replace("sandbox", country+"-wv");
                }
                else
                {
                    stringbuilder.Replace("sandbox", country);
                }
            }
            return stringbuilder.ToString();
        }
    }
}