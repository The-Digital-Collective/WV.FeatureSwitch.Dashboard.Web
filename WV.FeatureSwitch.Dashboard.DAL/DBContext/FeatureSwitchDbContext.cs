using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WV.FeatureSwitch.Dashboard.DAL.Entities;

namespace WV.FeatureSwitch.Dashboard.DAL.DBContext
{
    public class FeatureSwitchDbContext : DbContext
    {
        public FeatureSwitchDbContext(DbContextOptions<FeatureSwitchDbContext> options):base(options)
        {

        }

        public DbSet<Feature> Features { get; set; }
    }
}
