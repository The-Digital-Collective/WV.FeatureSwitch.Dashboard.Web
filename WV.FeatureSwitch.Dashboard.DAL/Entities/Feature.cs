using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WV.FeatureSwitch.Dashboard.DAL.Entities
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Flag { get; set; }
    }
}
