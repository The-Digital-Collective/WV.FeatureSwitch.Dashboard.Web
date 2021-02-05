using System.Collections.Generic;
using WV.FeatureSwitch.Dashboard.BAL.Models;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureSwitchViewModel
    {      
        public List<Feature> Features { get; set; }
        public string CountrySite { get; set; }
    }
}