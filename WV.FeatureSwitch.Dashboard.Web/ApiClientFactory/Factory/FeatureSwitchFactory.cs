
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WV.FeatureSwitch.Dashboard.Web.APIClient;
using WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.Factory
{
    public class FeatureSwitchFactory : IFeatureSwitchFactory
    {

        protected ApiClient apiClient;                

        public FeatureSwitchFactory()
        {
            this.apiClient = new ApiClient();
        }

        public FeatureSwitchFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }        

        public async Task<List<FeatureViewModel>> LoadList()
        {   
            try
            {
                List<FeatureViewModel> featureSwitchVMList = new List<FeatureViewModel>();
                var requestUrl = apiClient.CreateRequestUri(FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                featureSwitchVMList = JsonConvert.DeserializeObject<List<FeatureViewModel>>(Convert.ToString(response.ResponseObject));
                return featureSwitchVMList;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<FeatureViewModel> Load(string name)
        {
            try
            {
                FeatureViewModel featureViewModel = new FeatureViewModel();
                var requestUrl = apiClient.CreateRequestUri(FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Load.Replace("{name}", name));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                featureViewModel = JsonConvert.DeserializeObject<FeatureViewModel>(Convert.ToString(response.ResponseObject));
                return featureViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create/Update
        /// </summary>
        /// <param name="featureVM"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Create(FeatureViewModel featureVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Create);
                var response=  await apiClient.PostAsync<FeatureViewModel>(requestUrl, featureVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }
     
        /// <summary>
        /// Remove this zaki - doesn't work
        /// </summary>
        /// <param name="featureVM"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Update(FeatureViewModel featureVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Create);
                var response = await apiClient.PutSync<FeatureViewModel>(requestUrl, featureVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Delete(string name)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Delete.Replace("{name}", name));
                var response = await apiClient.DeleteAsync<FeatureViewModel>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

      

        private bool disposed = false;

        /// <summary>
        ///Dispose the object used
        /// </summary>
        /// <param name=""></param>
        /// <returns>no values</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.apiClient.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

      

     
    }
}