using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
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
        private IConfiguration _configuration;
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

        public FeatureSwitchControllerTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        #region IConfiguration

        [Test]
        public void ConfigurationBuilder_AddJsonFile_CanReadFromConfigurations()
        {
            #region Act

            var baseUrl = _configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value;
            var countrySites = _configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value;

            #endregion

            #region Assert

            Assert.NotNull(baseUrl);
            Assert.NotNull(countrySites);
            Assert.IsNotEmpty(baseUrl);
            Assert.IsNotEmpty(countrySites);

            #endregion
        }

        #endregion

        #region Index

        /// <summary>
        ///  Validity check on Index  
        /// </summary>
        [Test]
        public void Index_GetAllFeatureLists_ReturnsAListOfValidFeatureSwitchViewModelObject()
        {
            #region Arrange

            List<FeatureModel> objList = new List<FeatureModel>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(objList);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<FeatureSwitchViewModel>;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(ViewResult), actionResponse);
            Assert.IsInstanceOf(typeof(List<FeatureSwitchViewModel>), dataResult);
            Assert.IsTrue(dataResult.Count > 0);

            #endregion
        }

        /// <summary>
        ///  Validity check on Index  
        /// </summary>
        [Test]
        public void Index_WhenExceptionThrown_ShowsError()
        {
            #region Arrange

            List<FeatureModel> objList = new List<FeatureModel>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadListFeatureModelThrowsException();
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Index("");
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.IsFalse(dataResult);
            Assert.AreEqual("Error Occurred in While processing your request.", dataMessage);

            #endregion
        }

        #endregion

        #region GetAllFeatureLists

        /// <summary>
        /// Return All Feature Lists
        /// </summary>
        [Test]
        public void GetAllFeatureLists_CallsLoadList_ReturnsAValidListOfFeatureSwitchViewModelObjects()
        {
            #region Arrange

            List<FeatureModel> objList = new List<FeatureModel>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(objList);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.GetAllFeatureLists().Result;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(List<FeatureSwitchViewModel>), actionResult);
            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.Count > 0);

            #endregion
        }

        #endregion

        #region BulkCreate

        /// <summary>
        ///  Validity check on Bulk Create Method 
        /// </summary>
        [Test]
        public void BulkCreate_CallsCreate_SuccessfullyReturnsToIndexPage()
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
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkCreate(featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsTrue(dataResult);
            Assert.AreEqual("Success: Bulk Features Created", dataMessage);

            #endregion
        }

        [Test]
        public void CreateAction_WhenExceptionThrown_ShowsError()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestCreateFeature",
                Flag = true
            };
            var mockFeatureSwitchFactory = new MockFeatureSwitchFactory().MockCreateFeatureModelThrowsException();
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkCreate(featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsFalse(dataResult);
            Assert.AreEqual("Error Occurred in While processing your request.", dataMessage);

            #endregion
        }

        #endregion

        #region BulkDelete

        /// <summary>
        /// Validity check on Bulk Delete Method 
        /// </summary>
        [Test]
        public void BulkDelete_CallsDelete_SuccessfullyReturnsToIndexPage()
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
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkDelete(featureTestCreateObject.Name);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsTrue(dataResult);
            Assert.AreEqual("Success: Bulk Features Delete", dataMessage);

            #endregion
        }

        [Test]
        public void DeleteAction_WhenExceptionThrown_ShowsError()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestDeleteFeature",
                Flag = true
            };
            var mockFeatureSwitchFactory = new MockFeatureSwitchFactory().MockDeleteFeatureModelThrowsException();
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.BulkDelete(featureTestCreateObject.Name);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsFalse(dataResult);
            Assert.AreEqual("Error Occurred in While processing your request.", dataMessage);

            #endregion
        }

        #endregion

        #region ResetAll

        /// <summary>
        /// Validity check on Reset All Method 
        /// </summary>
        [Test]
        public void ResetAll_CallsCreate_SuccessfullyReturnsToIndexPage()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestReset" +
                "Feature",
                Flag = true
            };
            string countrySite = "sandbox";
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Chosen Country Site Feature Flag Set To Default";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, objList, featureTestCreateObject);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite, featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsTrue(dataResult);
            Assert.AreEqual("Success: Chosen Country Site Feature Flag Set To Default", dataMessage);

            #endregion
        }

        [Test]
        public void ResetAllAction_WhenExceptionThrown_ShowsError()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "TestCreateFeature",
                Flag = true
            };
            string countrySite = "sandbox";
            var mockFeatureSwitchFactory = new MockFeatureSwitchFactory().MockCreateFeatureModelThrowsException();
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite, featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsFalse(dataResult);
            Assert.AreEqual("Error Occurred in While processing your request.", dataMessage);

            #endregion
        }

        #endregion
    }
}