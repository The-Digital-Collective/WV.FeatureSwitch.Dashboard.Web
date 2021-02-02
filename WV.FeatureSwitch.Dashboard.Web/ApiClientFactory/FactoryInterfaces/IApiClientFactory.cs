using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.Web.APIClient;

namespace WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces
{
    public interface IApiClientFactory<T> : IDisposable
    {

        /// <summary>
        /// Load the details from Table of <T>
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<T></returns>
        Task<List<T>> LoadList();

        /// <summary>
        ///Load the details from Table of<T> based on Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>T</returns>
        Task<T> Load(string name);

        /// <summary>
        ///Create the new entry in Table of<T>
        /// </summary>
        /// <param name="T"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Create(T t);

        /// <summary>
        ///Update the entry in Table of<T> based on Id
        /// </summary>
        /// <param name="T"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Update(T t);

        /// <summary>
        ///Delete the entry in Table of<T> based on Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Delete(string name);
  
        /// <summary>
        /// Reset All entries to Default in Table based on A Chosen Country Site  
        /// </summary>
        /// <param name="CountrySiteName"></param>
        /// <returns></returns>
        Task<ApiResponse> ResetAll(string CountrySiteName);
    }
}
