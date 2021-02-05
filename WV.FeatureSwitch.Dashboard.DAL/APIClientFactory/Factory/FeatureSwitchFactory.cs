using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;
using WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.DAL.Helper;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;


namespace WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.Factory
{
    public class FeatureSwitchFactory : IFeatureSwitchFactory
    {

        protected APIClient.ApiClient apiClient;
        protected Serilog _serilog;

        public FeatureSwitchFactory()
        {
            this.apiClient = new APIClient.ApiClient();
            _serilog = new Serilog();
        }

        public FeatureSwitchFactory(APIClient.ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }        

        public async Task<List<FeatureModel>> LoadList(string baseUrl)
        {   
            try
            {
                List<FeatureModel> featureSwitchVMList = new List<FeatureModel>();
                var requestUrl = apiClient.CreateRequestUri(baseUrl, FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                featureSwitchVMList = JsonConvert.DeserializeObject<List<FeatureModel>>(Convert.ToString(response.ResponseObject));

                return featureSwitchVMList;
            }
            catch (Exception ex)
            {
                await _serilog.Log(baseUrl, ex.Message);
                return new List<FeatureModel>();
            }
        }

        /// <summary>
        /// Create (and Update of existing Feature)
        /// </summary>
        /// <param name="featureVM"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Create(FeatureModel featureModel, string baseUrl)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(baseUrl, FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Create);
                var response=  await apiClient.PostAsync<FeatureModel>(requestUrl, featureModel);
                return response;
            }
            catch (Exception ex)
            {
                await _serilog.Log(baseUrl, ex.Message);
                throw ex;
            }
        }
       
        public async Task<ApiResponse> Delete(string featureName, string baseUrl)
        {
            try
            {
                ApiResponse response = null;
                var requestUrl = apiClient.CreateRequestUri(baseUrl, FeatureSwitchServiceApiUrls.FeatureSwitchApiUrl.Delete.Replace("{name}", featureName));
                response = await apiClient.DeleteAsync<string>(requestUrl);

                return response;
            }
            catch (Exception ex)
            {
                await _serilog.Log(baseUrl, ex.Message);
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