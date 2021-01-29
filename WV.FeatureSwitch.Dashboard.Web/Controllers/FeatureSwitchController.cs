using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces;
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
                IList<FeatureViewModel> featureSwitchVMList = await _featureSwitchFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(featureSwitchVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }



























        public ActionResult Create(int? Id = 0)
        {
            ViewBag.Action = "Create";
            try
            {
                FeatureViewModel newCE = new FeatureViewModel();
                
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", newCE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(FeatureViewModel featureViewModel)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<FeatureViewModel> featureViewModelList = await _featureSwitchFactory.LoadList();
                    var featureExistsCount = featureViewModelList.Where(x => x.Name == featureViewModel.Name).Count();
                    if (featureExistsCount == 0)
                    {
                        var response = await _featureSwitchFactory.Create(featureViewModel);
                        _logger.LogInformation(ConstantMessages.Create, "CreateSave");
                        return RedirectToAction("Index", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));                        
                    }
                }             
                return View("Edit", featureViewModel);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ConstantMessages.Error);
                // return RedirectToAction("Error", "Home");
                ModelState.AddModelError("Error", ConstantMessages.Error);           
                return View("Index");
            }
        }

        public async Task<ActionResult> Edit(string name)
        {
            ViewBag.Action = "Edit";

            try
            {               
                FeatureViewModel featureViewModel = await _featureSwitchFactory.Load(name);
                if (featureViewModel == null)
                {
                    return NotFound();
                }
                
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", featureViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ConstantMessages.Error, ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ChoosingParty/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(FeatureViewModel featureViewModel)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<FeatureViewModel> featureVMList = await _featureSwitchFactory.LoadList();
                    var featureExistsCount = featureVMList.Where(x => x.Id == featureViewModel.Id).Count();
                    if (featureExistsCount >= 1)
                    {
                        var response = await _featureSwitchFactory.Create(featureViewModel);
                        _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                        return RedirectToAction("Index", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                    }
                }

                ViewBag.Action = pageAction;                
                return View("Edit", featureViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", featureViewModel);
            }
        }

        public async Task<ActionResult> Delete(string name)
        {
            try
            {
                FeatureViewModel featureViewModel = await _featureSwitchFactory.Load(name);
                if (featureViewModel == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                
                return View(featureViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string name)
        {
            try
            {
                var response = await _featureSwitchFactory.Delete(name);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                //FeatureViewModel cpVM = await _featureSwitchFactory.Load(id);
                //ModelState.AddModelError("Error", ConstantMessages.Error);
                //return View(cpVM);
                return View("Index");
            }
        }
    }
}