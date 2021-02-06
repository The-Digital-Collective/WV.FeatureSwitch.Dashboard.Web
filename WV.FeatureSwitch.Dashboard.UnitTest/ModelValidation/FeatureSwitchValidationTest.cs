using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WV.FeatureSwitch.Dashboard.BAL.Models;
using WV.FeatureSwitch.Dashboard.UnitTest.Mocks.ApiClientFactory;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.UnitTest.ModelValidation
{
    [TestFixture]
    public class FeatureSwitchValidationTest
    {
        FeatureSwitchViewModel _featureSwitchViewModel = null;        

        [SetUp]
        public void TestInitialize()
        {
            _featureSwitchViewModel = new FeatureSwitchViewModel
            {
                Features = new List<Feature>()
                {
                    new Feature() { Id = 1, Name = "feature", Flag = true }
                },
                CountrySite = "sandbox"
            };
        }


        [Test]
        public void CreateChoosingParty_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_featureSwitchViewModel, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModel, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateFeatureSwitch_ValidationOnFeatureSwitch()
        {
            //Arrange
            var context = new ValidationContext(_featureSwitchViewModel.Features[0], null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeature(_featureSwitchViewModel.Features[0]);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModel.Features[0], context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchFactoryResult);
        }    
        

        [Test]
        public void FeatureSwitchViewModel_ValidationOnFeatureSwitchViewModelFeatureName()
        {
            //Arrange
            var context = new ValidationContext(_featureSwitchViewModel.Features[0].Name, null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchFactoryResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelFeatureName(_featureSwitchViewModel.Features[0].Name);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModel.Features[0].Name, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchFactoryResult);
        }

        [Test]
        public void CreateFeatureSwitch_ValidationOnFeatureSwitchViewModelCountrySite()
        {
            //Arrange
            var context = new ValidationContext(_featureSwitchViewModel.CountrySite, null, null);
            var results = new List<ValidationResult>();
            var mockFeatureSwitchManagerResult = new MockFeatureSwitchFactory().MockValidFeatureSwitchViewModelCountrySite(_featureSwitchViewModel.CountrySite);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_featureSwitchViewModel.CountrySite, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, mockFeatureSwitchManagerResult);
        }
    }
}