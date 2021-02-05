using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WV.FeatureSwitch.Dashboard.BAL.Models;
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
        private readonly string pageName = "Feature Switch";
        
        public FeatureSwitchController(IFeatureSwitchFactory featureSwitchFactory, ILogger<FeatureSwitchController> logger)
        {
            _featureSwitchFactory = featureSwitchFactory;
            _logger = logger;            
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
                return RedirectToAction("Error", "Home");
            }
        }        

        /// <summary>
        /// Get All Feature Lists
        /// </summary>
        /// <returns></returns>
        public async Task<List<FeatureSwitchViewModel>> GetAllFeatureLists()
        {
            string baseUrl = AppConfigValues.ApiBaseUrl;
            var listOfCountries = AppConfigValues.ApiCountry;
            List<string> CountrySites = listOfCountries.Split(',').ToList();
            List<FeatureSwitchViewModel> featureViewModel = new List<FeatureSwitchViewModel>();

            foreach (string country in CountrySites)
            {
                string url = UrlBuilder.BaseUrlBuilder(baseUrl, country);
                List<FeatureModel> featureSwitchViewModelMList = new List<FeatureModel>();
                featureSwitchViewModelMList = await _featureSwitchFactory.LoadList(url);

                FeatureSwitchViewModel featureSwitchViewModel = new FeatureSwitchViewModel() 
                {
                    Features = new List<Feature>(),
                };

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
                    string baseUrl = AppConfigValues.ApiBaseUrl;
                    var listOfCountries = AppConfigValues.ApiCountry;
                    List<string> CountrySites = listOfCountries.Split(',').ToList();

                    foreach (string countrySiteName in CountrySites)
                    {
                        string url = UrlBuilder.BaseUrlBuilder(baseUrl, countrySiteName);
                        var response = await _featureSwitchFactory.Create(featureViewModel, url);
                    }
                    _logger.LogInformation(ConstantMessages.BulkCreate, pageName);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Index");
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
                    string baseUrl = AppConfigValues.ApiBaseUrl;
                    var listOfCountries = AppConfigValues.ApiCountry;
                    List<string> CountrySites = listOfCountries.Split(',').ToList();

                    foreach (string countrySiteName in CountrySites)
                    {
                        string url = string.Empty;
                        url = UrlBuilder.BaseUrlBuilder(baseUrl, countrySiteName);
                        var response = await _featureSwitchFactory.Delete(featureName, url);
                    }
                    _logger.LogInformation(ConstantMessages.ResetAll, pageName);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return View("Index");
            }
        }

        /// <summary>
        /// Reset All 
        /// </summary>
        /// <param name="countrySiteName"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ResetAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAll(string countrySiteName)
        {
            ViewBag.Action = "ResetAll";
            try
            {
                if (ModelState.IsValid)
                {
                    string baseUrl = AppConfigValues.ApiBaseUrl;
                    string url = UrlBuilder.BaseUrlBuilder(baseUrl, countrySiteName);
                    List<FeatureModel> featureSwitchViewModelMList = new List<FeatureModel>();
                    featureSwitchViewModelMList = await _featureSwitchFactory.LoadList(url);

                    foreach (var feature in featureSwitchViewModelMList)
                    {
                        feature.Flag = false;
                        var response = await _featureSwitchFactory.Create(feature, url);
                    }
                    _logger.LogInformation(ConstantMessages.ResetAll, pageName);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return View("Index");
            }
        }
    }
}