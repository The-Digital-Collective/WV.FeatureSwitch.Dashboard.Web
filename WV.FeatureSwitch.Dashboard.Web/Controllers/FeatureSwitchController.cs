using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WV.FeatureSwitch.Dashboard.Web.APIClient;
using WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.Web.Helper;
using WV.FeatureSwitch.Dashboard.Web.Models;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.Controllers
{
    [Authorize("Admin")] 
    public class FeatureSwitchController : Controller
    {
        private readonly IFeatureSwitchFactory _featureSwitchFactory;
        private readonly ILogger<FeatureSwitchController> _logger;
        private ApiResponse response;
        private readonly string pageName = "Feature Switch";
        private string _baseUrl = "";
        private string _listOfCountries = "";
        private List<FeatureSwitchViewModel> _featureSwitchViewModelList;

        public FeatureSwitchController(IFeatureSwitchFactory featureSwitchFactory, ILogger<FeatureSwitchController> logger, IConfiguration configuration)
        {
            _featureSwitchFactory = featureSwitchFactory;
            _logger = logger;
            response = new ApiResponse();
            _baseUrl = (AppConfigValues.ApiBaseUrl == null) ? configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value : AppConfigValues.ApiBaseUrl;
            _listOfCountries = (AppConfigValues.ApiCountry == null) ? configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value : AppConfigValues.ApiCountry;
        }

        [ActivatorUtilitiesConstructor]
        public FeatureSwitchController(IFeatureSwitchFactory featureSwitchFactory, ILogger<FeatureSwitchController> logger, IConfiguration configuration, IEnumerable<FeatureSwitchViewModel> testFeatureModelList)
        {
            _featureSwitchFactory = featureSwitchFactory;
            _logger = logger;
            response = new ApiResponse();
            _baseUrl = (AppConfigValues.ApiBaseUrl == null) ? configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value : AppConfigValues.ApiBaseUrl;
            _listOfCountries = (AppConfigValues.ApiCountry == null) ? configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value : AppConfigValues.ApiCountry;
            _featureSwitchViewModelList = testFeatureModelList.ToList();
        }

        // GET: FeatureSwitch       
        public async Task<ActionResult> Index(string notification)
        {
            try
            {
                // Call all the Feature Switch Models, to be displayed/returned to the View
                List<FeatureSwitchViewModel> featureViewModel = await GetAllFeatureLists();
 
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{pageName}", pageName));

                return View(featureViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                response.Message = ConstantMessages.Error;
                return RedirectToAction("Error", "Home", response);
            }
        }

        /// <summary>
        /// Get All Feature Lists
        /// </summary>
        /// <returns></returns>
        public async Task<List<FeatureSwitchViewModel>> GetAllFeatureLists()
        {
            try
            {
                FeatureSwitchViewModel featureSwitchViewModel;
                List<FeatureSwitchViewModel> featureViewModel = new List<FeatureSwitchViewModel>();

                // Check if we have a valid (not empty) URL (being passed in throught appsettings.json)
                if (!string.IsNullOrEmpty(_baseUrl))
                {
                    // Split the Comma-Seperated string of feature names and add them to a list
                    List<string> CountrySites = _listOfCountries.Split(',').ToList();

                    // Loop through all Feature Switch View Models for all country sites
                    foreach (string country in CountrySites)
                    {
                        // For each Feature Switch Model, create the appropriate url for it's country site
                        string url = UrlBuilder.BaseUrlBuilder(_baseUrl, country);
                        List<FeatureModel> featureSwitchViewModelList = await _featureSwitchFactory.LoadList(url);

                        featureSwitchViewModel = new FeatureSwitchViewModel();

                        // Add each Feature Switch View Model to a list of Feature Switch View Model
                        if (featureSwitchViewModelList != null)
                        {
                            featureSwitchViewModel.Features = featureSwitchViewModelList;
                            featureSwitchViewModel.CountrySite = country;
                            featureViewModel.Add(featureSwitchViewModel);
                        }
                    }
                }
                return featureViewModel;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return new List<FeatureSwitchViewModel>(); ;
            }           
        }

        /// <summary>
        /// Update Feature Flag
        /// </summary>
        /// <param name="flag">New Flag Status</param>
        /// <param name="inputFeatureNames">Names of all Features to be updated</param>
        /// <returns></returns>
        [Authorize("Admin")]
        public async Task<ActionResult> Update(bool flag, string inputFeatureNames)
        {
            ViewBag.Action = "Update";

            try
            {
                // Check if any features have been selected
                if (!string.IsNullOrEmpty(inputFeatureNames))
                {
                    // Split the Comma-Seperated string of feature names and add them to a list
                    List<string> inputFeatureNameList = inputFeatureNames.Split(',').ToList();

                    // This if statement is only for testing purposes
                    if (_featureSwitchViewModelList == null || _featureSwitchViewModelList.Count == 0)
                    {
                        _featureSwitchViewModelList = await GetAllFeatureLists();
                    }
                    
                    // Loop through all the Feature Switch View Models
                    foreach (var featureSwitchViewModel in _featureSwitchViewModelList)
                    {
                        // For each Feature Switch Model, create the appropriate url for it's country site
                        string url = UrlBuilder.BaseUrlBuilder(_baseUrl, featureSwitchViewModel.CountrySite);
                        List<FeatureModel> featureModels = featureSwitchViewModel.Features;
                       
                        // Check if there if any common feature names between the lists of the feature switch model, feature names and the selected feature names
                        var result = featureModels.Select(x => x.Name).Intersect(inputFeatureNameList);

                        // Do not enter if there is not common feature names between the lists of the feature switch model, feature names and the selected feature names
                        if (result.Any())
                        {
                            // Loop through the common feature names and update the feature flag status with the flag status specified in the parameter
                            foreach (var item in result)
                            {
                                var newFeatureModel = new FeatureModel() { Name = item, Flag = flag };
                                response = await _featureSwitchFactory.Create(newFeatureModel, url);
                            }
                        }
                    }                    
                    _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                }
                
                return RedirectToAction("Index", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                response.Message = ConstantMessages.Error;
                return RedirectToAction("Index", response);
            }
        }

        /// <summary>
        /// Bulk Create
        /// </summary>
        /// <param name="featureViewModel">New Feature to be created</param>
        /// <returns></returns>
        [HttpPost, ActionName("BulkCreate")]
        public async Task<ActionResult> BulkCreate(FeatureModel featureViewModel)
        {
            ViewBag.Action = "BulkCreate";
            try
            {
                if (ModelState.IsValid)
                {               
                    // Check if we have a valid (not empty) URL (being passed in throught appsettings.json)
                    if (!string.IsNullOrEmpty(_baseUrl))
                    {
                        // Split the Comma-Seperated string of feature names and add them to a list
                        List<string> CountrySites = _listOfCountries.Split(',').ToList();

                        // Bulk create the featureViewModel (feature) to all country sites
                        foreach (string countrySiteName in CountrySites)
                        {
                            string url = UrlBuilder.BaseUrlBuilder(_baseUrl, countrySiteName);
                            response = await _featureSwitchFactory.Create(featureViewModel, url);                            
                        }
                        _logger.LogInformation(ConstantMessages.BulkCreate, pageName);
                    }                    
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return RedirectToAction("Index", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                response.Message = ConstantMessages.Error;
                return RedirectToAction("Index", response);
            }
        }

        /// <summary>
        /// Bulk Delete
        /// </summary>
        /// <param name="featureName">Name of the feature to be deleted</param>
        /// <returns></returns>
        [HttpPost, ActionName("BulkDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BulkDelete(string featureName)
        {
            ViewBag.Action = "BulkDelete";
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if we have a valid (not empty) URL (being passed in throught appsettings.json)
                    if (!string.IsNullOrEmpty(_baseUrl)) 
                    {
                        // Split the Comma-Seperated string of feature names and add them to a list
                        List<string> CountrySites = _listOfCountries.Split(',').ToList();

                        // Bulk delete the featureViewModel (feature) to all country sites
                        foreach (string countrySiteName in CountrySites)
                        {
                            string url = string.Empty;
                            url = UrlBuilder.BaseUrlBuilder(_baseUrl, countrySiteName);
                            response = await _featureSwitchFactory.Delete(featureName, url);
                        }
                        _logger.LogInformation(ConstantMessages.ResetAll, pageName);
                    }                     
                }
                return RedirectToAction("Index", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                response.Message = ConstantMessages.Error;
                return RedirectToAction("Index", response);
            }
        }

        /// <summary>
        /// Reset All 
        /// </summary>
        /// <param name="countrySiteName">Chosen country site to perform ResetAll operation on</param>
        /// <param name="feature">The Feature to be Reseted to default</param>
        /// <returns></returns>
        [HttpPost, ActionName("ResetAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAll(string countrySiteName, FeatureModel feature)
        {
            ViewBag.Action = "ResetAll";
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if we have a valid (not empty) URL (being passed in throught appsettings.json)
                    if (!string.IsNullOrEmpty(_baseUrl))
                    {
                        // Create the url for the chosen country site (countrySiteName)
                        string url = UrlBuilder.BaseUrlBuilder(_baseUrl, countrySiteName);                      

                        // Reset all feature flag statuses to default (false)
                        feature.Flag = false;
                        response = await _featureSwitchFactory.Create(feature, url);
                        _logger.LogInformation(ConstantMessages.ResetAll, pageName);

                    }                      
                }
                return RedirectToAction("Index", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                response.Message = ConstantMessages.Error;
                return RedirectToAction("Index", response);
            }
        }
    }
}