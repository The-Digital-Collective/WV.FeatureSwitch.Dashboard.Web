using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace WV.FeatureSwitch.Dashboard.BAL.Models
{
    public class FeatureModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Flag { get; set; }
        public SelectListItem SelectedItem { get; set; }
    }
}
