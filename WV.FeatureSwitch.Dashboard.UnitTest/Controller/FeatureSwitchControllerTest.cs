using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using WV.FeatureSwitch.Dashboard.DAL.APIClient;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;
using WV.FeatureSwitch.Dashboard.UnitTest.Mocks.ApiClientFactory;
using WV.FeatureSwitch.Dashboard.Web.Controllers;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.UnitTest.Controller
{
    [TestFixture]
    public class FeatureSwitchControllerTest
    {
        private FeatureSwitchController _featureSwitchController;

        private ApiResponse _apiReponse;
        private Mock<ILogger<FeatureSwitchController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            // Initialise ApiResponse
            _apiReponse = new ApiResponse();

            //logger
            _mockLogger = new Mock<ILogger<FeatureSwitchController>>();            
        }

        #region Index

        /// <summary>
        ///  Validity check on Index  
        /// </summary>
        [Test]
        public void FeatureSwitch_Index_Valid()
        {
            #region Arrange
            
            List<FeatureModel> objList = new List<FeatureModel>();            
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(objList);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<FeatureSwitchViewModel>;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(ViewResult), actionResponse);
            Assert.IsInstanceOf(typeof(List<FeatureSwitchViewModel>), dataResult);

            #endregion
        }

        #endregion

        #region GetAllFeatureLists

        /// <summary>
        /// Return All Feature Lists
        /// </summary>
        [Test]
        public void FeatureSwitch_GetAllFeatureLists_Valid()
        {
            #region Arrange
            
            List<FeatureModel> objList = new List<FeatureModel>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(objList);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.GetAllFeatureLists();
            var actionResponse = actionResult.Result;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(List<FeatureSwitchViewModel>), actionResponse);

            #endregion
        }

        #endregion

        #region BulkCreate

        /// <summary>
        ///  Validity check on Bulk Create Method 
        /// </summary>
        [Test]
        public void FeatureSwitch_BulkCreate_Valid()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestCreateFeature",
                Flag = true
            };
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Bulk Features Created";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, objList, featureTestCreateObject);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object);
            
            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkCreate(featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);

            #endregion
        }

        #endregion

        #region BulkDelete

        /// <summary>
        /// Validity check on Bulk Delete Method 
        /// </summary>
        [Test]
        public void FeatureSwitch_BulkDelete_Valid()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestDeleteFeature",
                Flag = true
            };
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Bulk Features Delete";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockDelete(_apiReponse, objList, featureTestCreateObject.Name);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkDelete(featureTestCreateObject.Name);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);

            #endregion
        }

        #endregion

        #region ResetAll

        /// <summary>
        /// Validity check on Reset All Method 
        /// </summary>
        [Test]
        public void FeatureSwitch_ResetAll_Valid()
        {
            #region Arrange

            string countrySite = "sandbox";
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Chosen Country Site Feature Flag Set To Default";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockDelete(_apiReponse, objList, countrySite);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);

            #endregion
        }

        #endregion
    }
}