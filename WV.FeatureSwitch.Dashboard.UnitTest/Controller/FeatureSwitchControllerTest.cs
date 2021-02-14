using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WV.FeatureSwitch.Dashboard.BAL.Models;
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
        private List<FeatureModel> _featureModels;
        private List<FeatureSwitchViewModel> _featureSwitchViewModels;
        private List<Feature> _features;

        [SetUp]
        public void SetUp()
        {
            // Initialise ApiResponse
            _apiReponse = new ApiResponse();

            //logger
            _mockLogger = new Mock<ILogger<FeatureSwitchController>>();

            #region Arrange

            //Creating Dummy FeatureModel List
            _featureModels = new List<FeatureModel>()
            {
                new FeatureModel (){Id = 1, Name="test 1", Flag = true},
                new FeatureModel (){Id = 2, Name="test 2", Flag = true},
                new FeatureModel (){Id = 3, Name="test 3", Flag = false},
                new FeatureModel (){Id = 4, Name="test 4", Flag = true},
                new FeatureModel (){Id = 5, Name="test 5", Flag = false},
            };

            //Creating Dummy Feature List
            _features = new List<Feature>()
            {
                new Feature (){Id = 1, Name="test 1", Flag = true},
                new Feature (){Id = 2, Name="test 2", Flag = true},
                new Feature (){Id = 3, Name="test 3", Flag = false},
                new Feature (){Id = 4, Name="test 4", Flag = true},
                new Feature (){Id = 5, Name="test 5", Flag = false},
            };

            //Creating Dummy Feature Switch View Model List
            _featureSwitchViewModels = new List<FeatureSwitchViewModel>()
            {
                new FeatureSwitchViewModel() {Features = _features, CountrySite = "sandbox"}
            };

            #endregion
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

            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(_featureModels);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

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
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

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
            Assert.AreEqual(_mockLogger.Invocations.Count, 1);

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

            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockLoadList(_featureModels);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.GetAllFeatureLists().Result;
            var featureList = actionResult[0].Features;

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(List<FeatureSwitchViewModel>), actionResult);
            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.Count > 0);
            Assert.AreEqual(featureList.Count, _featureModels.Count);
            CollectionAssert.AllItemsAreInstancesOfType(featureList, typeof(Feature));

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
                Name = "test 6",
                Flag = true
            };
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Bulk Features Created";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, objList, featureTestCreateObject, null);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

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
                Name = "test 1",
                Flag = true
            };
            var mockFeatureSwitchFactory = new MockFeatureSwitchFactory().MockCreateFeatureModelThrowsException();            
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration, null);

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
            Assert.AreEqual(_mockLogger.Invocations.Count, 1);

            #endregion
        }

        [Test]
        public void BulkCreate_CallsCreate_NewFeatureAdd()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 6,
                Name = "Test 6",
                Flag = true
            };
            var originalFeatureModelsCount = _featureModels.Count;
            _apiReponse.Message = "Success: Bulk Features Created";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, _featureModels, featureTestCreateObject, null);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);
            var originalFeature = _featureModels.Where(x => x.Name == featureTestCreateObject.Name).FirstOrDefault();

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
            Assert.AreNotEqual(originalFeatureModelsCount, _featureModels.Count);
            Assert.Contains(featureTestCreateObject, _featureModels);
            Assert.AreEqual(originalFeature.Name, featureTestCreateObject.Name);

            #endregion
        }

        #endregion

        #region Update

        /// <summary>
        /// Validity check on Update Method 
        /// </summary>
        [Test]
        public void Update_CallsCreate_SuccessfullyReturnsToIndexPage()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "test 1",
                Flag = false
            };
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Record Updated: Record Already Exists";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            string method = "update";
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, objList, featureTestCreateObject, method);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, _featureSwitchViewModels);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Update(featureTestCreateObject.Flag, featureTestCreateObject.Name);
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
            Assert.AreEqual("Record Updated: Record Already Exists", dataMessage);

            #endregion
        }

        [Test]
        public void Update_WhenExceptionThrown_ShowsError()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "test 1",
                Flag = false
            };
            var mockFeatureSwitchFactory = new MockFeatureSwitchFactory().MockCreateFeatureModelThrowsException();
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration, _featureSwitchViewModels);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Update(featureTestCreateObject.Flag, featureTestCreateObject.Name);
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
            Assert.AreEqual(_mockLogger.Invocations.Count, 1);

            #endregion
        }

        [Test]
        public void Update_CallsCreate_UpdateFlagOfExistingFeature()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "test 1",
                Flag = false
            };
            var originalFeatureSwitchViewModelsCount = _featureSwitchViewModels.Count;
            _apiReponse.Message = "Record Updated: Record Already Exists";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            string method = "update";
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, _featureModels, featureTestCreateObject, method);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, _featureSwitchViewModels);
            var featureWithOriginalFlagStatus = _featureModels.Where(x => x.Name == featureTestCreateObject.Name).FirstOrDefault();

            #endregion

            #region Act

            var actionResult = _featureSwitchController.Update(featureTestCreateObject.Flag, featureTestCreateObject.Name);
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
            Assert.AreEqual("Record Updated: Record Already Exists", dataMessage);
            Assert.AreEqual(originalFeatureSwitchViewModelsCount, _featureSwitchViewModels.Count);
            Assert.AreEqual(featureTestCreateObject.Flag, featureWithOriginalFlagStatus.Flag);
            CollectionAssert.AllItemsAreInstancesOfType(_featureSwitchViewModels, typeof(FeatureSwitchViewModel));

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
                Name = "test 1",
                Flag = true
            };
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Bulk Features Delete";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockDelete(_apiReponse, objList, featureTestCreateObject.Name);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

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
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration, null);

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
            Assert.AreEqual(_mockLogger.Invocations.Count, 1);

            #endregion
        }

        [Test]
        public void BulkDelete_CallsDelete_DeleteExistingFeature()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "test 1",
                Flag = true
            };
            var originalFeatureModelsCount = _featureModels.Count;
            _apiReponse.Message = "Success: Bulk Features Delete";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockDelete(_apiReponse, _featureModels, featureTestCreateObject.Name);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

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
            Assert.AreNotEqual(originalFeatureModelsCount, _featureModels.Count); 
            Assert.That(_featureModels, Has.No.Member(featureTestCreateObject));

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
                Name = "test 1",
                Flag = false
            };
            string countrySite = "sandbox";
            IList<FeatureModel> objList = new List<FeatureModel>();
            _apiReponse.Message = "Success: Chosen Country Site Feature Flag Set To Default";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            string method = "resetAll";
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, objList, featureTestCreateObject, method);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);            

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite, featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];
            var validCountrySite = _configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value.Contains(countrySite);

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsTrue(dataResult);
            Assert.AreEqual("Success: Chosen Country Site Feature Flag Set To Default", dataMessage);
            Assert.IsTrue(validCountrySite);
           
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
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactory.Result.Object, _mockLogger.Object, _configuration, null);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite, featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];
            var validCountrySite = _configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value.Contains(countrySite);

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsFalse(dataResult);
            Assert.AreEqual("Error Occurred in While processing your request.", dataMessage);
            Assert.AreEqual(_mockLogger.Invocations.Count, 1);
            Assert.IsTrue(validCountrySite);

            #endregion
        }

        [Test]
        public void ResetAll_CallsCreate_ResetFlagOfExistingFeature()
        {
            #region Arrange

            FeatureModel featureTestCreateObject = new FeatureModel()
            {
                Id = 1,
                Name = "test 1",
                Flag = false
            };
            string countrySite = "sandbox";
            var originalFeatureModelsCount = _featureModels.Count;
            var featureWithOriginalFlagStatus = _featureModels.Where(x => x.Name == featureTestCreateObject.Name).FirstOrDefault();
            _apiReponse.Message = "Success: Chosen Country Site Feature Flag Set To Default";
            _apiReponse.ResponseObject = true;
            _apiReponse.Success = true;
            string method = "resetAll";
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockCreate(_apiReponse, _featureModels, featureTestCreateObject, method);
            _featureSwitchController = new FeatureSwitchController(mockFeatureSwitchFactoryResult.Result.Object, _mockLogger.Object, _configuration, null);

            #endregion

            #region Act

            var actionResult = _featureSwitchController.ResetAll(countrySite, featureTestCreateObject);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var _mockResponse = actionResponse.RouteValues.Values as IList;
            var dataResult = Convert.ToBoolean(_mockResponse[0]);
            var dataMessage = _mockResponse[1];
            var validCountrySite = _configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value.Contains(countrySite);

            #endregion

            #region Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse);
            Assert.AreEqual("Index", actionResponse.ActionName);
            Assert.IsNotNull(_apiReponse);
            Assert.IsTrue(dataResult);
            Assert.AreEqual("Success: Chosen Country Site Feature Flag Set To Default", dataMessage);
            Assert.AreEqual(originalFeatureModelsCount, _featureModels.Count);
            Assert.IsFalse(_featureModels.All(x => x.Flag));
            CollectionAssert.AllItemsAreInstancesOfType(_featureModels, typeof(FeatureModel));
            Assert.IsTrue(validCountrySite);

            #endregion
        }

        #endregion
    }
}