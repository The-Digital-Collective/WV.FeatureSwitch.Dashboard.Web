using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureSwitchViewModel
    {      
        public List<FeatureModel> Features { get; set; }

        [Required]
        public string CountrySite { get; set; }
    }
}