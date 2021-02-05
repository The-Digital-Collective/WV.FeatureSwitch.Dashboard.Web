using System;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.DAL.ApiClient;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;


namespace WV.FeatureSwitch.Dashboard.DAL.Helper
{
    public class Serilog
    {
        protected APIClient.ApiClient apiClient;

        public Serilog()
        {
            this.apiClient = new APIClient.ApiClient();
        }

        public Serilog(APIClient.ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }
        public async Task<ApiResponse> Log(string baseUrl, string logContent)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(baseUrl, HelperUrls.SerilogApiUrl.LogError.Replace("{applicationName}", "FeatureSwitchDashboard").Replace("{logContent}", logContent));
                var response = await apiClient.PostAsync<ApiResponse>(requestUrl, null);
                return response;
            }
            catch(Exception ex){

                throw ex;
            }
        }
    }
}
