namespace WV.FeatureSwitch.Dashboard.UnitTest.Mocks.ApiClientFactory
{
    public class MockFeatureSwitchFactory : Mock<IMockFeatureSwitchFactory>
    {
        public async Task<MockFeatureSwitchFactory> MockLoad(ChoosingPartyVM result)
        {

            IList<WorkflowStatusVM> statusVMs = new List<WorkflowStatusVM>();
            Setup(x => x.GetWorkflowStatuses()).Returns(Task.Run(() => statusVMs));
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

    }
}