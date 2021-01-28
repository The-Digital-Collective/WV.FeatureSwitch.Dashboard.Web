using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureViewModel
    {      
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Flag { get; set; }
        public SelectListItem SelectedItem { get; set; }
    }
}
