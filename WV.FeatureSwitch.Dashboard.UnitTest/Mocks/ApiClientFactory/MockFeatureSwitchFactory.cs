using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.BAL.Models;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;
using WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;

namespace WV.FeatureSwitch.Dashboard.UnitTest.Mocks.ApiClientFactory
{
    public class MockFeatureSwitchFactory : Mock<IFeatureSwitchFactory>
    {
        public async Task<MockFeatureSwitchFactory> MockLoadList(List<FeatureModel> result)
        {
            Setup(x => x.LoadList(It.IsAny<string>())).
            Returns(Task.Run(() => new List<FeatureModel>(result)));
            return await Task.FromResult(this);
        }

        public async Task<MockFeatureSwitchFactory> MockCreate(ApiResponse result, IList<FeatureModel> objList, FeatureModel objCreate)
        {
            Setup(x => x.Create(It.IsAny<FeatureModel>(), It.IsAny<string>())).
            Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockFeatureSwitchFactory> MockDelete(ApiResponse result, IList<FeatureModel> objList, string featureName)
        {
            Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).
            Returns(Task.Run(() => result));
            return await Task.FromResult(this); 
        }
        public async Task<MockFeatureSwitchFactory> MockLoadListFeatureModelThrowsException()
        {
            Setup(x => x.LoadList(It.IsAny<string>())).
            Throws<Exception>();
            return await Task.FromResult(this);
        }

        public async Task<MockFeatureSwitchFactory> MockCreateFeatureModelThrowsException()
        {
            Setup(x => x.Create(It.IsAny<FeatureModel>(), It.IsAny<string>())).
            Throws<Exception>();
            return await Task.FromResult(this);
        }

        public async Task<MockFeatureSwitchFactory> MockDeleteFeatureModelThrowsException()
        {
            Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).
            Throws<Exception>();
            return await Task.FromResult(this);
        }

        public bool MockValidFeatureSwitchViewModelFeature(Feature feature)
        {
            var result = (feature != null) ? true : false;
            return result;
        }

        public bool MockValidFeatureSwitchViewModelFeatureName(string featureName)
        {
            var result = (featureName != string.Empty) ? true : false;
            return result;
        }

        public bool MockValidFeatureSwitchViewModelCountrySite(string countrySite)
        {
            var result = (countrySite != null) ? true : false;
            return result;
        }
    }
}