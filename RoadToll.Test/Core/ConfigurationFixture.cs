namespace RoadToll.Test.Core
{
    public class ConfigurationFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ConfigurationFixture()
        {
            ServiceProvider = Startup.ConfigureServices();
        }

        public void Dispose() { }
    }
}
