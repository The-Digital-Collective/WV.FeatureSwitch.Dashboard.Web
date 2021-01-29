using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WV.FeatureSwitch.Dashboard.Web.ViewModels
{
    public class FeatureViewModel
    {      
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public bool Flag { get; set; }
    }
}
