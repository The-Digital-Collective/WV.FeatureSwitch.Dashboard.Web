using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.BAL.Models;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureViewModels
    {      
        public List<FeatureModel> FeatureModel { get; set; }
        public CountrySites CountrySites { get; set; }
    }

    public enum CountrySites
    {
        UKCountrySiteConnectionString,
        NetherlandsCountrySiteConnectionString
    }
}
