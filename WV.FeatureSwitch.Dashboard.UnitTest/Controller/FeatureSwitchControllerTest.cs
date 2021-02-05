using NUnit.Framework;
using WV.FeatureSwitch.Dashboard.Web.Controllers;

namespace WV.FeatureSwitch.Dashboard.UnitTest.Controller
{
    [TestFixture]
    public class FeatureSwitchControllerTest
    {
        private FeatureSwitchController _featureSwitchController;

        [SetUp]
        public void SetUp()
        {

        }

        #region "Index Action"

        /// <summary>
        /// Can we Load all FeatureSwitches?
        /// </summary>
        [Test]
        public void FeatureSwitch_Index_Valid() 
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// If we have no records in FeatureSwitches?
        /// </summary>
        [Test]
        public void FeatureSwitch_Index_NoRecords() 
        {
            // Arrange

            // Act

            // Assert
        }

        #endregion

        #region "Create Action"

        /// <summary>
        /// while we Load a Choosing Event By Id for edit action?
        /// </summary>
        [Test]
        public void ChoosingEvent_CreateLoad_Valid()
        {

        }

        ///// <summary>
        ///// Can we insert a new ChoosingPartyVM?
        ///// </summary>
        [Test]
        public void ChoosingEvent_CreateSave_Valid()
        {

        }

        /// <summary>
        /// Can we insert a new ChoosingPartyVM?
        /// </summary>
        [Test]
        public void ChoosingEvent_CreateSave_DuplicateData()
        {

        }

        #endregion

        #region "Reset Action"



        #endregion

        #region "Delete Action"

        /// <summary>
        /// while we Load a Choosing Event By Id for delete action?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteLoad_SignupExists()
        {

        }

        /// <summary>
        /// while we Load a Choosing Event By Id for delete action?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteLoad_SignupNotExists()
        {

        }

        /// <summary>
        ///  we delete a ChoosingPartyVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteConfirmed_Valid()
        {

        }

        #endregion

    }
}