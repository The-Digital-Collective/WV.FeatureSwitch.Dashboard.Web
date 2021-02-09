using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WV.FeatureSwitch.Dashboard.BAL.Models;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureSwitchViewModel
    {      
        public List<Feature> Features { get; set; }

        [Required]
        public string CountrySite { get; set; }
    }
}