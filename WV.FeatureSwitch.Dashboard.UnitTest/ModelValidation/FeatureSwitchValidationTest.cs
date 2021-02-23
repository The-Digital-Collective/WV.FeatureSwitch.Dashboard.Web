using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;
using WV.FeatureSwitch.Dashboard.UnitTest.Mocks.ApiClientFactory;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.UnitTest.ModelValidation
{
    [TestFixture]
    public class FeatureSwitchValidationTest
    {
        FeatureSwitchViewModel _featureSwitchViewModelValid = null;
        FeatureSwitchViewModel _featureSwitchViewModelInvalid = null;

        [SetUp]
        public void TestInitialize()
        {
            _featureSwitchViewModelValid = new FeatureSwitchViewModel
            {
                Features = new List<FeatureModel>()
                {
                    new FeatureModel() { Id = 1, Name = "feature", Flag = true },
                },
                CountrySite = "sandbox"
            };

            _featureSwitchViewModelInvalid = new FeatureSwitchViewModel
            {
                Features = new List<FeatureModel>()
                {
                    new FeatureModel() { Id = 1, Name = string.Empty, Flag = true }
                },
                CountrySite = ""
            };
        }

        #region Test For Validation

        [Test]
        public void CreateFeatureSwitch_ValidationOnValidModelPasses()
        {
            #region Arrange
            
            var context = new ValidationContext(_featureSwitchViewModelValid, null, null);
            var results = new List<ValidationResult>();

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelValid, context, results, true);

            #endregion

            #region Assert

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);

            #endregion
        }

        [Test]
        public void CreateFeatureSwitch_ValidationOnFeatureSwitchViewModelObject()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelValid.Features[0], null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeature(_featureSwitchViewModelValid.Features[0]);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelValid.Features[0], context, results, true);

            #endregion

            #region Assert

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchFactoryResult);

            #endregion
        }

        [Test]
        public void FeatureSwitchViewModel_ValidationOnFeatureSwitchViewModelObjectFeatureName()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelValid.Features[0].Name, null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeatureName(_featureSwitchViewModelValid.Features[0].Name);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelValid.Features[0].Name, context, results, true);

            #endregion

            #region Assert

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchFactoryResult);

            #endregion
        }

        [Test]
        public void CreateFeatureSwitch_ValidationOnFeatureSwitchViewModelObjectCountrySite()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelValid.CountrySite, null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchManagerResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelCountrySite(_featureSwitchViewModelValid.CountrySite);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelValid.CountrySite, context, results, true);

            #endregion

            #region Assert

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchManagerResult);

            #endregion
        }

        #endregion

        #region Test For Invalidation

        [Test]
        public void CreateFeatureSwitch_InvalidationOnFeatureSwitchViewModelObject()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelInvalid.Features[0], null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeature(_featureSwitchViewModelInvalid.Features[0]);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelInvalid.Features[0], context, results, true);

            #endregion

            #region Assert

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchFactoryResult);

            #endregion
        }

        [Test]
        public void CreateFeatureSwitch_InvalidationOnFeatureSwitchViewModelObjectFeatureName()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelInvalid.Features[0], null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeatureName(_featureSwitchViewModelInvalid.Features[0].Name);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelInvalid.Features[0], context, results, true);

            #endregion

            #region Assert

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(false, mockFeatureSwitchFactoryResult);

            #endregion
        }

        [Test]
        public void CreateFeatureSwitch_InvalidationOnFeatureSwitchViewModelObjectCountrySite()
        {
            #region Arrange

            var context = new ValidationContext(_featureSwitchViewModelInvalid.CountrySite, null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchManagerResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelCountrySite(_featureSwitchViewModelInvalid.CountrySite);

            #endregion

            #region Act

            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModelInvalid.CountrySite, context, results, true);

            #endregion

            #region Assert

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchManagerResult);

            #endregion
        }

        #endregion
    }
}