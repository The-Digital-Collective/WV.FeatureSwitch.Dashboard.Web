using System.Collections.Generic;
using WV.FeatureSwitch.Dashboard.Web.Models;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureSwitchViewModel
    {      
        public List<FeatureModel> Features { get; set; }

        public string CountrySite { get; set; }
    }
}