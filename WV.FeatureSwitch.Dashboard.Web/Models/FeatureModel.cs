using System.ComponentModel.DataAnnotations;

namespace WV.FeatureSwitch.Dashboard.Web.Models
{
    public class FeatureModel
    {      
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public bool Flag { get; set; }
    }    
}