using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;

namespace WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.FactoryInterfaces
{
    public interface IApiClientFactory<T> : IDisposable
    {
        /// <summary>
        /// Load the details from Table of <T>
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<T></returns>
        Task<List<T>> LoadList(string baseUrl);

        /// <summary>
        ///Create the new entry in Table of<T>
        /// </summary>
        /// <param name="T"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Create(T t, string baseUrl);

        /// <summary>
        ///Delete the entry in Table of<T> based on Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Delete(string name, string baseUrl);
    }
}