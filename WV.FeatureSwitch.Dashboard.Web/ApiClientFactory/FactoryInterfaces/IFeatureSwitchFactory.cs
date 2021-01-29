using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces
{
    public interface IFeatureSwitchFactory : IApiClientFactory<FeatureViewModel>
    {
    }
}
