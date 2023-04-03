namespace TestTollCalculator
{
    [CollectionDefinition("TestTollCalculatorCollection")]
    public class TestTollCalculatorCollection: ICollectionFixture<TestTollCalculatorFixture>
    { }

    public class TestTollCalculatorFixture : IAsyncLifetime
    {
        public IJSONService jsonService;
        public IFeeService feeService;
        public async Task InitializeAsync()
        {
            jsonService = new JSONService();
            feeService = new FeeService(jsonService);
        }

        public async Task DisposeAsync()
        {
            
        }

        
    }
}
