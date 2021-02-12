using System.ComponentModel.DataAnnotations;

namespace WV.FeatureSwitch.Dashboard.BAL.Models
{
    public class Feature
    {       
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
       
        [Required]
        public bool Flag { get; set; }        
    }
}