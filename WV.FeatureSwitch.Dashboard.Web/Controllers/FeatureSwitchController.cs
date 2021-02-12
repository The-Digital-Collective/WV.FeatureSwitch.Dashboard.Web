using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WV.FeatureSwitch.Dashboard.BAL.Models;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;
using WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;
using WV.FeatureSwitch.Dashboard.Web.Helper;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.Controllers
{
    public class FeatureSwitchController : Controller
    {
        private readonly IFeatureSwitchFactory _featureSwitchFactory;
        private readonly ILogger<FeatureSwitchController> _logger;
        private ApiResponse response;
        private readonly string pageName = "Feature Switch";
        private string _baseUrl = "";
        private string _listOfCountries = "";

        public FeatureSwitchController(IFeatureSwitchFactory featureSwitchFactory, ILogger<FeatureSwitchController> logger, IConfiguration configuration)
        {
            _featureSwitchFactory = featureSwitchFactory;
            _logger = logger;
            response = new ApiResponse();
            _baseUrl = (AppConfigValues.ApiBaseUrl == null) ? configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value : AppConfigValues.ApiBaseUrl;
            _listOfCountries = (AppConfigValues.ApiCountry == null) ? configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value : AppConfigValues.ApiCountry;
        }

        // GET: FeatureSwitch       
        public async Task<ActionResult> Index(string notification)
        {
            try
            {                    
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
            List<FeatureSwitchViewModel> featureViewModel = new List<FeatureSwitchViewModel>();

            if (!string.IsNullOrEmpty(_baseUrl))
            {
                List<string> CountrySites = _listOfCountries.Split(',').ToList();

                foreach (string country in CountrySites)
                {
                    string url = UrlBuilder.BaseUrlBuilder(_baseUrl, country);
                    List<FeatureModel> featureSwitchViewModelMList = new List<FeatureModel>();
                    featureSwitchViewModelMList = await _featureSwitchFactory.LoadList(url);

                    FeatureSwitchViewModel featureSwitchViewModel = new FeatureSwitchViewModel()
                    {
                        Features = new List<Feature>(),
                    };

                    if (featureSwitchViewModelMList != null)
                    {
                        foreach (var item in featureSwitchViewModelMList)
                        {
                            featureSwitchViewModel.Features.Add(new Feature
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Flag = item.Flag
                            });
                        }
                        featureSwitchViewModel.CountrySite = country;
                        featureViewModel.Add(featureSwitchViewModel);
                    }                    
                }
            }            
            return featureViewModel;
        }

        /// <summary>
        /// Bulk Create
        /// </summary>
        /// <param name="featureViewModel"></param>
        /// <returns></returns>
        [HttpPost, ActionName("BulkCreate")]
        public async Task<ActionResult> BulkCreate(FeatureModel featureViewModel)
        {
            ViewBag.Action = "BulkCreate";
            try
            {
                if (ModelState.IsValid)
                {               
                    if (!string.IsNullOrEmpty(_baseUrl))
                    {
                        List<string> CountrySites = _listOfCountries.Split(',').ToList();

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
        /// <param name="featureName"></param>
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
                    if (!string.IsNullOrEmpty(_baseUrl)) 
                    {
                        List<string> CountrySites = _listOfCountries.Split(',').ToList();

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
        /// <param name="countrySiteName"></param>
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
                    if (!string.IsNullOrEmpty(_baseUrl))
                    {
                        string url = UrlBuilder.BaseUrlBuilder(_baseUrl, countrySiteName);                      

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